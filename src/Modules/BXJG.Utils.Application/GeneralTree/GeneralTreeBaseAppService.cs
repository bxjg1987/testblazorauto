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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Domain.Uow;
using BXJG.Common.Dto;
using Abp.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Components.Forms;

namespace BXJG.Utils.GeneralTree
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
    public class GeneralTreeProviderBaseAppService<TGetTreeForSelectInput,
                                                   TGetTreeForSelectOutput,
                                                   TGetNodesForSelectInput,
                                                   TGetNodesForSelectOutput,
                                                   TEntity> : ApplicationService, IUnAuthGeneralTreeAppServiceBase<TGetTreeForSelectInput,
                                                                                                                   TGetTreeForSelectOutput,
                                                                                                                   TGetNodesForSelectInput,
                                                                                                                   TGetNodesForSelectOutput>
    where TEntity : GeneralTreeEntity<TEntity>
    where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
    where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new()
    where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
    where TGetNodesForSelectOutput : GeneralTreeComboboxDto, new()
    {
        // protected string allTextForSearch, allTextForForm;//注意这里代表的是本地化文本的key

        public virtual string allTextForSearch { get; set; } = "不限";
        public virtual string allTextForForm { get; set; } = "请选择";

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        public IRepository<TEntity, long> repository { get; set; }
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        [Obsolete("子类继承时应使用无参的构造函数，当前构造函数是未简化子类实现以前的代码，已过时。")]
        public GeneralTreeProviderBaseAppService(IRepository<TEntity, long> repository,
                                                 string allTextForSearch = "不限",
                                                 string allTextForForm = "请选择")//这里的字符串后期可以使用常量
        {
            //base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
            this.repository = repository;
            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
            this.allTextForSearch = allTextForSearch;
            this.allTextForForm = allTextForForm;
        }

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
            if (input.Code.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await repository.GetAsync(input.ParentId.Value);
                parentCode = top.Code;
            }
            else
                parentCode = input.Code ?? "";

            var query = this.ComboTreeFilter(input, parentCode);
            query = this.ComboTreeSort(query, input);


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
            TGetTreeForSelectOutput parentDto;
            if (input.ParentId.HasValue)
            {
                parentDto = dtoList.SingleOrDefault(c => c.Id == input.ParentId.ToString());
                dtoList = dtoList.Where(c => c.ParentId == input.ParentId.ToString()).ToList();
            }
            else
            {
                parentDto = null;
                dtoList = dtoList.Where(c => string.IsNullOrWhiteSpace(c.ParentId)).ToList();
            }


            //通用树是通过继承来实现扩展的，所以这里L引用的本地化源可能被子类重写，因此这里用L是可以的
            if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
            {
                dtoList.Insert(0, new TGetTreeForSelectOutput { Id = null, Text = input.ParentText });
                return dtoList;
            }
            //return new List<TGetTreeForSelectOutput> { new TGetTreeForSelectOutput { Id = null, Text = L(input.ParentText), Children = dtoList } };

            if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
            {
                parentDto.Text = "==" + parentDto.Text + "==";
                parentDto.Id = null;
                return new List<TGetTreeForSelectOutput> { parentDto };
            }


            if (input.ForType == 1 || input.ForType == 2)
            {
                dtoList.Insert(0, new TGetTreeForSelectOutput { Id = null, Text = allTextForSearch });
                return dtoList;
            }
            //return new List<TGetTreeForSelectOutput> { new TGetTreeForSelectOutput { Id = null, Text = this.allTextForSearch, Children = dtoList } };

            if (input.ForType == 3 || input.ForType == 4)
            {
                dtoList.Insert(0, new TGetTreeForSelectOutput { Id = null, Text = allTextForForm });
                return dtoList;
            }
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
            var query = ComboboxFilter(input, input.ParentId);

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
                parentDto.Value = null;
                parentDto.DisplayText = "==" + parentDto.DisplayText + "==";
            }
            //dtoList = dtoList.Where(c => c.Value != input.Id).ToList();

            if (input.ForType > 0 && input.ForType < 5 && !string.IsNullOrWhiteSpace(input.ParentText))
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = input.ParentText });
            else if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
                dtoList.Insert(0, parentDto);
            else if (input.ForType == 1 || input.ForType == 2)
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForSearch });
            else if (input.ForType == 3 || input.ForType == 4)
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForForm });

            return dtoList;
        }

        /// <summary>
        /// 获取单个 扁平和树形列表都会调用，以获取查询对象
        /// 可以重写以应用所有查询都需要的Include
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> BuildQuery()
        {
            return repository.GetAll().Include(c => c.Parent).AsNoTrackingWithIdentityResolution();
        }

        #region 获取树形下拉框数据时子类可以重写的方法
        /// <summary>
        /// 获取树形数据的queryable，默认StartsWith parentCode
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parentCode"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ComboTreeFilter(TGetTreeForSelectInput input, string parentCode)
        {
            var q = BuildQuery().Where(c => c.Code.StartsWith(parentCode));
            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;
        }
        /// <summary>
        /// 获取树形数据的排序，默认按code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
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
        /// <param name="context"><see cref="GetNodesForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ComboboxFilter(TGetNodesForSelectInput input, long? parentId)
        {
            var q = BuildQuery().Where(c => c.ParentId == parentId);
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
    /// <typeparam name="TManager"></typeparam>
    public class GeneralTreeBaseAppService<TDto,
                                           TCreateInput,
                                           TEditDto,
                                           TDeleteInput,
                                           TGetAllInput,
                                           TGetInput,
                                           TMoveInput,
                                           TEntity,
                                           TManager> : ApplicationService, IGeneralTreeBaseAppService<TDto,
                                                                                                      TCreateInput,
                                                                                                      TEditDto,
                                                                                                      TDeleteInput,
                                                                                                      TGetAllInput,
                                                                                                      TGetInput,
                                                                                                      TMoveInput>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TDeleteInput : BatchOperationInputLong
        where TGetInput : EntityDto<long>
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TManager : GeneralTreeManager<TEntity>
        where TGetAllInput : GeneralTreeGetTreeInput
        where TMoveInput : GeneralTreeNodeMoveInput
    {
        /* 
         * 数据显示地方有：管理页列表、作为一个搜索条件框、作为表单里一个下拉框
         * 顶级文本可能是 前端传过来的、上级节点文本、默认文本；除非根本不现实
         */
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public IRepository<TEntity, long> repository { get; set; }
        public TManager generalTreeManager { get; set; }
        protected virtual string allTextForManager { get; set; } = "全部";//注意这里代表的是本地化文本的key
        /// <summary>
        /// 与当前请求关联的服务容器
        /// 通常你可以使用构造函数或属性注入，框架级别或特殊情况可以使用此对象。
        /// 注：IocManager是全局单例，解析实现IDisposeable的服务时比较危险，此时应使用ServiceProvider
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }
        //protected string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName;
        public virtual string createPermissionName { get; set; }
        public virtual string updatePermissionName { get; set; }
        public virtual string deletePermissionName { get; set; }
        public virtual string getPermissionName { get; set; }

        [Obsolete("子类继承时应使用无参的构造函数，当前构造函数是未简化子类实现以前的代码，已过时。")]
        public GeneralTreeBaseAppService(IRepository<TEntity, long> ownRepository,
                                         TManager manager,
                                         string createPermissionName = null,
                                         string updatePermissionName = null,
                                         string deletePermissionName = null,
                                         string getPermissionName = null,
                                         string allTextForManager = "全部")
        {
            //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
            this.allTextForManager = allTextForManager;
            this.repository = ownRepository;

            this.generalTreeManager = manager;

            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.getPermissionName = getPermissionName;
        }

        public GeneralTreeBaseAppService()
        {
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        /// <summary>
        /// 新增和修改都会执行的逻辑，若需要传递额外参数，请使用当前Uow.Items
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ValueTask MapToEntityAsync(TEntity entity) => ValueTask.CompletedTask;

        #region create
        /// <summary>
        /// 创建树形结构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();

            if (input.ParentId.HasValue && input.ParentId <= 0)
                input.ParentId = null;

            // var ctx = new Dictionary<string, object> { { "input", input } };
            var m = CreateMap(input);// ObjectMapper.Map<TEntity>(input);

            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }

            await BeforeCreateAsync(input, m);
            await MapToEntityAsync(m);
            await generalTreeManager.CreateAsync(m);
            return GetEntityToDto(m);
        }
        /// <summary>
        /// 新增时的映射，默认使用automapper映射
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual TEntity CreateMap(TCreateInput input)
        {
            return ObjectMapper.Map<TEntity>(input);
        }
        /// <summary>
        /// 新增前回调，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual ValueTask BeforeCreateAsync(TCreateInput input, TEntity entity)
        {
            return ValueTask.CompletedTask;
        }
        #endregion

        #region move
        /// <summary>
        /// 移动树形结构数据的节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> MoveAsync(TMoveInput input)
        {
            //移动关于追加 之后 之前 的处理逻辑本应该定义在领域服务中
            await CheckUpdatePermissionAsync();
            await BeforeMoveAsync(input);
            var m = await generalTreeManager.MoveAsync(input.Id, input.TargetId, input.MoveType);
            return ObjectMapper.Map<TDto>(m);// m.MapTo<TDto>();
        }
        /// <summary>
        /// 移动节点前回调，默认啥也没干
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask BeforeMoveAsync(TMoveInput input)
        {
            return ValueTask.CompletedTask;
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

            if (input.ParentId == 0)
                input.ParentId = null;

            //  var ctx = new Dictionary<string, object> { { "input", input } };
            var m = await GetEntityByIdAsync(input.Id);

            //await UpdateMapAsync(input, m, ctx);
            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }
            await BeforeUpdateAsync(input, m);
            await MapToEntityAsync(m);
            await generalTreeManager.UpdateAsync(m);
            return GetEntityToDto(m);
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
        protected virtual void UpdateMapAsync(TEditDto input, TEntity entity)
        {
            ObjectMapper.Map(input, entity);
        }
        /// <summary>
        /// 修改前回调，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="UpdateAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask BeforeUpdateAsync(TEditDto input, TEntity entity)
        {
            return ValueTask.CompletedTask;
        }
        #endregion


        /// <summary>
        /// 批量处理
        /// </summary>
        /// <param name="input"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        protected virtual async Task<BatchOperationOutputLong> BatchHandleAsync(BatchOperationInputLong input, Func<TEntity, ValueTask> func)
        {
            var r = new BatchOperationOutputLong();
            foreach (var id in input.Ids)
            {
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    var entity = await GetEntityByIdAsync(id);
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
                    Logger.Warn($"部分操作失败！{id}", ex);
                }
            }
            return r;
        }


        #region delete

        /// <summary>
        /// 删除树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutputLong> DeleteAsync(TDeleteInput input)
        {
            await CheckDeletePermissionAsync();
            ///  await this.generalTreeManager.DeleteAsync(BeforeDeleteAsync, input.Ids);
            return await BatchHandleAsync(input, BeforeDeleteAsync);
        }
        /// <summary>
        /// 删除前回调，默认当前及其后代节点都删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual async ValueTask BeforeDeleteAsync(TEntity entity)
        {
            await repository.DeleteAsync(c => c.Code.StartsWith(entity.Code));
        }
        #endregion

        /// <summary>
        /// 获取列表和获取指定id的信息都将回调此方法，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual void EntityToDto(TEntity entity, TDto dto)
        { }

        /// <summary>
        /// 所有查询都会调用，以获取查询对象。
        /// 可以重写以应用所有查询都需要的Include
        /// </summary>
        /// <returns></returns>
        protected IQueryable<TEntity> BuildQuery() => repository.GetAll().Include(c => c.Parent);

        #region get
        /// <summary>
        /// 获取指定节点的树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<TDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            // var ctx = new Dictionary<string, object> { { "input", input } };
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(GetEntityByIdInclude(input.Id).AsNoTrackingWithIdentityResolution());//.SingleAsync();

            var n = GetEntityToDto(entity);
            //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
            //    n.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            return n;
        }
        /// <summary>
        /// 增、删、改、获取单个时都会调用，用来根据id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual Task<TEntity> GetEntityByIdAsync(long id)
        {
            return GetEntityByIdInclude(id).SingleAsync();
        }
        /// <summary>
        /// 根据id获取实体时回调，你可以重写使用Include包含导航属性，默认不处理
        /// </summary>
        /// <param name="q"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetEntityByIdInclude(long id) => BuildQuery().Where(c => c.Id == id);

        ///// <summary>
        ///// 根据id获取单个实体时将调用此方法获取IQueryable，默认已加入id比对条件
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="context"><see cref="GetAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        ///// <returns></returns>
        //protected virtual ValueTask<IQueryable<TEntity>> GetQueryAsync(TGetInput input, IDictionary<string, object> context = default)
        //{
        //    var query = repository.GetAll().Where(c => c.Id == input.Id);
        //    return ValueTask.FromResult(query);
        //}
        /// <summary>
        /// 根据id获取单个实体时的映射，默认使用automapper
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual TDto GetEntityToDto(TEntity entity)
        {
            var dto = ObjectMapper.Map<TDto>(entity);
            EntityToDto(entity, dto);
            return dto;
        }
        #endregion

        #region getall
        /// <summary>
        /// 获取所有树形结构的数据，以树形层次结构返回
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<IList<TDto>> GetAllAsync(TGetAllInput input)
        {
            //权限判断
            await CheckGetPermissionAsync();

            //获取父节点的code 方便后续查询所有子集
            string parentCode = "";

            if (input.ParentCode.IsNullOrWhiteSpace() && input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await repository.SingleAsync(c => c.Id == input.ParentId.Value);
                parentCode = top.Code;
            }
            else
                parentCode = input.ParentCode ?? "";

            // var ctx = new Dictionary<string, object> { { "input", input } };
            //查询
            var query = GetAllFiltered(input, parentCode);//.Where(c => c.Code.StartsWith(parentCode));
            query = GetAllSorting(query, input); //方便子类排序
            var list = await AsyncQueryableExecuter.ToListAsync(query);//.ToListAsync();
            //建立dto以及处理父子关系
            //TEntity parent = list.SingleOrDefault(c => c.Id == input.ParentId);
            //if (parent != null)
            //    list.Remove(parent);

            var list1 = GetAllEntityToDto(list);// ObjectMapper.Map<IList<TDto>>(list);//使用映射的好处是子类扩展多个属性时都可以使用映射，避免大量属性赋值的代码


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
            list1 = list1.Where(c => c.ParentId == input.ParentId).ToList();

            //处理顶级文本
            if (input.LoadParent)
            {
                if (!string.IsNullOrWhiteSpace(input.ParentText))
                    return new List<TDto> { new TDto { DisplayName = input.ParentText, Children = list1 } };

                if (input.ParentId.HasValue)
                    return new List<TDto> { parentDto };

                return new List<TDto> { new TDto { DisplayName = allTextForManager, Children = list1 } };
            }
            return list1;
        }
        ///// <summary>
        ///// 获取列表时回调，你可以重写以Include更多导航属性
        ///// </summary>
        ///// <param name="q"></param>
        ///// <returns></returns>
        //protected virtual IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> q) => q;

        /// <summary>
        /// 获取所有数据的查询，默认已加入parentCode条件
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="parentCode">父节点code</param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllFiltered(TGetAllInput input, string parentCode)
        {
            var q = BuildQuery().AsNoTrackingWithIdentityResolution().Where(c => c.Code.StartsWith(parentCode));//.ApplyDynamicCondtion(input);
            if (input is IHaveFilter p)
                q = q.ApplyDynamicCondtion(p.Filter);
            return q;
        }
        /// <summary>
        /// 获取所有数据的排序
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="query">查询</param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> GetAllSorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.OrderBy(c => c.Code);
        }
        /// <summary>
        /// 获取所有数据的实体到dto的映射
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="context">不要再使用此参数，请直接使用uow.Items</param>
        /// <returns></returns>
        protected virtual List<TDto> GetAllEntityToDto(IEnumerable<TEntity> entities)
        {
            var dtos = ObjectMapper.Map<List<TDto>>(entities);
            foreach (var item in dtos)
            {
                var entity = entities.Single(c => c.Id == item.Id);
                EntityToDto(entity, item);
            }
            return dtos;
        }
        #endregion

        #region 权限判断
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(createPermissionName);
        }
        protected virtual Task CheckUpdatePermissionAsync()
        {
            return CheckPermissionAsync(updatePermissionName);
        }
        protected virtual Task CheckDeletePermissionAsync()
        {
            return CheckPermissionAsync(deletePermissionName);
        }
        protected virtual Task CheckGetPermissionAsync()
        {
            return CheckPermissionAsync(getPermissionName);
        }
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
    }
    /// <summary>
    /// 通用的树形结构的数据的crud抽象服务（常用）
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TCreateInput"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class GeneralTreeAppServiceBase<TDto,
                                           TCreateInput,
                                           TEditDto,
                                           TGetAllInput,
                                           TEntity> : GeneralTreeBaseAppService<TDto,
                                                                               TCreateInput,
                                                                               TEditDto,
                                                                               BatchOperationInputLong,
                                                                               TGetAllInput,
                                                                               EntityDto<long>,
                                                                               GeneralTreeNodeMoveInput,
                                                                               TEntity,
                                                                               GeneralTreeManager<TEntity>>
        where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : GeneralTreeGetTreeInput
    { }
}