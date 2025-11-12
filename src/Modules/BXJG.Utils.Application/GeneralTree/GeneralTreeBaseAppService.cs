/*
 * 作者：变形精怪 手机/微信17723896676 QQ/邮箱453008453
 * 创建时间：2018-10-10 22:49:57
 *
 * 说明：略...
 */
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.UI;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Domain.Uow;
using Abp.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Components.Forms;
using BXJG.Common;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.GeneralTree;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.Dtos;
using Abp.Auditing;
using System.Linq.Expressions;

namespace BXJG.Utils.Application.GeneralTree
{
    /*
     * 原本很多地方返回了不必要的ValuTask，比如返回IQueryable的虚方法，最初的目的是希望允许子类有机会直接使用异步
     * 但那些地方使用异步的机会特别少，反而由于返回ValuTask而导致子类编写起来更麻烦，无法使用链式编程
     * 所以决定去掉ValuTask，若必须使用异步时，应在重写外层，如：GetAllAsync，然后在内部异步处理数据，若需要传递参数则使用当前Uow.Items
     */

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    /// <typeparam name="TGetNodesForSelectOutput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    [UnitOfWork(false)]
    [DisableAuditing]
    public abstract class GeneralTreeProviderBaseAppService<TEntity,
                                                   TGetTreeForSelectInput,
                                                   TGetTreeForSelectOutput,
                                                   TGetNodesForSelectInput,
                                                   TGetNodesForSelectOutput> : BXJGUtilsBaseAppService, IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                   TGetTreeForSelectOutput,
                                                                                                                   TGetNodesForSelectInput,
                                                                                                                   TGetNodesForSelectOutput>
    where TEntity : Entity<long>, IGeneralTree<TEntity>// GeneralTreeEntity<TEntity>
    where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
    where TGetTreeForSelectOutput : GeneralTreeNodeForSelectDto<TGetTreeForSelectOutput>//, new()
    where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
    where TGetNodesForSelectOutput : GeneralTreeComboboxDto//, new()
    {
        // protected string allTextForSearch, allTextForForm;//注意这里代表的是本地化文本的key

        public virtual string allTextForSearch { get; set; } = "不限";
        public virtual string allTextForForm { get; set; } = "请选择";

        //public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        public IRepository<TEntity, long> Repository { get; set; }
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
        
        //[Obsolete("子类继承时应使用无参的构造函数，当前构造函数是未简化子类实现以前的代码，已过时。")]
        //public GeneralTreeProviderBaseAppService(IRepository<TEntity, long> repository,
        //                                         string allTextForSearch = "不限",
        //                                         string allTextForForm = "请选择")//这里的字符串后期可以使用常量
        //{
        //    //base.LocalizationSourceName = UtilsConsts.LocalizationSourceName;
        //    this.Repository = repository;
        //    this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        //    //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
        //    this.allTextForSearch = allTextForSearch;
        //    this.allTextForForm = allTextForForm;
        //}

        public GeneralTreeProviderBaseAppService()
        {
            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public virtual string PermissionName { get; set; }
        protected virtual async Task CheckGetPermissionAsync()
        {
            if (PermissionName.IsNotNullOrWhiteSpaceBXJG())
                await base.PermissionChecker.AuthorizeAsync(PermissionName);
        }
        /// <summary>
        /// 获取树形的下拉框数据，不需要身份验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[UnitOfWork(false)]
        public virtual async Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input)
        {
            //权限判断
            await CheckGetPermissionAsync();


            //得到实体扁平集合
            string parentCode = "";
            if (input.ParentName.IsNotNullOrWhiteSpaceBXJG())
            {
                var top = await (await Repository.GetAllAsync()).Where(c => c.Name == input.ParentName).Select(x => new { x.Code, x.Id }).SingleAsync(CancellationTokenProvider.Token);
                parentCode = top.Code;
                input.ParentId = top.Id;
            }
            else if (input.ParentCode.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                //var top = await Repository.GetAsync(input.ParentId.Value);
                // parentCode = top.Code;
                var code = await (await Repository.GetAllAsync()).Where(c => c.Name == input.ParentName).Select(x => x.Code).SingleAsync(CancellationTokenProvider.Token);
                parentCode = code;
            }
            else
                parentCode = input.ParentCode ?? "";

            var query = await ComboTreeFilter(input, parentCode);
            query = ComboTreeSort(query, input);


            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = EntityToTreeDto(list);// ObjectMapper.Map<List<TGetTreeForSelectOutput>>(list);

            //if (ComboTreeMap != null)
            //{
            //    dtoList.ForEach(c =>
            //    {
            //        var entity = list.Single(d => d.Id.ToString() == c.Id);

            //        //这俩属性通过属性的方式处理了
            //        //c.state = "closed";//默认值为 open
            //        //c.attributes.extData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

            //        ComboTreeMap(entity, c);
            //    });
            //}
            //TGetTreeForSelectOutput parentDto;
            if (input.ParentId.HasValue)
            {
                //parentDto = dtoList.SingleOrDefault(c => c.Id == input.ParentId);
                dtoList = dtoList.Where(c => c.ParentId == input.ParentId).ToList();
            }
            else
            {
                //parentDto = null;
                dtoList = dtoList.Where(c => !c.ParentId.HasValue).ToList();
            }


            ////通用树是通过继承来实现扩展的，所以这里L引用的本地化源可能被子类重写，因此这里用L是可以的
            //if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
            //{
            //    dtoList.Insert(0, new TGetTreeForSelectOutput { Id = default, Text = input.ParentText });
            //    return dtoList;
            //}
            ////return new List<TGetTreeForSelectOutput> { new TGetTreeForSelectOutput { Id = null, Text = L(input.ParentText), Children = dtoList } };

            //if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
            //{
            //    parentDto.Text = "==" + parentDto.Text + "==";
            //    parentDto.Id = default;
            //    return new List<TGetTreeForSelectOutput> { parentDto };
            //}


            //if (input.ForType == 1 || input.ForType == 2)
            //{
            //    dtoList.Insert(0, new TGetTreeForSelectOutput { Id = default, Text = allTextForSearch });
            //    return dtoList;
            //}
            ////return new List<TGetTreeForSelectOutput> { new TGetTreeForSelectOutput { Id = null, Text = this.allTextForSearch, Children = dtoList } };

            //if (input.ForType == 3 || input.ForType == 4)
            //{
            //    dtoList.Insert(0, new TGetTreeForSelectOutput { Id = default, Text = allTextForForm });
            //    return dtoList;
            //}
            // return new List<TGetTreeForSelectOutput> { new TGetTreeForSelectOutput { Id = null, Text = this.allTextForForm, Children = dtoList } };

            return dtoList;
        }
        /// <summary>
        /// 获取扁平化的下拉框数据，不需要身份验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[UnitOfWork(false)]
        public virtual async Task<IList<TGetNodesForSelectOutput>> GetNodesForSelectAsync(TGetNodesForSelectInput input)
        {
            await CheckGetPermissionAsync();
            //得到实体扁平集合
            //string parentCode = "";
            //if (input.ParentId.HasValue && input.ParentId.Value > 0)
            //{
            //    var top = await ownRepository.GetAsync(input.ParentId.Value);
            //    parentCode = top.Code;
            //}

            //var ppid = input.ParentId;
            if (input.ParentName.IsNotNullOrWhiteSpaceBXJG())
            {
                var top = await (await Repository.GetAllAsync()).Where(c => c.Name == input.ParentName).Select(x=>x.Id).SingleAsync(CancellationTokenProvider.Token);
                input.ParentId = top;
            }
            var query = await ComboboxFilter(input, input.ParentId);

            query = ComboboxSort(input, query);
            //GetNodesForSelectProjection允许子类直接投影，这种情况可能不太灵活，因为子类可能不方便做ef投影，所以将来可能考虑完全获取实体，在内存中来做这个转换

            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = EntityToComboboDto(list);// ObjectMapper.Map<List<TGetNodesForSelectOutput>>(list);

            //if (ComboboxMap != null)
            //{
            //    dtoList.ForEach(c =>
            //    {
            //        var entity = list.Single(d => d.Id.ToString() == c.Value);

            //        //这俩属性通过属性的方式处理了
            //        //c.state = "closed";//默认值为 open
            //        //c.attributes.extData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

            //        ComboboxMap(entity, c);
            //    });

            //}

            //var dtoList = await ComboboxProjectionAsync(query);
            var parentDto = input.ParentId.HasValue ? dtoList.SingleOrDefault(c => c.Value == input.ParentId.ToString()) : null;
            if (parentDto != null)
            {
                dtoList.Remove(parentDto);
                //parentDto.Value = null;
                //parentDto.DisplayText = "==" + parentDto.DisplayText + "==";
            }
            //dtoList = dtoList.Where(c => c.Value != input.Id).ToList();

            //if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
            //    dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = input.ParentText });
            //else if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
            //    dtoList.Insert(0, parentDto);
            //else if (input.ForType == 1 || input.ForType == 2)
            //    dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForSearch });
            //else if (input.ForType == 3 || input.ForType == 4)
            //    dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForForm });

            return dtoList;
        }

        /// <summary>
        /// 获取单个 扁平和树形列表都会调用，以获取查询对象
        /// 可以重写以应用所有查询都需要的Include
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildQuery()
        {
            return (await Repository.GetAllAsync()).Include(c => c.Parent).AsNoTrackingWithIdentityResolution();
        }

        #region 获取树形下拉框数据时子类可以重写的方法
        /// <summary>
        /// 获取树形数据的queryable，默认StartsWith parentCode
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parentCode"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> ComboTreeFilter(TGetTreeForSelectInput input, string parentCode)
        {
            var q = (await BuildQuery()).WhereIf(!input.IsOnlyLoadChild, c => c.Code.StartsWith(parentCode))
                                .WhereIf(input.IsOnlyLoadChild && parentCode.IsNotNullOrWhiteSpaceBXJG(), c => c.Parent.Code == parentCode || c.Code == parentCode)
                                .WhereIf(input.IsOnlyLoadChild && parentCode.IsNullOrWhiteSpaceBXJG(), c => !c.ParentId.HasValue);


            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;
        }
        /// <summary>
        /// 获取树形数据的排序，默认按code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ComboTreeSort(IQueryable<TEntity> query, TGetTreeForSelectInput input)
        {
            return query.OrderBy(c => c.Code);
        }
        /// <summary>
        /// 实体转换为dto时调用，默认使用automapper
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual List<TGetTreeForSelectOutput> EntityToTreeDto(IEnumerable<TEntity> entities)
        {
            return ObjectMapper.Map<List<TGetTreeForSelectOutput>>(entities);
        }
        #endregion
        #region 获取扁平化下拉框数据时子类可重写的方法
        /// <summary>
        /// 获取扁平化下拉框数据queryable，默认ParentId等于parentId
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> ComboboxFilter(TGetNodesForSelectInput input, long? parentId)
        {
            var q = (await BuildQuery()).Where(c => c.ParentId == parentId);
            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;
            //return ownRepository.GetAll().Where(c => c.ParentId == input.ParentId || c.Id == input.ParentId);
        }
        /// <summary>
        /// 获取扁平化下拉框数据的排序，默认code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ComboboxSort(TGetNodesForSelectInput input, IQueryable<TEntity> query)
        {
            return query.OrderBy(c => c.Code);
        }
        //protected virtual async Task<IList<TGetNodesForSelectOutput>> ComboboxProjectionAsync(IQueryable<TEntity> query)
        //{
        //    //var q = query.Select(c => new TGetNodesForSelectOutput { ExtDataString = c.ExtensionData, DisplayText = c.DisplayName, Value = c.Id.ToString() });
        //    //return await AsyncQueryableExecuter.ToListAsync(q);

        //    var list = await AsyncQueryableExecuter.ToListAsync(query);
        //    var dtos = new List<TGetNodesForSelectOutput>();
        //    foreach (var item in list)
        //    {
        //        var dto = new TGetNodesForSelectOutput()
        //        {
        //            Code = item.Code,
        //            DisplayText = item.DisplayName,
        //            ExtDataString = item.ExtensionData,
        //            Value = item.Id.ToString()
        //        };
        //        ComboboxMap(item, dto);
        //        dtos.Add(dto);
        //    }
        //    return dtos;
        //}
        /// <summary>
        /// 实体转换为dto时调用，默认使用automapper
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        protected virtual List<TGetNodesForSelectOutput> EntityToComboboDto(IEnumerable<TEntity> entities)
        {
            return ObjectMapper.Map<List<TGetNodesForSelectOutput>>(entities);
        }
        #endregion
    }

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    public abstract class GeneralTreeProviderBaseAppService<TEntity,
                                                   TGetTreeForSelectInput,
                                                   TGetTreeForSelectOutput,
                                                   TGetNodesForSelectInput> : GeneralTreeProviderBaseAppService<TEntity,
                                                                                                                TGetTreeForSelectInput,
                                                                                                                TGetTreeForSelectOutput,
                                                                                                                TGetNodesForSelectInput,
                                                                                                                GeneralTreeComboboxDto>, IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                                                                            TGetTreeForSelectOutput,
                                                                                                                                                                            TGetNodesForSelectInput>
        where TEntity : GeneralTreeEntity<TEntity>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeForSelectDto<TGetTreeForSelectOutput>, new()
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
    {
    }

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    public abstract class GeneralTreeProviderBaseAppService<TEntity,
                                                   TGetTreeForSelectInput,
                                                   TGetTreeForSelectOutput> : GeneralTreeProviderBaseAppService<TEntity,
                                                                                                                TGetTreeForSelectInput,
                                                                                                                TGetTreeForSelectOutput,
                                                                                                                TGetTreeForSelectInput>, IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                                                                                                                                            TGetTreeForSelectOutput>
        where TEntity : GeneralTreeEntity<TEntity>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeForSelectDto<TGetTreeForSelectOutput>, new()
    {
    }

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    public abstract class GeneralTreeProviderBaseAppService<TEntity,
                                                   TGetTreeForSelectInput> : GeneralTreeProviderBaseAppService<TEntity,
                                                                                                               TGetTreeForSelectInput,
                                                                                                               DataDictionaryForSelectDto>, IGeneralTreeProviderBaseAppService<TGetTreeForSelectInput>
        where TEntity : GeneralTreeEntity<TEntity>
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
    {
    }

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class GeneralTreeProviderBaseAppService<TEntity> : GeneralTreeProviderBaseAppService<TEntity,
                                                                                                GeneralTreeGetForSelectInput>, IGeneralTreeProviderBaseAppService
        where TEntity : GeneralTreeEntity<TEntity>
    {
    }

