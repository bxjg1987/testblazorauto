using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using BXJG.Common.Contracts;
using BXJG.Utils.Extensions;
using BXJG.Utils.Files;
using BXJG.Utils.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.Domain.Repositories
{
    public static class RepositoryExt
    {
        #region 附件

        //这里只定义常用场景需要的查询，更多场景不要定义扩展，而是直接在需要时自己查询
        //尽量别用这些扩展方法，封装得太深了不好

        /// <summary>
        /// 获取单个实体的一个或多个属性的附件列表
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="entityType">实体类型，若实体id是全局唯一的，比如guid，则此参数可省略</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>key属性名，value文件列表</returns>
        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static async Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository,
                                                                                            string entityId,
                                                                                            string? entityType = default,
                                                                                            bool track = false,
                                                                                            CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = (await repository.GetAllAsync()).WhereAttachment(entityType, default, track, entityId);
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.PropertyName).ToDictionary(x => x.Key, x => x.OrderBy(c => c.OrderIndex).Select(c => c.File).ToList());
        }

        //这种少用，最好始终给实体附件设置属性名称，不然查询是否忽略属性名这里逻辑麻烦
        ///// <summary>
        ///// 从附件中获取文件，忽略propertyName
        ///// </summary>
        ///// <param name="repository"></param>
        ///// <param name="entityType">实体类型</param>
        ///// <param name="entityId">实体id</param>
        ///// <param name="track"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns>文件列表</returns>
        //public static async Task<List<FileEntity>> GetFilesByAttachmentWithoutProperty(this IRepository<AttachmentEntity, Guid> repository, string entityType, string entityId, bool track = false, CancellationToken cancellationToken = default)
        //{
        //    IQueryable<AttachmentEntity> q = (await repository.GetAllAsync()).WhereAttachment(entityType, default, track, entityId).Where(x=>x.PropertyName==string.Empty);
        //    return await q.OrderBy(x => x.OrderIndex).Select(x => x.File).ToListAsync(cancellationToken);
        //}


        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：属性名和文件列表</returns>

        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static async Task<Dictionary<string, Dictionary<string, List<FileEntity>>>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, string? entityType = default, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = (await repository.GetAllAsync()).WhereAttachment(entityType, default, track, entityIds.ToArray());
            var list = await q.ToArrayAsync(cancellationToken);
            return list.GroupBy(x => x.EntityId)
                       .ToDictionary(x => x.Key,
                                     x => x.GroupBy(y => y.PropertyName)
                                           .ToDictionary(z => z.Key,
                                                         z => z.OrderBy(v => v.OrderIndex).Select(g => g.File).ToList()));
        }
        ///// <summary>
        ///// 从附件中获取文件，忽略propertyName
        ///// </summary>
        ///// <param name="repository"></param>
        ///// <param name="entityType">实体类型</param>
        ///// <param name="entityIds">实体id</param>
        ///// <param name="track"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns> key实体id；value：文件列表</returns>
        //public static async Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty(this IRepository<AttachmentEntity, Guid> repository, string entityType, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        //{
        //    IQueryable<AttachmentEntity> q = (await repository.GetAllAsync()).WhereAttachment(entityType, default, track, entityIds.ToArray());
        //    var list = await q.ToArrayAsync(cancellationToken);
        //    return list.GroupBy(x => x.EntityId).ToDictionary(x => x.Key, z => z.OrderBy(v => v.OrderIndex).Select(g => g.File).ToList());
        //}





        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体id</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>

        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static async Task<List<FileEntity>> GetFilesByAttachment(this IRepository<AttachmentEntity, Guid> repository, string entityId, string? entityType = default, string propertyName = default, bool track = false, CancellationToken cancellationToken = default)
        {
            IQueryable<AttachmentEntity> q = (await repository.GetAllAsync()).WhereAttachment(entityType, propertyName, track, entityId).OrderBy(x => x.OrderIndex);
            var list = await q.ToArrayAsync(cancellationToken);
            return list.Select(x => x.File).ToList();
        }



        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>key属性名，value文件列表</returns>

        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(entityId, typeof(TEntity).FullName, track, cancellationToken);
        }
        ///// <summary>
        ///// 从附件中获取文件，忽略propertyName
        ///// </summary>
        ///// <param name="repository"></param>
        ///// <param name="entityId">实体id</param>
        ///// <param name="track"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns>文件列表</returns>
        //public static Task<List<FileEntity>> GetFilesByAttachmentWithoutProperty<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, bool track = false, CancellationToken cancellationToken = default)
        //{
        //    return repository.GetFilesByAttachmentWithoutProperty(typeof(TEntity).FullName, entityId, track, cancellationToken);
        //}

        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityIds">实体id</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> key实体id；value：属性名和文件列表</returns>

        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static Task<Dictionary<string, Dictionary<string, List<FileEntity>>>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(entityIds, typeof(TEntity).FullName, track, cancellationToken);
        }

        ///// <summary>
        ///// 从附件中获取文件，忽略propertyName
        ///// </summary>
        ///// <param name="repository"></param>
        ///// <param name="entityIds">实体id</param>
        ///// <param name="track"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns> key实体id；value：文件列表</returns>
        //public static Task<Dictionary<string, List<FileEntity>>> GetFilesByAttachmentWithoutProperty<TEntity>(this IRepository<AttachmentEntity, Guid> repository, IEnumerable<string> entityIds, bool track = false, CancellationToken cancellationToken = default)
        //{
        //    return repository.GetFilesByAttachmentWithoutProperty(typeof(TEntity).FullName, entityIds, track, cancellationToken);
        //}


        /// <summary>
        /// 从附件中获取文件
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="entityId">实体id</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="track"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>文件列表</returns>

        [Obsolete("尽管有用，但是封装层次太多了不好，建议直接用IQueryable<AttachmentEntity>的扩展方法")]
        public static Task<List<FileEntity>> GetFilesByAttachment<TEntity>(this IRepository<AttachmentEntity, Guid> repository, string entityId, string propertyName = default, bool track = false, CancellationToken cancellationToken = default)
        {
            return repository.GetFilesByAttachment(entityId, typeof(TEntity).FullName, propertyName, track, cancellationToken);
        }



        #endregion

        #region 树
        /// <summary>
        /// 根据code获取所有后代节点，并以平铺结构的集合返回
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<List<TEntity>> GetFlattenOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, string code = null) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var query = (await repository.GetAllAsync());
            if (!code.IsNullOrWhiteSpace())
                query = query.Where(c => c.Code.StartsWith(code));
            query = query.OrderBy(c => c.Code);
            return await query.ToListAsync();// AsyncQueryableExecuter.ToListAsync(query);
        }

        //public static string GetParentCode(this string code)
        //{
        //    if (code.Length == Share.BXJGUtilsConsts.CodeUnitLength)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return code.Substring(0, code.Length - Share.BXJGUtilsConsts.CodeUnitLength).TrimEnd('.');
        //    }
        //}

        /// <summary>
        /// 获取指定节点的兄弟节点、父节点(可能为空)
        /// 均包含其后代节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Item1父节点，Item2兄弟节点</returns>
        public static async Task<Tuple<TEntity, List<TEntity>>> GetBrotherWithOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, TEntity entity) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var parentCode = entity.Code.GetParentCode();

            TEntity parent = null;
            List<TEntity> children;

            if (!Abp.Extensions.StringExtensions.IsNullOrWhiteSpace(parentCode))
            {
                children = await repository.GetFlattenOffspringAsync<TEntity>(parentCode);
                parent = children[0];
            }
            else
            {
                //本来也可以用上面的StartsWith，但是直接getAll性能更好
                children = await repository.GetFlattenOffspringAsync();
            }
            children = children.Where(c => c.ParentId.Equals(entity.ParentId)).ToList();
            return new Tuple<TEntity, List<TEntity>>(parent, children);
        }

        #endregion

        #region 杂项
        /// <summary>
        /// 判断指定条件的数据是否已存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> IsExists<TEntity>(this IRepository<TEntity> q, Expression<Func<TEntity, bool>> w, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            return await (await q.GetAllReadonlyAsync()).AnyAsync(w, cancellationToken);
        }
        /// <summary>
        /// 若指定条件的数据已存在，则抛出异常
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <param name="msg"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task IsExistsThrow<TEntity>(this IRepository<TEntity> q, Expression<Func<TEntity, bool>> w, string msg = default, CancellationToken cancellationToken = default) where TEntity : class, IEntity
        {
            if (await q.IsExists(w, cancellationToken))
            {
                if (msg.IsNullOrWhiteSpace())
                    msg = BXJGUtilsLocalizationExt.UtilsL("数据已存在！");
                throw new UserFriendlyException(msg);
            }
        }
        /// <summary>
        /// 判断指定条件的数据是否已存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="q"></param>
        /// <param name="w"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<bool> IsExists<TEntity, TKey>(this IRepository<TEntity, TKey> q, Expression<Func<TEntity, bool>> w, CancellationToken cancellationToken = default) where TEntity : class, IEntity<TKey>
        {
            return await (await q.GetAllReadonlyAsync()).AnyAsync(w, cancellationToken);
        }
        /// <summary>
        /// 若指定条件的数据已存在，则抛出异常
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="repository">仓储实例</param>
        /// <param name="where"></param>
        /// <param name="displayNameProperty"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task IsExistsThrow<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
                                                              Expression<Func<TEntity, bool>> where,
                                                              Expression<Func<TEntity, string>> displayNameProperty,
                                                              CancellationToken cancellationToken = default) where TEntity : class, IEntity<TKey>
        {
            var cx = await repository.GetAllReadonlyAsync();
            var list = await cx.Where(where).Select(displayNameProperty).Distinct().ToArrayAsync(cancellationToken);
            if (list.Length == 0)
                return;

            var ss = "存在重复项！" + string.Join(",", list);
            UserFriendlyExceptionFactory.Throw(ss);

            //if (await q.IsExists(w, cancellationToken))
            //{
            //    if (msg.IsNullOrWhiteSpace())
            //        msg = BXJGUtilsLocalizationExt.UtilsL("数据已存在！");
            //    throw new UserFriendlyException(msg);
            //}
        }
        /// <summary>
        /// 获取重复项
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TKey">实体主键类型</typeparam>
        /// <param name="repository">仓储实例</param>
        /// <param name="where">条件</param>
        /// <param name="displayNameProperty">重复项的显示名</param>
        /// <param name="cancellationToken">异步取消token</param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        public static async Task<List<IdName<TKey>>> GetExists<TEntity, TKey>(this IRepository<TEntity, TKey> repository,
                                                                              Expression<Func<TEntity, bool>> where,
                                                                              Func<TEntity, string> displayNameProperty,
                                                                              CancellationToken cancellationToken = default) where TEntity : class, IEntity<TKey>
        {
            var cx = await repository.GetAllReadonlyAsync();
            var list = await cx.Where(where).ToArrayAsync(cancellationToken);
            return list.Select(x => new IdName<TKey>(x.Id, displayNameProperty.Invoke(x))).ToList();
        }

        #region abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// <summary>
        /// abp仓储的默认实现目前的删除是查询出来之后再删除，数据量大时有问题，已经提交了issue，这里是临时解决方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="q"></param>
        /// <param name="where"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> BatchDelete<T, TKey>(this IRepository<T, TKey> q, Expression<Func<T, bool>> where, CancellationToken cancellationToken = default) where T : class, IEntity<TKey>
        {
            var x = await q.GetAllAsync();
            return await x.Where(where).BatchDelete(cancellationToken);
        }

        #endregion

        #endregion
    }
}