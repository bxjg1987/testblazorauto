using Abp;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using BXJG.Utils.Share;
using BXJG.Utils.Share.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    //************************EF6 linq中不支持泛型属性的操作************************

    /*
     * code用数据库唯一索引，这样能简化编程
     * 通常大部分数据库支持唯一索引所以这不会有问题。
     * 在分布式场景中，通常也是单库写入，所以也不会有问题
     */

    /// <summary>
    /// 通用树形领域服务
    /// </summary>
    /// <typeparam name="TEntity">通用树形实体类型</typeparam>
    public class GeneralTreeManager<TEntity> : DomainService
        where TEntity : Entity<long>, IGeneralTree<TEntity>// GeneralTreeEntity<TEntity>
    {
        public IRepository<TEntity, long> Repository { get; set; }

        //由于Utils模块是通用模块，所以需要这个来做异步操作，我们的主项目中直接引用ef的，所以不需要
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        public IAbpSession AbpSession { get; set; }

        public GeneralTreeManager()
        {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        //用属性注入，这样子类不用重写烦人的构造函数
        //public GeneralTreeManager(IRepository<TEntity, long> repository) : this()
        //{
        //    this.repository = repository;
        //    //base.LocalizationSourceName = UtilsConsts.LocalizationSourceName;
        //    //this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        //}

        /// <summary>
        /// 往数据库中插入节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            //处理节点数量和code，数据库唯一索引确保并发时code不重复

            TEntity parent = null;
            if (entity.ParentId.HasValue)
            {
                parent = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(c => c.Id == entity.ParentId.Value));
                //  var parentCodeQuery = repository.GetAll().Where(c => c.Id == entity.ParentId.Value).Select(c => c.Code);
                var parentCode = parent.Code;// await AsyncQueryableExecuter.FirstOrDefaultAsync(parentCodeQuery);
                var childrenCount = await Repository.CountAsync(c => c.ParentId == entity.ParentId);
                entity.Code = GeneralTreeExtensions.BuildCode(parentCode, childrenCount);
            }
            else
            {
                var childrenCount = await Repository.CountAsync(c => c.ParentId == null);
                entity.Code = GeneralTreeExtensions.BuildCode("", childrenCount);
            }


            //不晓得为啥abp不自动设置这些值，手动来吧
            if (entity is IMayHaveTenant mayt)
                mayt.TenantId = AbpSession.TenantId;

            if (entity is IMustHaveTenant mustt && AbpSession.TenantId.HasValue)
                mustt.TenantId = AbpSession.TenantId.Value;

            if (entity is ICreationAudited ca)
            {
                ca.CreationTime = Clock.Now;
                ca.CreatorUserId = AbpSession.UserId;
            }

            //  IAudited

            //var ssss = AbpSession;


            await Repository.InsertAsync(entity);
            await base.CurrentUnitOfWork.SaveChangesAsync();
            if (parent != null)
                await UpdateChildrenCount(parent);
            return entity;
        }

        /// <summary>
        /// 更新节点的子节点数量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task UpdateChildrenCount(TEntity entity)
        {
            entity.ChildrenCount = await AsyncQueryableExecuter.CountAsync(Repository.GetAll().Where(c => c.ParentId == entity.Id));
        }
        /// <summary>
        /// 修改节点，内部会自动处理节点移动的问题
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            //正常修改、节点移动、节点数量计算

            //var o = await repository.GetAsync(entity.Id);
            //o.ExtensionData = entity.ExtensionData;
            //o.DisplayName = entity.DisplayName;

            long? newParentId = entity.ParentId;

            var oldIdQuery = Repository.GetAll().Where(c => c.Id == entity.Id).Select(c => c.ParentId);
            var oldId = await AsyncQueryableExecuter.FirstOrDefaultAsync(oldIdQuery);
            var needMove = oldId != entity.ParentId;
            //entity.Code = old.Code;
            //先将新节点的父节点设置为老节点，方便后续处理移动，因为移动时要同时处理新老节点的相邻及其子代节点
            entity.ParentId = oldId;
            ////经过测试，这里又有效
            //if (entity is IHasModificationTime ca)
            //    ca.LastModificationTime = Clock.Now;
            //if(entity is IModificationAudited hc)
            //    hc.LastModifierUserId = AbpSession.UserId;
            await Repository.UpdateAsync(entity);

            await CurrentUnitOfWork.SaveChangesAsync();

            if (needMove)
            {
                if (newParentId.HasValue)
                    await MoveAsync(entity, await Repository.GetAsync(newParentId.Value), GeneralTreeMoveType.Append);
                else
                {
                    var ds = await AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(c => c.ParentId == null).OrderByDescending(c => c.Code));
                    await MoveAsync(entity, ds, GeneralTreeMoveType.After);
                }
            }
            return entity;
        }
        /// <summary>
        /// 删除节点及其后代节点
        /// 目前不会处理相邻节点的code问题，因为不影响查询和排序
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity item)
        {
            // if (keys != null && keys.Length > 0)
            //{
            //默认情况下ParentId的外键关联没有做级联删除，因为我们想通过程序来控制
            // var entities = await repository.GetAllListAsync(c => keys.Contains(c.Id));
            //  HashSet<long> parentIds = new HashSet<long>();
            //  foreach (var item in entities)
            //   {
            //  if (item.ParentId.HasValue)
            //      parentIds.AddIfNotContains(item.ParentId.Value);

            //    if (act != null)
            //      await act(item);
            await Repository.DeleteAsync(c => c.Code.StartsWith(item.Code));

            //}
            await CurrentUnitOfWork.SaveChangesAsync();



            if (item.Parent != null)
                await UpdateChildrenCount(item.Parent);


            //这里后台节点 还需要重置code，以后来改
            //}
            //else
            //{
            //    //await Task.CompletedTask;
            //    await repository.DeleteAsync(c => true);
            //}
        }

        //public virtual async Task<TEntity> GetEntityWithOffspringAsync(long id)
        //{
        //    var parent = await repository.GetAsync(id);
        //    var list = await GetFlattenOffspringAsync(parent.Code);
        //    return parent;//ef自动建立父子关系，
        //}

        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <param name="moveType"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> MoveAsync(long sourceId, long? targetId, GeneralTreeMoveType moveType)
        {
            if (targetId == 0)
                targetId = null;
            var source = await Repository.GetAsync(sourceId);
            var target = targetId.HasValue ? await Repository.GetAsync(targetId.Value) : null;
            return await MoveAsync(source, target, moveType);
        }
        public virtual async Task<TEntity> MoveAsync(TEntity source, TEntity target, GeneralTreeMoveType moveType)
        {
            //保存父节点id，方便后续更新子节点数量
            HashSet<long> parentIds = new HashSet<long>();
            if (source.ParentId.HasValue)
                parentIds.AddIfNotContains(source.ParentId.Value);

            TEntity targetParent = null;//真正的目标父节点
            IList<TEntity> targetChildren = null;//真正的目标父节点的子节点
            int targetIndex = 0;//真正的目标索引

            //if (target == null)
            //{
            //    //if (moveType != GeneralTreeMoveType.Append)
            //    //    throw new UserFriendlyException(L("不能移动顶级节点的前或后面"));

            //    var all = await AsyncQueryableExecuter.ToListAsync(repository.GetAll().OrderByDescending(c => c.Code));
            //    targetChildren = all.Where(c => c.ParentId == null).ToList();
            //    targetIndex = targetChildren.Count;
            //}
            //else

            #region 真正的目标父节点、真正的目标父节点的子节点、真正的目标索引 赋值
            //若不是移动到目标节点内部
            if (moveType != GeneralTreeMoveType.Append)
            {
                if (target.ParentId.HasValue)
                    parentIds.AddIfNotContains(target.ParentId.Value);

                var temp1 = await Repository.GetBrotherWithOffspringAsync(target);
                targetParent = temp1.Item1;
                targetChildren = temp1.Item2;
                targetIndex = targetChildren.IndexOf(target) + (moveType == GeneralTreeMoveType.Front ? 0 : 1);
            }
            else
            {
                parentIds.AddIfNotContains(target.Id);

                targetParent = target;
                //既然是append了，那么target就不可能为空
                // if (target != null)
                // {
                targetChildren = await Repository.GetFlattenOffspringAsync(target.Code);
                targetChildren = targetChildren.Where(c => c.ParentId == target.Id).ToList();
                // }
                // else
                // {
                //     targetChildren = await GetFlattenOffspringAsync();
                //     targetChildren = targetChildren.Where(c => c.ParentId == null).ToList();
                // }
                targetIndex = targetChildren.Count;
            }
            #endregion

            var r = await MoveAsync(source, targetParent, targetChildren, targetIndex);

            await CurrentUnitOfWork.SaveChangesAsync();
            if (parentIds.Any())
            {
                var parents = await AsyncQueryableExecuter.ToListAsync(Repository.GetAll().Where(c => parentIds.Contains(c.Id)));
                foreach (var item in parents)
                {
                    await UpdateChildrenCount(item);
                }
            }


            return r;
        }


        public Task<string> GetCodeAsync(long id)
        {
            return AsyncQueryableExecuter.FirstOrDefaultAsync(Repository.GetAll().Where(c => c.Id == id).Select(c => c.Code));
        }
        //public string GetCode(long id)
        //{
        //    return Repository.GetAll().Where(c => c.Id == id).Select(c => c.Code).Single();
        //}
        //async Task<TEntity> MoveAsync(long sourceId, TEntity target, IList<TEntity> targetList, int targetIndex)
        //{
        //    var source = await repository.GetAsync(sourceId);
        //    return await MoveAsync(source, target, targetList, targetIndex);
        //}

        /*
         * 00001
         *      00001.00001
         *      (目标位置)  后续节点及其后代节点均重新计算code
         *      00001.00002
         *
         * 00002
         *      00002.00001
         *      00002.00002    源位置   后代和后续节点及其后代节点都重新计算code
         *            00002.00001.00001
         *      00002.00003
         */
        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="source">源节点，对应00002.00002</param>
        /// <param name="targetParent">目标位置的父节点，对应00001</param>
        /// <param name="targetBrotherList">目标节点的子节点集合，对应0001.00001、00001.00002</param>
        /// <param name="targetIndex">移动后所处目标的目标位置的索引，对应1</param>
        /// <returns></returns>
        async Task<TEntity> MoveAsync(TEntity source, TEntity targetParent, IList<TEntity> targetBrotherList, int targetIndex)
        {
            //如果调用方让节点移动到原来的位置，这种情况暂时没处理

            var sourceParentCode = source.Code.GetParentCode();//源节点的父节点code

            IList<TEntity> sourceBrotherList; //源节点所在的列表
            bool betweenBrother = false;//是否本身就是在同级节点下移动

            //1、准备源节点的兄弟节点列表 顶层兄弟节点之间移动 或 在某个指定节点下的兄弟节点之间移动
            if ((targetParent == null && !source.ParentId.HasValue) || (targetParent != null && source.ParentId.Equals(targetParent.Id)))
            {
                sourceBrotherList = targetBrotherList;
                betweenBrother = true;
            }
            else
            {
                var temp = await Repository.GetBrotherWithOffspringAsync(source);
                sourceBrotherList = temp.Item2;
            }

            //2、从源列表中移除源节点
            // var tempSource = brotherList.Single(c => c.Id.Equals(source.Id));
            var souteceIndex = sourceBrotherList.IndexOf(source);//source和brotherList集合中存在的那个对象也许不是同一个引用，在EF中有可能是
            sourceBrotherList.RemoveAt(souteceIndex);
            if (source.Parent != null)
                source.Parent.Children = sourceBrotherList;

            //3、将源节点放到目标位置
            if (betweenBrother && targetIndex > souteceIndex)
                targetIndex--;

            if (targetIndex < 0)
                targetIndex = 0;
            if (targetIndex > targetBrotherList.Count)
                targetIndex = targetBrotherList.Count;

            targetBrotherList.Insert(targetIndex, source);

            //5、设置目标的父节点
            if (targetParent == null)
            {
                source.ParentId = null;
                source.Parent = null;
            }
            else
            {
                source.ParentId = targetParent.Id;
                source.Parent = targetParent;
                targetParent.Children = targetBrotherList;
            }

            //6、重置相关节点code
            if (betweenBrother)
            {
                var idx = 0;
                if (targetIndex > souteceIndex)
                    idx = souteceIndex;
                else
                    idx = targetIndex;
                GeneralTreeExtensions.ResetCode(sourceParentCode, targetBrotherList, idx);
            }
            else
            {
                var taregetCode = targetParent == null ? "" : targetParent.Code;
                if (taregetCode.StartsWith(sourceParentCode))
                {
                    GeneralTreeExtensions.ResetCode(sourceParentCode, sourceBrotherList, 0);
                }
                else
                {
                    GeneralTreeExtensions.ResetCode(taregetCode, targetBrotherList, 0);
                }
            }
            //GeneralTreeExtensions.ResetCode(sourceParentCode, sourceBrotherList, souteceIndex);
            //GeneralTreeExtensions.ResetCode(taregetCode, targetBrotherList, targetIndex);
            //source.Parent = target;//千万不要在重新生成code前设置Parent，因为后面递归生成code
            //if (betweenBrother)
            //{
            //    var tempIndex = targetIndex > souteceIndex ? souteceIndex : targetIndex;
            //    ResetCode(taregetCode, targetList, tempIndex);
            //}
            //else
            //{
            //    ResetCode(sourceParentCode, brotherList, souteceIndex);
            //    ResetCode(taregetCode, targetList, targetIndex);
            //}
            return source;
        }
        //async Task<Tuple<TEntity, IList<TEntity>>> GetBrotherWithOffspringAsync(long id)
        //{
        //    return await repository.GetBrotherWithOffspringAsync(await repository.GetAsync(id));

        //}

        //protected class LastOrParent
        //{
        //    public long Id { get; set; }
        //    public string Code { get; set; }
        //    public long? ParentId { get; set; }
        //    public string ParentCode { get; set; }
        //}
        ///// <summary>
        ///// 根据父级id找到最后一个子类，若没找到则返回父类
        ///// 由于父类实体ParentId Id是泛型，在EF的where条件里行不通，所以这个功能延迟到子类去处理
        ///// </summary>
        ///// <param name="parentId"></param>
        ///// <returns></returns>
        //protected abstract Task<LastOrParent> GetLastOrParent(long? parentId);
        ////{
        ////    //var sf = new Nullable<long>(parentId);
        ////    var query = repository.GetAll()
        ////    .Where(c => (c.ParentId.Equals(parentId) || c.Id.Equals(parentId)))
        ////       .OrderByDescending(c => c.Code)
        ////       //经过7小时的折腾，最后发现不能用匿名类
        ////       .Select(c => new LastOrParent { ParentId = c.ParentId, ParentCode = c.Parent.Code, ChildCode = c.Code });

        ////    return await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
        ////}
        /////public virtual async Task<IList<TEntity>> GetByParentId(long id, bool loadProgeny = false)
        //{ 

        //}

        //protected virtual async Task<string> BuildCode(long? parentId)
        //{
        //    var q = await GetLastOrParent(parentId);
        //    if (q == null)
        //        return BuildCode("", "");
        //    else if (parentId.Equals(q.Id)) //拿到的父节点
        //        return BuildCodeByLastCode(q.Code, "");
        //    else
        //        return BuildCodeByLastCode(q.ParentCode, q.Code);
        //}
        //protected async Task<string> BuildCode(string parentCode)
        //{
        //    if (parentCode.IsNullOrWhiteSpace())
        //        return BuildCodeByLastCode("", "");

        //    var query = repository.GetAll()
        //        .Where(c => c.Code.StartsWith(parentCode))
        //        .OrderByDescending(c => c.Code)
        //        .Select(c => c.Code);

        //    var q = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
        //    if (q == parentCode)
        //        q = "";

        //    return BuildCodeByLastCode(parentCode, q);
        //}
    }

}