    /// <summary>
    /// 通用的树形结构的数据的crud抽象服务（完整）
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    [UnitOfWork]//在blazor server中，加这个更保险
    public abstract class GeneralTreeBaseAppService<TEntity,
                                           TDto,
                                           TCreateInput,
                                           TEditDto,
                                           TGetAllInput,
                                           TManager> : BXJGUtilsBaseAppService, IGeneralTreeBaseAppService<TDto,
                                                                                                           TCreateInput,
                                                                                                           TEditDto,
                                                                                                           TGetAllInput>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntity : Entity<long>, IGeneralTree<TEntity>// GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TManager : GeneralTreeManager<TEntity>
        where TGetAllInput : GeneralTreeGetTreeInput
    {
        //Zhongjie仅用于界面，业务逻辑层任然使用abp的事件总线（它不是为界面设计的，默认也没提供多个实例），在ui提供abpk事件处理器 来连接到zhongjie实例
        //public Zhongjie Zhongjie { get; set; }
        /* 
         * 数据显示地方有：管理页列表、作为一个搜索条件框、作为表单里一个下拉框
         * 顶级文本可能是 前端传过来的、上级节点文本、默认文本；除非根本不现实
         */
        //public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        /// <summary>
        /// 仓储
        /// </summary>
        public IRepository<TEntity, long> Repository { get; set; }
        /// <summary>
        /// 领域服务
        /// </summary>
        public TManager GeneralTreeManager { get; set; }
        /// <summary>
        /// 分布式锁帮助器
        /// 通常在应用服务方法的中间部分用，少数情况在领域服务方法中也可以
        /// uow释放后会自动释放锁
        /// </summary>
        public DistributedLockHelper DistributedLockHelper { get; set; }
        /// <summary>
        /// 新增时的重复检查，返回null则不检查，默认情况下引用<see cref="GetUpdateIsExistsChenker(TUpdateInput)"/>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<ExistsExpression<TEntity>> GetCreateIsExistsChenker(TCreateInput input) => (input is TEditDto a)? GetUpdateIsExistsChenker(a): ValueTask.FromResult<ExistsExpression<TEntity>>(null);
        /// <summary>
        /// 修改时的重复检查，返回null则不检查
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask<ExistsExpression<TEntity>> GetUpdateIsExistsChenker(TEditDto input)=> ValueTask.FromResult<ExistsExpression<TEntity>>(null);
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。能不用就不用
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// 保险起见，使用时创建个范围
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
        //protected string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName;
        /// <summary>
        /// 新增权限名称，通常需要重写
        /// </summary>
        protected virtual string CreatePermissionName { get; }
        /// <summary>
        /// 修改权限名称，通常需要重写
        /// </summary>
        protected virtual string UpdatePermissionName { get; }
        /// <summary>
        /// 删除权限名称，通常需要重写
        /// </summary>
        protected virtual string DeletePermissionName { get; }
        /// <summary>
        /// 获取权限名称，通常需要重写
        /// </summary>
        protected virtual string GetPermissionName { get; }

        #region create
        /// <summary>
        /// 创建树形结构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();
            var cfjc = await GetCreateIsExistsChenker(input);
            if (cfjc != null)
            {
                //加锁注意是为了防止重复提交，数据库唯一约束局限性太大，比如软删除冲突，另外在代码层面做了，数据库见表也省得到处去建唯一索引
                await DistributedLockHelper.AcquireLockTenantAsync(typeof(TEntity).FullName, TimeSpan.FromMinutes(1), CancellationTokenProvider.Token);
                //要完美的话，前端还应该先判断，后端是最后的保障
                await Repository.IsExistsThrow(cfjc.Where,cfjc.DisplayNameProperty);
            }

            // var ctx = new Dictionary<string, object> { { "input", input } };
            var m = MapToEntity(input);// ObjectMapper.Map<TEntity>(input);



            //await BeforeCreateAsync(input, m);
            await MapToEntity(m);
            //await GeneralTreeManager.CreateAsync(m);
            await CreateCore(m, input);

            //m = await GetEntityByIdAsync(m.Id, false);
            //return EntityToDto(m);
            return await CreateAfter(m);
        }
        /// <summary>
        /// 若你希望使用自己的manager插入，请重写此方法
        /// 通用树本身就是使用manager插入的，所以它不需要这样的设计
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual Task CreateCore(TEntity entity, TCreateInput input)
        {
            return GeneralTreeManager.CreateAsync(entity);
        }
        protected virtual Task<TDto> CreateAfter(TEntity entity)
        {
            return GetDtoById(entity);
        }



