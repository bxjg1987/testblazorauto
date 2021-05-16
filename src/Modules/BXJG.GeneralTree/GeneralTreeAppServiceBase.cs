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

namespace BXJG.GeneralTree
{
    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    /// <typeparam name="TGetNodesForSelectOutput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    public class UnAuthGeneralTreeAppServiceBase<TGetTreeForSelectInput,
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
        /* 
         * 数据显示地方有：管理页列表、作为一个搜索条件框、作为表单里一个下拉框
         * 顶级文本可能是 前端传过来的、上级节点文本、默认文本；除非根本不现实
         */

        protected string allTextForSearch, allTextForForm;//注意这里代表的是本地化文本的key
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }//属性注入
        protected readonly IRepository<TEntity, long> repository;

        public UnAuthGeneralTreeAppServiceBase(IRepository<TEntity, long> repository,
                                               string allTextForSearch = "不限",
                                               string allTextForForm = "请选择")//这里的字符串后期可以使用常量
        {
            //base.LocalizationSourceName = GeneralTreeConsts.LocalizationSourceName;
            this.repository = repository;
            this.AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
            //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
            this.allTextForSearch = allTextForSearch.UtilsL();
            this.allTextForForm = allTextForForm.UtilsL();
        }
        /// <summary>
        /// 获取树形的下拉框数据，不需要身份验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[UnitOfWork(false)]//接口已经加了
        public virtual async Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input)
        {
            //权限判断
            //await CheckGetPermissionAsync();


            //得到实体扁平集合
            string parentCode = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await repository.GetAsync(input.ParentId.Value);
                parentCode = top.Code;
            }
            var ctx = new Dictionary<string, object> { { "input", input } };
            var query = await this.ComboTreeFilterAsync(input, parentCode, ctx);
            query = await this.ComboTreeSortAsync(input, query, ctx);


            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = await EntityToTreeDtoAsync(list, ctx);// ObjectMapper.Map<List<TGetTreeForSelectOutput>>(list);

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
                dtoList.Insert(0, new TGetTreeForSelectOutput { Id = null, Text = L(input.ParentText) });
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
        //[UnitOfWork(false)]//接口已经加了
        public virtual async Task<IList<TGetNodesForSelectOutput>> GetNodesForSelectAsync(TGetNodesForSelectInput input)
        {
            //await CheckGetPermissionAsync();
            //得到实体扁平集合
            //string parentCode = "";
            //if (input.ParentId.HasValue && input.ParentId.Value > 0)
            //{
            //    var top = await ownRepository.GetAsync(input.ParentId.Value);
            //    parentCode = top.Code;
            //}
            var ctx = new Dictionary<string, object> { { "input", input } };
            var query = await ComboboxFilterAsync(input, input.ParentId, ctx);

            query = await ComboboxSortAsync(input, query, ctx);
            //GetNodesForSelectProjection允许子类直接投影，这种情况可能不太灵活，因为子类可能不方便做ef投影，所以将来可能考虑完全获取实体，在内存中来做这个转换

            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = await EntityToComboboDtoAsync(list, ctx);// ObjectMapper.Map<List<TGetNodesForSelectOutput>>(list);

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
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = L(input.ParentText) });
            else if ((input.ForType == 1 || input.ForType == 3) && input.ParentId.HasValue)
                dtoList.Insert(0, parentDto);
            else if (input.ForType == 1 || input.ForType == 2)
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForSearch });
            else if (input.ForType == 3 || input.ForType == 4)
                dtoList.Insert(0, new TGetNodesForSelectOutput { Value = null, DisplayText = allTextForForm });

            return dtoList;
        }

        #region 获取树形下拉框数据时子类可以重写的方法
        /// <summary>
        /// 获取树形数据的queryable，默认StartsWith parentCode
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parentCode"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> ComboTreeFilterAsync(TGetTreeForSelectInput input, string parentCode, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(repository.GetAll().Where(c => c.Code.StartsWith(parentCode)));
        }
        /// <summary>
        /// 获取树形数据的排序，默认按code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> ComboTreeSortAsync(TGetTreeForSelectInput input, IQueryable<TEntity> query, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(query.OrderBy(c => c.Code) as IQueryable<TEntity>);
        }
        /// <summary>
        /// 实体转换为dto时调用，默认使用automapper
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="context"><see cref="GetTreeForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<List<TGetTreeForSelectOutput>> EntityToTreeDtoAsync(IEnumerable<TEntity> entities, IDictionary<string, object> context = default)
        {
            var dtos = ObjectMapper.Map<List<TGetTreeForSelectOutput>>(entities);
            return ValueTask.FromResult(dtos);
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
        protected virtual ValueTask<IQueryable<TEntity>> ComboboxFilterAsync(TGetNodesForSelectInput input, long? parentId, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(repository.GetAll().Where(c => c.ParentId == parentId));
            //return ownRepository.GetAll().Where(c => c.ParentId == input.ParentId || c.Id == input.ParentId);
        }
        /// <summary>
        /// 获取扁平化下拉框数据的排序，默认code
        /// </summary>
        /// <param name="query"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="GetNodesForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> ComboboxSortAsync(TGetNodesForSelectInput input, IQueryable<TEntity> query, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(query.OrderBy(c => c.Code) as IQueryable<TEntity>);
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
        /// <param name="context"><see cref="GetNodesForSelectAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<List<TGetNodesForSelectOutput>> EntityToComboboDtoAsync(IEnumerable<TEntity> entities, IDictionary<string, object> context = default)
        {
            var dtos = ObjectMapper.Map<List<TGetNodesForSelectOutput>>(entities);
            return ValueTask.FromResult(dtos);
        }
        #endregion
    }

    /// <summary>
    /// 树形结构应用逻辑基类
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
    public class GeneralTreeAppServiceBase<TDto,
                                           TCreateInput,
                                           TEditDto,
                                           TDeleteInput,
                                           TGetAllInput,
                                           TGetInput,
                                           TMoveInput,
                                           TEntity,
                                           TManager> : ApplicationService, IGeneralTreeAppServiceBase<TDto,
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
        protected readonly IRepository<TEntity, long> repository;
        protected readonly TManager generalTreeManager;
        protected string allTextForManager;//注意这里代表的是本地化文本的key

        protected string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName;

        public GeneralTreeAppServiceBase(IRepository<TEntity, long> ownRepository,
                                         TManager manager,
                                         string createPermissionName = null,
                                         string updatePermissionName = null,
                                         string deletePermissionName = null,
                                         string getPermissionName = null,
                                         string allTextForManager = "全部")
        {
            //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
            this.allTextForManager = allTextForManager.UtilsL();
            this.repository = ownRepository;

            this.generalTreeManager = manager;

            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.getPermissionName = getPermissionName;
        }
        #region create
        /// <summary>
        /// 创建树形结构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> CreateAsync(TCreateInput input)
        {
            await CheckCreatePermissionAsync();

            if (input.ParentId <= 0)
                input.ParentId = null;

            var ctx = new Dictionary<string, object> { { "input", input } };
            var m = await CreateMapAsync(input, ctx);// ObjectMapper.Map<TEntity>(input);

            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }

            await BeforeCreateAsync(input, m, ctx);
            await generalTreeManager.CreateAsync(m);
            return await GetEntityToDtoAsync(m);
        }
        /// <summary>
        /// 新增时的映射，默认使用automapper映射
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"><see cref="GetAllAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<TEntity> CreateMapAsync(TCreateInput input, IDictionary<string, object> context = default)
        {
            var entity = ObjectMapper.Map<TEntity>(input);
            return ValueTask.FromResult(entity);
        }
        /// <summary>
        /// 新增前回调，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="CreateAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask BeforeCreateAsync(TCreateInput input, TEntity entity, IDictionary<string, object> context = default)
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

            var ctx = new Dictionary<string, object> { { "input", input } };
            var m = await AsyncQueryableExecuter.FirstOrDefaultAsync(await UpdateGetAsync(input, ctx));

            await UpdateMapAsync(input, m, ctx);
            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }
            await BeforeUpdateAsync(input, m, ctx);
            await generalTreeManager.UpdateAsync(m);
            return await GetEntityToDtoAsync(m);
        }
        /// <summary>
        /// 修改时的查询，默认根据id查询
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"><see cref="UpdateAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> UpdateGetAsync(TEditDto input, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(repository.GetAll().Where(c => c.Id == input.Id));
        }
        /// <summary>
        /// 修改时的映射，默认使用automapper
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="UpdateAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask UpdateMapAsync(TEditDto input, TEntity entity, IDictionary<string, object> context = default)
        {
            ObjectMapper.Map(input, entity);
            return ValueTask.CompletedTask;
        }
        /// <summary>
        /// 修改前回调，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="input"></param>
        /// <param name="context"><see cref="UpdateAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask BeforeUpdateAsync(TEditDto input, TEntity entity, IDictionary<string, object> context = default)
        {
            return ValueTask.CompletedTask;
        }
        #endregion

        #region delete
        /// <summary>
        /// 删除树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TDeleteInput input)
        {
            await CheckDeletePermissionAsync();
            await this.generalTreeManager.DeleteAsync(BeforeDeleteAsync, input.Ids);
        }
        /// <summary>
        /// 删除前回调，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected virtual ValueTask BeforeDeleteAsync(TEntity entity)
        {
            return ValueTask.CompletedTask;
        }
        #endregion

        /// <summary>
        /// 获取列表和获取指定id的信息都将回调此方法，默认啥也没干
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dto"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual ValueTask EntityToDtoAsync(TEntity entity, TDto dto, IDictionary<string, object> context = default)
        {
            return ValueTask.CompletedTask;
        }

        #region get
        /// <summary>
        /// 获取指定节点的树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetAsync(TGetInput input)
        {
            await CheckGetPermissionAsync();
            var ctx = new Dictionary<string, object> { { "input", input } };
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(await GetQueryAsync(input, ctx));

            var n = await GetEntityToDtoAsync(entity, ctx);
            //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
            //    n.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            return n;
        }
        /// <summary>
        /// 根据id获取单个实体时将调用此方法获取IQueryable，默认已加入id比对条件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"><see cref="GetAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> GetQueryAsync(TGetInput input, IDictionary<string, object> context = default)
        {
            var query = repository.GetAll().Where(c => c.Id == input.Id);
            return ValueTask.FromResult(query);
        }
        /// <summary>
        /// 根据id获取单个实体时的映射，默认使用automapper
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="context"><see cref="GetAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual async ValueTask<TDto> GetEntityToDtoAsync(TEntity entity, IDictionary<string, object> context = default)
        {
            var dto = ObjectMapper.Map<TDto>(entity);
            await EntityToDtoAsync(entity, dto, context);
            return dto;
        }
        #endregion

        #region getall
        /// <summary>
        /// 获取所有树形结构的数据，以树形层次结构返回
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<IList<TDto>> GetAllAsync(TGetAllInput input)
        {
            //权限判断
            await CheckGetPermissionAsync();

            //获取父节点的code 方便后续查询所有子集
            string parentCode = "";

            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await repository.SingleAsync(c => c.Id == input.ParentId.Value);
                parentCode = top.Code;
            }

            var ctx = new Dictionary<string, object> { { "input", input } };
            //查询
            var query = await GetAllFilteredAsync(input, parentCode, ctx);//.Where(c => c.Code.StartsWith(parentCode));
            query = await GetAllSortingAsync(input, query, ctx); //方便子类排序
            var list = await AsyncQueryableExecuter.ToListAsync(query);//.ToListAsync();
            //建立dto以及处理父子关系
            //TEntity parent = list.SingleOrDefault(c => c.Id == input.ParentId);
            //if (parent != null)
            //    list.Remove(parent);

            var list1 = await GetAllEntityToDtoAsync(list, ctx);// ObjectMapper.Map<IList<TDto>>(list);//使用映射的好处是子类扩展多个属性时都可以使用映射，避免大量属性赋值的代码


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
                    return new List<TDto> { new TDto { DisplayName = L(input.ParentText), Children = list1 } };

                if (input.ParentId.HasValue)
                    return new List<TDto> { parentDto };

                return new List<TDto> { new TDto { DisplayName = allTextForManager, Children = list1 } };
            }
            return list1;
        }
        /// <summary>
        /// 获取所有数据的查询
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="parentCode">父节点code</param>
        /// <param name="context"><see cref="GetAllAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> GetAllFilteredAsync(TGetAllInput input, string parentCode, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(repository.GetAll().Where(c => c.Code.StartsWith(parentCode)));
        }
        /// <summary>
        /// 获取所有数据的排序
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <param name="query">查询</param>
        /// <param name="context"><see cref="GetAllAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual ValueTask<IQueryable<TEntity>> GetAllSortingAsync(TGetAllInput input, IQueryable<TEntity> query, IDictionary<string, object> context = default)
        {
            return ValueTask.FromResult(query.OrderBy(c => c.Code) as IQueryable<TEntity>);
        }
        /// <summary>
        /// 获取所有数据的实体到dto的映射
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="context"><see cref="GetAllAsync"/>的多个步骤间共享数据，默认存在input的key</param>
        /// <returns></returns>
        protected virtual async ValueTask<List<TDto>> GetAllEntityToDtoAsync(IEnumerable<TEntity> entities, IDictionary<string, object> context = default)
        {
            var dtos = ObjectMapper.Map<List<TDto>>(entities);
            foreach (var item in dtos)
            {
                var entity = entities.Single(c => c.Id == item.Id);
                await EntityToDtoAsync(entity, item, context);
            }
            return dtos;
        }
        #endregion

        #region 权限判断
        protected virtual Task CheckCreatePermissionAsync()
        {
            return CheckPermissionAsync(deletePermissionName);
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
}
