using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using BXJG.Common.Dto;
using BXJG.Utils;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Components
{
    /// <summary>
    /// 抽象的，与具体的ui框架无关的，列表页抽象组件
    /// 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
    /// 木有应用统一异常处理拦截器，它的子类（与具体ui有关的抽象列表组件）应使用aop统一异常处理拦截器
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型，分页或不分页</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    /// <typeparam name="TList">列表类型，不同的ui库，列表组件使用的列表对象类型可能不同</typeparam>
    public abstract class AbpListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput,
                                               TList> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>, IExtendableDto
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TList : class, IEnumerable<TEntityDto>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected abstract string FuncName { get; }

        //protected Type DtoType => typeof(TEntityDto);//不是太有必要的，就不要浪费内存了

        //使用这个会导致add按钮的显示不太正常
        ///// <summary>
        ///// 是否应该显示新增按钮，默认 没有正在加载数据时 为true
        ///// </summary>
        //protected virtual bool ShouldEnableCreate => !dataGrid.Loading && !isDeleting;


        //不要在组件中使用AuthorizeAttribute，因为这样会导致组件的渲染速度变慢，因为每次都要去检查权限
        //不要靠应用层定义的权限，因为前后端分离时，应用接口就不应该提供权限名


        //我们只需要状态，不需要存储，以免浪费性能
        ///// <summary>
        ///// 新增权限名称
        ///// </summary>
        //protected virtual string CreatePermissionName => "";
        ///// <summary>
        ///// 修改权限名称
        ///// </summary>
        //protected virtual string UpdatePermissionName => "";
        ///// <summary>
        ///// 删除权限名称
        ///// </summary>
        //protected virtual string DeletePermissionName => "";

        //组件顶部做权限验证即可
        ///// <summary>
        ///// 是否有查看权限
        ///// </summary>
        //protected bool getIsGranted = true;

        /// <summary>
        /// 初始化权限状态
        /// 我们只需要最终是否有某个状态，不需要保留原本的权限字符串，所以使用方法定义，而非虚属性
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default,
            string deletePermissionName = default/*, string getPermissionName = default*/)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = await PermissionChecker.IsGrantedAsync(deletePermissionName);
            //if (getPermissionName.IsNotNullOrWhiteSpaceBXJG())
            //    getIsGranted = await PermissionChecker.IsGrantedAsync(getPermissionName);
        }
        /// <summary>
        /// 批量操作消息提醒
        /// 如：批量删除、批量审核时消息提醒
        /// </summary>
        /// <param name="output">批量操作结果</param>
        /// <param name="funName">操作名</param>
        protected virtual async ValueTask BatchOperationMessage(BatchOperationOutput<TPrimaryKey> output, string funName = "删除")
        {
            if (output.ErrorMessage.Any())
            {
                if (output.Ids.Count == output.ErrorMessage.Count)
                    await ShowFailMessage(msg: $"批量{funName}全部失败！");
                else
                    await ShowFailMessage(msg: $"批量{funName}部分失败！成功数量：{output.Ids.Count}；失败数量：{output.ErrorMessage.Count}");
            }
            else
                await ShowSuccessMessage(msg: $"批量{funName}全部成功！");
        }
     

        #region 列表
        /// <summary>
        /// 搜索条件
        /// </summary>
        protected TGetAllInput GetAllInput = new TGetAllInput();
        /// <summary>
        /// 是否是分页模式
        /// </summary>
        public bool IsPage => GetAllInput is IPagedResultRequest;

        ///// <summary>
        ///// 填充动态条件
        ///// </summary>
        ///// <returns></returns>
        //protected virtual ValueTask FillDynamicConditions(ICollection<ConditionFieldDefine> conditions) => ValueTask.CompletedTask;


        /// <summary>
        /// 获取每页行数，若不做分页请返回0
        /// </summary>
        protected virtual int PageSize
        {
            get
            {
                if (GetAllInput is ILimitedResultRequest dx)
                {
                    return dx.MaxResultCount;
                }
                else if (GetAllInput is IHaveFilter dx1 && dx1.Filter is ILimitedResultRequest dx2)
                {
                    return dx2.MaxResultCount;
                }
                return int.MaxValue;
            }
            set
            {
                if (GetAllInput is ILimitedResultRequest dx)
                {
                    dx.MaxResultCount = value;
                }
                else if (GetAllInput is IHaveFilter dx1 && dx1.Filter is ILimitedResultRequest dx2)
                {
                    dx2.MaxResultCount = value;
                }
            }
        }

        /// <summary>
        /// 排序规则，格式："field1 aes,field2 desc"
        /// </summary>
        public virtual string Sorting
        {
            get
            {
                if (GetAllInput is ISortedResultRequest dxx)
                    return dxx.Sorting;
                else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                    return dx22.Sorting;
                return "Id";
            }
            set
            {

                ISortedResultRequest sd222 = null;
                if (GetAllInput is ISortedResultRequest dxx)
                    sd222 = dxx;
                else if (GetAllInput is IHaveFilter dx11 && dx11.Filter is ISortedResultRequest dx22)
                    sd222 = dx22;

                // var ls = condition.SortModel.Where(c => c.Sort.IsNotNullOrWhiteSpaceBXJG()).OrderBy(c => c.Priority).Select(c => c.FieldName + " " + c.Sort.Replace("end", ""));
                //  sd222.Sorting = string.Join(",", ls);
                sd222.Sorting = value;

                if (sd222.Sorting.IsNullOrWhiteSpaceBXJG())
                    sd222.Sorting = "Id";


            }
        }




        /// <summary>
        /// 当前列表数据
        /// 通常是当前页的数据
        /// </summary>
        protected virtual TList Items { get; set; } = new List<TEntityDto>() as TList;

        /// <summary>
        /// 当前条件下的总数据数量
        /// </summary>
        protected virtual int TotalCount { get; set; }
        /// <summary>
        /// 当前是第几页
        /// </summary>
        protected virtual int PageIndex
        {
            get
            {
                //  int pageIndex = 1;
                //  if (GetAllInput is IPagedResultRequest dx)
                return (GetAllInput as IPagedResultRequest).SkipCount / PageSize + 1;

                //若是纯条件，就木有必要

                //if (pageIndex <= 0)
                //    pageIndex = 1;


            }
            set
            {
                if (GetAllInput is IPagedResultRequest dx)
                {
                    dx.SkipCount = (value - 1) * PageSize;
                }
            }
        }

        /// <summary>
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual TList SelectedItems { get; set; } = new List<TEntityDto>() as TList;
        /// <summary>
        /// 父类仅仅需要读，至于是否可写由子类自己决定
        /// </summary>
        protected virtual bool IsLoading { get; set; }

        

        /// <summary>
        /// 刷新列表
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Refresh()
        {
            await LoadListData();
            if (SelectedItems != default && SelectedItems is ICollection<TEntityDto> list)
                list.Clear();
            else
                SelectedItems = new List<TEntityDto>() as TList;
        }

        /// <summary>
        /// 加载列表数据
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadListData()
        {
            IsLoading = true;
            try
            {
                //var cd = GetAllInput;
                //if (cd is IDynamicCondition cdd)
                //{
                //    cdd.Conditions = new List<ConditionFieldDefine>();// await BuildDynamicCondition();// state.FilterDefinitions.MapToDynamicCondition().ToList();
                //    await FillDynamicConditions(cdd.Conditions as List<ConditionFieldDefine>);
                //}
                //else if (cd is IHaveFilter cddq && cddq.Filter is IDynamicCondition cddqq)
                //{
                //    cddqq.Conditions = new List<ConditionFieldDefine>();// await BuildDynamicCondition();//state.FilterDefinitions.MapToDynamicCondition().ToList();
                //    await FillDynamicConditions(cddqq.Conditions as List<ConditionFieldDefine>);
                //}
                //if (cd is IPagedAndSortedResultRequest cd2)
                //{
                //    cd2.MaxResultCount = PageSize; //state.PageSize;
                //    cd2.SkipCount = (PageIndex - 1) * PageSize;
                //}
                //if (cd is ISortedResultRequest cd3)
                //{
                //    cd3.Sorting = Sorting;// state.SortDefinitions.ToLinqDynamicCore();
                //}

                //if (cd is IHaveKeywords cd4)
                //{
                //    cd4.Keywords = Keywords;
                //}
                //else if (cd is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                //{
                //    cddqq.Keywords = Keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                //}
                //await FillCondtion(cd);
                var dtos = await AppService.GetAllAsync(GetAllInput);

                //_ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
                //dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下

                //给每行属性附加额外状态

                foreach (var dto in dtos.Items)
                {
                    dynamic dd = new ExpandoObject();
                    dd.IsDeleting = false;
                    dd.IsShowDeleteConfirmation = false;
                    await AddItemExtData(dto, dd);
                    dto.ExtensionData = dd;
                }
                Items = dtos.Items as TList;
                TotalCount = dtos.TotalCount;
            }
            finally
            {
                IsLoading = false;
            }
        }
        /// <summary>
        /// 获取列表时，为其中的每项添加额外的数据
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ValueTask AddItemExtData(TEntityDto dto, dynamic data) => ValueTask.CompletedTask;
        /// <summary>
        /// 搜索关键字
        /// </summary>
        protected virtual string Keywords
        {
            get
            {
                if (GetAllInput is IHaveKeywords cd4)
                {
                    return cd4.Keywords;
                }
                else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                {
                    return cddqq.Keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
                return string.Empty;
            }
            set
            {
                if (GetAllInput is IHaveKeywords cd4)
                {
                    cd4.Keywords = value;
                }
                else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                {
                    cddqq.Keywords = value;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
            }
        }
        /// <summary>
        /// 关键字变化时回调，默认修改关键字字段并刷新列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        protected virtual async Task KeywordsChanged(string keywords = default)
        {
            //if (GetAllInput is IHaveKeywords cd4)
            //{
            //    cd4.Keywords = keywords;
            //}
            //else if (GetAllInput is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
            //{
            //    cddqq.Keywords = keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
            //}
            Keywords = keywords;
            await Refresh();
        }
        /// <summary>
        /// 重置搜索条件后刷新
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Reset()
        {
            PageIndex = 1;
            PageSize = 20;
            Keywords = string.Empty;
            await Refresh();
        }
        #endregion

        #region 表单
        //不要顶部的修改按钮
        ///// <summary>
        ///// 是否显示修改按钮，默认勾选了某个行且 没有正在加载数据时为true 
        ///// </summary>
        //protected virtual bool ShouldEnableEdit => !dataGrid.Loading && !isDeleting && dataGrid.SelectedItems != default && dataGrid.SelectedItems.Any();
        /// <summary>
        /// 是否有新增权限
        /// </summary>
        protected bool createIsGranted = true;
        /// <summary>
        /// 是否有修改权限
        /// </summary>
        protected bool updateIsGranted = true;
        #endregion

        #region 删除
        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool deleteIsGranted = true;
        //没权限时不显示的，所以不加入这个判断
        /// <summary>
        /// 是否禁用批量删除按钮，出现任意情况，则为true：正在加载数据；正在删除数据；没有选择数据；
        /// </summary>
        protected virtual bool ShouldDisableDelete => IsLoading || isDeleting || SelectedItems == default || !SelectedItems.Any();
        /// <summary>
        /// 是否批量删除的确认框
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在执行批量删除操作
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 显示批量删除的确认框
        /// </summary>
        protected virtual void ShowDeleteConfirm()
        {
            HideDeleteConfirm();
            isShowDeleteConfirm = true;
        }
        /// <summary>
        /// 隐藏批量删除的确认框
        /// </summary>
        protected virtual void HideDeleteConfirm()
        {
            isShowDeleteConfirm = false;
            foreach (var dto in Items)
            {
                dto.ExtensionData.IsShowDeleteConfirmation = false;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Delete()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            HideDeleteConfirm();
            isDeleting = true;
            try
            {
                var r = await AppService.BatchDeleteAsync(new BatchOperationInput<TPrimaryKey> { Ids = SelectedItems.Select(x => x.Id).ToArray() });
                BatchOperationMessage(r, "批量删除");//这里木有必要await
                //BatchDeleteMessage(temp);
                if (r.Ids.Any())
                    await Refresh();
                //_ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange
            }
            finally
            {
                isDeleting = false;
            }
        }
        /// <summary>
        /// 删除单个项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual async Task Delete(TEntityDto item)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            HideDeleteConfirm();
            item.ExtensionData.IsDeleting = true;
            try
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(item.Id));
                ShowSuccessMessage("删除提示", "删除成功！");//这里木有必要await
                //若上面异常，下面不会执行
                //_ = InvokeAsync(dataGrid.ReloadServerData);
                await Refresh();
            }
            finally
            {
                item.ExtensionData.IsDeleting = false;
            }
        }
        /// <summary>
        /// 显示删除明细的确认框
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void ShowDeleteConfirm(TEntityDto dto)
        {
            HideDeleteConfirm();
            dto.ExtensionData.IsShowDeleteConfirmation = true;
        }
        #endregion
    }
}