        /// <summary>
        /// 新增时的映射，默认使用automapper映射
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual TEntity MapToEntity(TCreateInput input)
        {
            var entity = ObjectMapper.Map<TEntity>(input);
            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null && entity is IExtendableObject kk)
            {
                foreach (var item in input.ExtData)
                {
                    kk.RemoveData(item.Key);
                    kk.SetData(item.Key, item.Value);
                }
            }
            return entity;
        }
        #endregion

        #region move
        /// <summary>
        /// 移动树形结构数据的节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> MoveAsync(GeneralTreeNodeMoveInput input)
        {
            //移动关于追加 之后 之前 的处理逻辑本应该定义在领域服务中
            await CheckUpdatePermissionAsync();
            var m = await GeneralTreeManager.MoveAsync(input.Id, input.TargetId, input.MoveType);
            return EntityToDto(m);
        }
        #endregion

        #region update
        /// <summary>
        /// 更新树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> UpdateAsync(TEditDto input)
        {
            //var sdf = input.Pci.inputText;

            await CheckUpdatePermissionAsync();

            var cfjc = await GetUpdateIsExistsChenker(input);
            if (cfjc != null)
            {
                //加锁注意是为了防止重复提交，数据库唯一约束局限性太大，比如软删除冲突，另外在代码层面做了，数据库见表也省得到处去建唯一索引
                await DistributedLockHelper.AcquireLockTenantAsync(typeof(TEntity).FullName, TimeSpan.FromMinutes(1), CancellationTokenProvider.Token);
                //cfjc.And(c => c.Id != input.Id);
                //要完美的话，前端还应该先判断，后端是最后的保障
                await Repository.IsExistsThrow(cfjc.Where.And(x => !x.Id.Equals(input.Id)), cfjc.DisplayNameProperty);
            }

            //  var ctx = new Dictionary<string, object> { { "input", input } };
            var m = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, m);

            await MapToEntity(m);
            await GeneralTreeManager.UpdateAsync(m);
            //m = await GetEntityByIdAsync(m.Id, false);
            //return EntityToDto(m);
            return await UpdateAfter(m);
        }
        protected virtual Task<TDto> UpdateAfter(TEntity entity)
        {
            return GetDtoById(entity);
        }
        ///// <summary>
        ///// 修改时的查询，默认根据id查询
        ///// 除了修改，其它地方也可能使用，等同于crudappservcie的GetEntityById，只是没把名词改过来
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="context"><see cref="UpdateAsync"/>不要再使用此参数，请直接使用uow.Items</param>
        ///// <returns></returns>
        //[Obsolete("请使用GetEntityByIdAsyn")]
        //protected virtual ValueTask<IQueryable<TEntity>> UpdateGetAsync(TEditDto input, IDictionary<string, object> context = default)
        //{
        //    return ValueTask.FromResult(repository.GetAll().Where(c => c.Id == input.Id));
        //}
        /// <summary>
        /// 修改时的映射，默认使用automapper
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual void MapToEntity(TEditDto input, TEntity entity)
        {
            ObjectMapper.Map(input, entity);

            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null && entity is IExtendableObject kk)
            {
                foreach (var item in input.ExtData)
                {
                    kk.RemoveData(item.Key);
                    kk.SetData(item.Key, item.Value);
                }
            }
        }

        #endregion

        #region delete
        [UnitOfWork(IsDisabled = true)]
        public async Task<BatchOperationOutputLong> DeleteBatchAsync(BatchOperationInputLong input)
        {
            await CheckDeletePermissionAsync();
            return await BatchHandleAsync(input.Ids,async x=> await DeleteCore(x));
        }
        /// <summary>
        /// 删除树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(EntityDto<long> input)
        {
            await CheckDeletePermissionAsync();
            var entity = await GetEntityByIdAsync(input.Id);
            await DeleteCore(entity);
        }
        /// <summary>
        /// 批量或单个删除的核心逻辑，默认当前及其后代节点都删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async Task DeleteCore(TEntity entity)
        {
             await GeneralTreeManager.DeleteAsync(entity);
        }
        #endregion

        #region get
        /// <summary>
        /// 获取指定节点的树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<TDto> GetAsync(EntityDto<long> input)
        {
            await CheckGetPermissionAsync();
            //var entity = await GetEntityByIdAsync(input.Id, false);
            //return EntityToDto(entity);
          return await  GetDtoById(default, input.Id);
        }
        #endregion

        #region getall
        /// <summary>
        /// 获取所有树形结构的数据，以树形层次结构返回
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<TDto>> GetAllAsync(TGetAllInput input)
        {
            //权限判断
            await CheckGetPermissionAsync();

            //获取父节点的code 方便后续查询所有子集
            string parentCode = "";

            if (input.ParentCode.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await Repository.SingleAsync(c => c.Id == input.ParentId.Value);
                parentCode = top.Code;
            }
            else
                parentCode = input.ParentCode ?? "";

            // var ctx = new Dictionary<string, object> { { "input", input } };
            //查询

            var query = await BuildQuery(false);
            //query = GetAllInclude(query);
            query = GetAllFilter(query, input, parentCode);//.Where(c => c.Code.StartsWith(parentCode));
            query = GetAllSort(query, input); //方便子类排序
            var list = await AsyncQueryableExecuter.ToListAsync(query);//.ToListAsync();
            if (!input.ParentId.HasValue)
                input.ParentId = list.OrderBy(c => c.Code.Length).FirstOrDefault()?.ParentId;
            //建立dto以及处理父子关系
            //TEntity parent = list.SingleOrDefault(c => c.Id == input.ParentId);
            //if (parent != null)
            //    list.Remove(parent);
            var list1 = new List<TDto>();
            foreach (var item in list)
            {
                //var entity = entities.Single(c => c.Id == item.Id);
                list1.Add(EntityToDto(item));
            }
            //  return dtos;


            // var list1 = GetAllEntityToDto(list);// ObjectMapper.Map<IList<TDto>>(list);//使用映射的好处是子类扩展多个属性时都可以使用映射，避免大量属性赋值的代码


            //这里应该加个开关，因为子类可能并不需要遍历
            //if (GetAllMap != null)
            //{
            //    foreach (var c in list1)
            //    {
            //        //c.Children = list1.Where(d => d.ParentId == c.Id).ToList();
            //        var entity = list.Single(d => d.Id == c.Id);

            //        //if (c.Children != null && c.Children.Count > 0)
            //        //    c.State = "closed";//默认值为 open

            //        //dto属性中处理了
            //        //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
            //        //    c.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

            //        GetAllMap(entity, c);
            //        GetAllEntityToDto(entity, c);
            //    }
            //}

            var parentDto = input.ParentId.HasValue ? list1.SingleOrDefault(c => c.Id == input.ParentId) : null;
            //得到顶级节点集合
            return list1.Where(c => c.ParentId == input.ParentId).ToList();

        }
        ///// <summary>
        ///// 获取列表时的include操作
        ///// </summary>
        ///// <param name="q"></param>
        ///// <returns></returns>
        //protected virtual IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> q)
        //{
        //    return q;//BuildQuery已经include父节点了
        //}
        /// <summary>
        /// 获取所有数据的查询
        /// </summary>
        /// <param name="q"></param>
        /// <param name="input">输入参数</param>
        /// <param name="parentCode">父节点code</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllFilter(IQueryable<TEntity> q, TGetAllInput input, string parentCode)
        {
            q = q.WhereIf(!input.IsOnlyLoadChild, c => c.Code.StartsWith(parentCode))
                 .WhereIf(input.IsOnlyLoadChild && parentCode.IsNotNullOrWhiteSpaceBXJG(), c => c.Parent.Code == parentCode || c.Code == parentCode)
                 .WhereIf(input.IsOnlyLoadChild && parentCode.IsNullOrWhiteSpaceBXJG(), c => !c.ParentId.HasValue);
            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;
        }
        /// <summary>
        /// 获取所有数据的排序
        /// </summary>
        /// <param name="query">查询</param>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllSort(IQueryable<TEntity> query, TGetAllInput input)
        {
            if (input is ISortedResultRequest a && a.Sorting.IsNotNullOrWhiteSpaceBXJG())
                return query.OrderBy(c => a.Sorting);
            return query.OrderBy(c => c.Code);
        }
        #endregion

        #region 辅助
        #region 权限判断
        /// <summary>
        /// 检查新增权限，若失败则抛出异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(CreatePermissionName);
        }
        /// <summary>
        /// 检查修改权限，若失败则抛出异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckUpdatePermissionAsync()
        {
            return CheckPermissionAsync(UpdatePermissionName);
        }
        /// <summary>
        /// 检查删除权限，若失败则抛出异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckDeletePermissionAsync()
        {
            return CheckPermissionAsync(DeletePermissionName);
        }
        /// <summary>
        /// 检查查询权限，若失败则抛出异常
        /// </summary>
        /// <returns></returns>
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(GetPermissionName);
        }
        /// <summary>
        /// 检查指定权限，若失败则抛出异常
        /// </summary>
        /// <param name="permissionName"></param>
        /// <returns></returns>
        protected virtual async Task CheckPermissionAsync(string permissionName)
        {
            //if (string.IsNullOrWhiteSpace(permissionName))
            //    return;

            //if (!await IsGrantedAsync(permissionName))
            //    throw new UserFriendlyException(L("UnAuthorized"));

            //使用父类的权限检查可以得到一个正常的未授权响应
            if (!string.IsNullOrEmpty(permissionName))
            {
                await PermissionChecker.AuthorizeAsync(permissionName);
            }
        }
        #endregion
        /// <summary>
        /// 新增和修改都会执行的逻辑，若需要传递额外参数，请使用当前Uow.Items
        /// 根据需要选择重写
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ValueTask MapToEntity(TEntity entity)
        {
            if (entity.ParentId == 0)
                entity.ParentId = null;
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 实体转换为dto，默认使用ObjectMapper映射
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual TDto EntityToDto(TEntity entity)
        {
            return ObjectMapper.Map<TDto>(entity);
        }
        /// <summary>
        /// 所有查询都会调用，可以重写以应用所有查询都需要的Include
        /// </summary>
        /// <param name="track">是否跟踪实体</param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<TEntity>> BuildQuery(bool track = true)
        {
            IQueryable<TEntity> q = (await Repository.GetAllAsync()).Include(c => c.Parent);
            if (!track)
                q = q.AsNoTrackingWithIdentityResolution();
            return q;
        }
        /// <summary>
        /// 批量处理
        /// </summary>
        /// <param name="ids">目标id集合</param>
        /// <param name="func">每个实体的具体操作</param>
        /// <param name="funcName">单个操作的名称</param>
        /// <returns></returns>
        protected virtual async Task<BatchOperationOutputLong> BatchHandleAsync(IEnumerable<long> ids, Func<TEntity, ValueTask> func, string funcName = "删除")
        {
            var r = new BatchOperationOutputLong();
            foreach (var id in ids)
            {
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var entity = await GetEntityByIdAsync(id);//这里每次查询性能更低，但是代码更清晰简洁
                    await func(entity);
                    await uow.CompleteAsync();
                    r.Ids.Add(id);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(id, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(id.Message500());
                    Logger.Warn($"部分{funcName}失败！{id}", ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 增、删、改、获取单个时都会调用，用来根据id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="track">是否跟踪实体</param>
        /// <returns></returns>
        protected virtual async Task<TEntity> GetEntityByIdAsync(long id, bool track = true)
        {
            return await AsyncQueryableExecuter.FirstOrDefaultAsync((await BuildQuery(track)).Where(c => c.Id == id));
        }
        protected virtual async Task<TDto> GetDtoById(TEntity entity, long? id = default)
        {

            var entity1 = await GetEntityByIdAsync(id ?? entity.Id, false);//.SingleAsync(c => c.Id.Equals(id));
            return EntityToDto(entity1);
        }
        #endregion
    }

    /// <summary>
    /// 通用的树形结构的数据的crud抽象服务（完整）
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TDeleteInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TGetInput"></typeparam>
    /// <typeparam name="TMoveInput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class GeneralTreeBaseAppService<TEntity,
                                           TDto,
                                           TCreateInput,
                                           TEditDto,
                                           TGetAllInput> : GeneralTreeBaseAppService<TEntity,
                                                                                   TDto,
                                                                                   TCreateInput,
                                                                                   TEditDto,
                                                                                   TGetAllInput,
                                                                                   GeneralTreeManager<TEntity>>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                            TCreateInput,
                                                                                                                                            TEditDto,
                                                                                                                                            TGetAllInput>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : GeneralTreeGetTreeInput
    {

    }


    /// <summary>
    /// 通用的树形结构的数据的crud抽象服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    public abstract class GeneralTreeBaseAppService<TEntity,
                                           TDto,
                                           TCreateInput,
                                           TEditDto> : GeneralTreeBaseAppService<TEntity,
                                                                                 TDto,
                                                                                 TCreateInput,
                                                                                 TEditDto,
                                                                                 GeneralTreeGetTreeInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                                      TCreateInput,
                                                                                                                                      TEditDto>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
    {

    }

    /// <summary>
    /// 通用的树形结构的数据的crud抽象服务（完整）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    public abstract class GeneralTreeBaseAppService<TEntity,
                                           TDto,
                                           TCreateInput> : GeneralTreeBaseAppService<TEntity,
                                                                                     TDto,
                                                                                     TCreateInput,
                                                                                     TCreateInput>, IGeneralTreeBaseAppService<TDto,
                                                                                                                               TCreateInput>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeNodeBaseDto<TDto>, new()
    {

    }


    ///// <summary>
    ///// 通用的树形结构的数据的crud抽象服务（常用）
    ///// </summary>
    ///// <typeparam name="TDto"></typeparam>
    ///// <typeparam name="TCreateInput"></typeparam>
    ///// <typeparam name="TEditDto"></typeparam>
    ///// <typeparam name="TGetAllInput"></typeparam>
    ///// <typeparam name="TEntity"></typeparam>
    //public class GeneralTreeAppServiceBase<TDto,
    //                                   TCreateInput,
    //                                   TEditDto,
    //                                   TGetAllInput,
    //                                   TEntity> : GeneralTreeBaseAppService<
    //                                                                       TEntity, TDto,
    //                                                                       TCreateInput,
    //                                                                       TEditDto,
    //                                                                       BatchOperationInputLong,
    //                                                                       TGetAllInput,
    //                                                                       EntityDto<long>,
    //                                                                       GeneralTreeNodeMoveInput,
    //                                                                       GeneralTreeManager<TEntity>>
    //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
    //where TEntity : GeneralTreeEntity<TEntity>
    //where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
    //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
    //where TGetAllInput : GeneralTreeGetTreeInput
    //{ }
}