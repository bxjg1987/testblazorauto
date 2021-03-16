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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Linq.Extensions;
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
        protected readonly IRepository<TEntity, long> ownRepository;

        public UnAuthGeneralTreeAppServiceBase(IRepository<TEntity, long> ownRepository,
                                               string allTextForSearch = "不限",
                                               string allTextForForm = "请选择")//这里的字符串后期可以使用常量
        {
            base.LocalizationSourceName = GeneralTreeConsts.LocalizationSourceName;
            this.ownRepository = ownRepository;
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
        public virtual async Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input)
        {
            //权限判断
            //await CheckGetPermissionAsync();


            //得到实体扁平集合
            string parentCode = "";
            if (input.ParentId.HasValue && input.ParentId.Value > 0)
            {
                var top = await ownRepository.GetAsync(input.ParentId.Value);
                parentCode = top.Code;
            }
            var query = this.ComboTreeFilter(input, parentCode);
            query = this.ComboTreeSort(query, input);


            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = ObjectMapper.Map<List<TGetTreeForSelectOutput>>(list);

            if (ComboTreeMap != null)
            {
                dtoList.ForEach(c =>
                {
                    var entity = list.Single(d => d.Id.ToString() == c.Id);

                    //这俩属性通过属性的方式处理了
                    //c.state = "closed";//默认值为 open
                    //c.attributes.extData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

                    ComboTreeMap(entity, c);
                });

            }
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

            var query = ComboboxFilter(input, input.ParentId);

            query = ComboboxSort(query, input);
            //GetNodesForSelectProjection允许子类直接投影，这种情况可能不太灵活，因为子类可能不方便做ef投影，所以将来可能考虑完全获取实体，在内存中来做这个转换

            var list = await AsyncQueryableExecuter.ToListAsync(query);

            var dtoList = ObjectMapper.Map<List<TGetNodesForSelectOutput>>(list);

            if (ComboboxMap != null)
            {
                dtoList.ForEach(c =>
                {
                    var entity = list.Single(d => d.Id.ToString() == c.Value);

                    //这俩属性通过属性的方式处理了
                    //c.state = "closed";//默认值为 open
                    //c.attributes.extData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

                    ComboboxMap(entity, c);
                });

            }

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
        protected virtual IQueryable<TEntity> ComboTreeFilter(TGetTreeForSelectInput input, string parentCode)
        {
            return ownRepository.GetAll().Where(c => c.Code.StartsWith(parentCode));
        }
        protected virtual IQueryable<TEntity> ComboTreeSort(IQueryable<TEntity> query, TGetTreeForSelectInput input)
        {
            return query.OrderBy(c => c.Code);
        }

        protected Action<TEntity, TGetTreeForSelectOutput> ComboTreeMap = null;
        //protected virtual void ComboTreeMap(TEntity entity, TGetTreeForSelectOutput node)
        //{
        //    //base.ObjectMapper.Map<TGetTreeForSelectOutput>(entity);
        //}
        #endregion
        #region 获取扁平化下拉框数据时子类可重写的方法
        protected virtual IQueryable<TEntity> ComboboxFilter(TGetNodesForSelectInput input, long? parentId)
        {
            return ownRepository.GetAll().Where(c => c.ParentId == parentId);
            //return ownRepository.GetAll().Where(c => c.ParentId == input.ParentId || c.Id == input.ParentId);
        }
        protected virtual IQueryable<TEntity> ComboboxSort(IQueryable<TEntity> query, TGetNodesForSelectInput input)
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
        protected Action<TEntity, TGetNodesForSelectOutput> ComboboxMap = null;
        #endregion
    }

    /// <summary>
    /// 树形结构应用逻辑基类
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TGetAllInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectInput"></typeparam>
    /// <typeparam name="TGetTreeForSelectOutput"></typeparam>
    /// <typeparam name="TGetNodesForSelectInput"></typeparam>
    /// <typeparam name="TGetNodesForSelectOutput"></typeparam>
    /// <typeparam name="TMoveInput"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TManager"></typeparam>
    public class GeneralTreeAppServiceBase<TDto,
                                           TEditDto,
                                           TGetAllInput,
                                           TGetTreeForSelectInput,
                                           TGetTreeForSelectOutput,
                                           TGetNodesForSelectInput,
                                           TGetNodesForSelectOutput,
                                           TMoveInput,
                                           TEntity,
                                           TManager> : UnAuthGeneralTreeAppServiceBase<TGetTreeForSelectInput,
                                                                                       TGetTreeForSelectOutput,
                                                                                       TGetNodesForSelectInput,
                                                                                       TGetNodesForSelectOutput,
                                                                                       TEntity>, IGeneralTreeAppServiceBase<TDto,
                                                                                                                            TEditDto,
                                                                                                                            TGetAllInput,
                                                                                                                            TGetTreeForSelectInput,
                                                                                                                            TGetTreeForSelectOutput,
                                                                                                                            TGetNodesForSelectInput,
                                                                                                                            TGetNodesForSelectOutput,
                                                                                                                            TMoveInput>
        where TEntity : GeneralTreeEntity<TEntity>
        where TDto : GeneralTreeGetTreeNodeBaseDto<TDto>, new()
        where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TManager : GeneralTreeManager<TEntity>
        where TGetAllInput : GeneralTreeGetTreeInput
        where TGetTreeForSelectInput : GeneralTreeGetForSelectInput
        where TGetTreeForSelectOutput : GeneralTreeNodeDto<TGetTreeForSelectOutput>, new()
        where TGetNodesForSelectInput : GeneralTreeGetForSelectInput
        where TGetNodesForSelectOutput : GeneralTreeComboboxDto, new()
        where TMoveInput : GeneralTreeNodeMoveInput
    {
        /* 
         * 数据显示地方有：管理页列表、作为一个搜索条件框、作为表单里一个下拉框
         * 顶级文本可能是 前端传过来的、上级节点文本、默认文本；除非根本不现实
         */

        protected readonly TManager generalTreeManager;
        protected string allTextForManager;//注意这里代表的是本地化文本的key

        protected string createPermissionName, updatePermissionName, deletePermissionName, getPermissionName;

        public GeneralTreeAppServiceBase(IRepository<TEntity, long> ownRepository,
                                         TManager organizationUnitManager,
                                         string createPermissionName = null,
                                         string updatePermissionName = null,
                                         string deletePermissionName = null,
                                         string getPermissionName = null,
                                         string allTextForManager = "全部",
                                         string allTextForSearch = "不限",
                                         string allTextForForm = "请选择") : base(ownRepository,
                                                                                  allTextForSearch,
                                                                                  allTextForForm)
        {
            //L内部调用的LocationSource是使用的属性注入，所以在构造函数中无法使用L()  此规则.net framework版本是这个规则，.net core版本未测试过
            this.allTextForManager = allTextForManager.UtilsL();

            this.generalTreeManager = organizationUnitManager;

            this.createPermissionName = createPermissionName;
            this.updatePermissionName = updatePermissionName;
            this.deletePermissionName = deletePermissionName;
            this.getPermissionName = getPermissionName;
        }
        /// <summary>
        /// 创建树形结构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> CreateAsync(TEditDto input)
        {
            await CheckCreatePermissionAsync();

            if (input.ParentId <= 0)
                input.ParentId = null;

            var m = ObjectMapper.Map<TEntity>(input);

            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }

            //await BeforeCreate(m);
            await generalTreeManager.CreateAsync(m);
            return ObjectMapper.Map<TDto>(m);
        }
        /// <summary>
        /// 移动树形结构数据的节点
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> MoveAsync(TMoveInput input)
        {
            //移动关于追加 之后 之前 的处理逻辑本应该定义在领域服务中
            await CheckUpdatePermissionAsync();

            var m = await generalTreeManager.MoveAsync(input.Id, input.TargetId, input.MoveType);
            return ObjectMapper.Map<TDto>(m);// m.MapTo<TDto>();
        }
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

            var m = await ownRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, m);
            //扩展属性的处理后期放到Manager中去处理
            if (input.ExtData != null)
            {
                foreach (var item in input.ExtData)
                {
                    m.RemoveData(item.Key);
                    m.SetData(item.Key, item.Value);
                }
            }
            await generalTreeManager.UpdateAsync(m);
            return ObjectMapper.Map<TDto>(m);
        }
        /// <summary>
        /// 删除树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(EntityDto<long> input)
        {
            await CheckDeletePermissionAsync();
            await this.generalTreeManager.DeleteAsync(input.Id);
        }
        /// <summary>
        /// 获取指定节点的树形结构的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<TDto> GetAsync(EntityDto<long> input)
        {
            await CheckGetPermissionAsync();
            var entity = await ownRepository.GetAsync(input.Id);

            var n = ObjectMapper.Map<TDto>(entity);
            //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
            //    n.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            return n;
        }

        //如果没有上级节点包装 列表默认被选择了 点击新增时 上级几点无法定位到null，因此需要做个包装
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
                var top = await ownRepository.SingleAsync(c => c.Id == input.ParentId.Value);
                parentCode = top.Code;
            }

            //查询
            var query = GetAllFiltered(input, parentCode);//.Where(c => c.Code.StartsWith(parentCode));
            query = GetAllSorting(query, input); //方便子类排序
            //var list = await AsyncQueryableExecuter.ToListAsync(query);//.ToListAsync();
            var list = await query.ToListAsync();
            //建立dto以及处理父子关系
            //TEntity parent = list.SingleOrDefault(c => c.Id == input.ParentId);
            //if (parent != null)
            //    list.Remove(parent);

            var list1 = ObjectMapper.Map<IList<TDto>>(list);//使用映射的好处是子类扩展多个属性时都可以使用映射，避免大量属性赋值的代码


            //这里应该加个开关，因为子类可能并不需要遍历
            if (GetAllMap != null)
            {
                foreach (var c in list1)
                {
                    //c.Children = list1.Where(d => d.ParentId == c.Id).ToList();
                    var entity = list.Single(d => d.Id == c.Id);

                    //if (c.Children != null && c.Children.Count > 0)
                    //    c.State = "closed";//默认值为 open

                    //dto属性中处理了
                    //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
                    //    c.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);

                    GetAllMap(entity, c);
                }
            }

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
        /// 获取树形下拉框数据，需要身份验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override Task<IList<TGetTreeForSelectOutput>> GetTreeForSelectAsync(TGetTreeForSelectInput input)
        {
            //权限判断
            //await CheckGetPermissionAsync();
            //不用[AbpAuthorize]，因为不确定子类是否可以覆盖
            if (!base.AbpSession.UserId.HasValue)
                throw new AbpAuthorizationException();

            return base.GetTreeForSelectAsync(input);
        }
        /// <summary>
        /// 获取扁平化下拉框数据，需要身份验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override Task<IList<TGetNodesForSelectOutput>> GetNodesForSelectAsync(TGetNodesForSelectInput input)
        {
            //await CheckGetPermissionAsync();
            //不用[AbpAuthorize]，因为不确定子类是否可以覆盖
            if (!base.AbpSession.UserId.HasValue)
                throw new AbpAuthorizationException();
            return base.GetNodesForSelectAsync(input);
        }

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

        #region 后台管理获取列表时可重写的方法
        protected virtual IQueryable<TEntity> GetAllFiltered(TGetAllInput q, string parentCode)
        {
            return ownRepository.GetAll().Where(c => c.Code.StartsWith(parentCode));
        }
        protected virtual IQueryable<TEntity> GetAllSorting(IQueryable<TEntity> query, TGetAllInput input)
        {
            return query.OrderBy(c => c.Code);
        }
        protected Action<TEntity, TDto> GetAllMap = null;
        #endregion
    }
}
