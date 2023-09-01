using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using BXJG.Common.Dto;
using BXJG.Utils;
using BXJG.Utils.Dto;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Components
{
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    /// <typeparam name="TGetInput">获取单条数据的输入类型</typeparam>
    /// <typeparam name="TDeleteInput">删除数据时的输入类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput,
                                               TGetInput,
                                               TDeleteInput> : AbpMudBlazorBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TGetInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TDeleteInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
    {
        #region 基础

        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        private TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();


        //private IDialogService dialogService;

        /// <summary>
        /// 经过测试，它是Scope的生命周期，且使用ScopedServices.GetRequiredService方式不能用
        /// </summary>
        [Inject]
        protected virtual IDialogService DialogService { get; set; }
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => $"请重写{nameof(FuncName)}属性";

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

        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="createPermissionName"></param>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default, string deletePermissionName = default)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                createIsGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = await PermissionChecker.IsGrantedAsync(deletePermissionName);
        }
        /// <summary>
        /// 批量操作消息提醒
        /// </summary>
        /// <param name="output">批量操作结果</param>
        /// <param name="funName">操作名</param>
        protected virtual void BatchOperationMessage(BatchOperationOutput<TPrimaryKey> output, string funName = "操作")
        {
            if (output.ErrorMessage.Any())
            {
                if (output.Ids.Count == output.ErrorMessage.Count)
                    Snackbar.Add($"批量{funName}全部失败！", Severity.Error);
                else
                    Snackbar.Add($"批量{funName}部分失败！成功数量：{output.Ids.Count}；失败数量：{output.ErrorMessage.Count}", Severity.Warning);
            }
            else
                Snackbar.Add($"批量{funName}全部成功！", Severity.Success);
        }
        #endregion

        #region 生命周期
        //protected override async Task OnInitialized2Async()
        //{
        //    await InitPermission();
        //}
        #endregion

        #region 权限

        #endregion

        #region 列表

        protected MudDataGrid<TEntityDto> dataGrid;
        ///// <summary>
        ///// 当前页码
        ///// </summary>
        //protected virtual int pageIndex { get => dataGrid.CurrentPage; set => dataGrid.CurrentPage = value; }
        ///// <summary>
        ///// 页大小
        ///// </summary>
        //protected virtual int PageSize{ get => dataGrid.RowsPerPage; set => dataGrid.RowsPerPage = value; }

        ///// <summary>
        ///// 总页数
        ///// </summary>
        //protected int pageCount =1;

        ///// <summary>
        ///// 是否正在加载数据
        ///// </summary>
        //protected virtual bool isLoading => dataGrid != default && dataGrid.Loading;// false;
        /// <summary>
        /// 表格数据加载
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual async Task<GridData<TEntityDto>> LoadDataAsync(GridState<TEntityDto> state)
        {
            

            return await SafeExecuteAsync(async () =>
            {
                var cd = new TGetAllInput();
                if (cd is IDynamicCondition cdd)
                {
                    cdd.Conditions = state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
                else if (cd is IHaveFilter cddq && cddq.Filter is IDynamicCondition cddqq)
                {
                    cddqq.Conditions = state.FilterDefinitions.MapToDynamicCondition().ToList();
                }

                if (cd is IPagedAndSortedResultRequest cd2)
                {
                    cd2.MaxResultCount = state.PageSize;
                    cd2.SkipCount = state.Page * state.PageSize;
                }
                if (cd is ISortedResultRequest cd3)
                {
                    cd3.Sorting = state.SortDefinitions.ToLinqDynamicCore();
                }

                if (cd is IHaveKeywords cd4)
                {
                    cd4.Keywords = keywords;
                }
                else if (cd is IHaveFilter cddq && cddq.Filter is IHaveKeywords cddqq)
                {
                    cddqq.Keywords = keywords;// state.FilterDefinitions.MapToDynamicCondition().ToList();
                }
                await FillCondtion(cd);
                var dtos = await AppService.GetAllAsync(cd);

                _ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
                dataGrid.SelectedItems.Clear();
                
                return new GridData<TEntityDto>
                {
                    TotalItems = dtos.TotalCount,
                    Items = dtos.Items
                };
            });
        }
        /// <summary>
        /// 默认已填充动态条件和关键字，你可以重写以填充其它条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask FillCondtion(TGetAllInput input) => ValueTask.CompletedTask;

        ///// <summary>
        ///// 已选中的项
        ///// </summary>
        //protected virtual HashSet<TEntityDto> selectedItems => dataGrid?.SelectedItems;// new HashSet<TEntityDto>();
        /// <summary>
        /// 批量选择变化时回调
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual Task SelectedItemsChanged(HashSet<TEntityDto> items)
        {
            StateHasChanged();
            //selectedItems = items;
            return Task.CompletedTask;
            //_events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
        }
        //未选中项时，直接禁用按钮
        ///// <summary>
        ///// 批量操作时，检查是否有选中项
        ///// </summary>
        ///// <returns></returns>
        //protected void CheckSelect()
        //{
        //}

        protected string keywords = "";
        protected virtual async Task KeywordsChanged(string keywords)
        {
            await base.SafeExecuteAsync(async () =>
            {
                this.keywords = keywords;
                await dataGrid.ReloadServerData();
            });
        }
        #endregion

        #region 表单

        /// <summary>
        /// 是否显示修改按钮，默认勾选了某个行且 没有正在加载数据时为true 
        /// </summary>
        protected virtual bool ShouldEnableEdit => !dataGrid.Loading && !isDeleting && dataGrid.SelectedItems != default && dataGrid.SelectedItems.Any();

        /// <summary>
        /// 是否有新增权限
        /// </summary>
        protected bool createIsGranted = true;
        /// <summary>
        /// 是否有修改权限
        /// </summary>
        protected bool updateIsGranted = true;
        /// <summary>
        /// 新增或修改的弹窗
        /// </summary>
        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; }
        /// <summary>
        /// 新增或修改弹窗的配置对象
        /// </summary>
        protected virtual DialogOptions DialogOptions => new DialogOptions { CloseOnEscapeKey = true };
        /// <summary>
        /// 点击新增按钮
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Add()
        {
            await base.SafeExecuteAsync(async () =>
            {
                var dr = await DialogService.Show<TFormComponent>("新增" + FuncName, DialogOptions).Result;
                if (dr.Canceled)
                    return;
                //this.pageIndex = 1;
                await dataGrid.ReloadServerData();
            });
        }
        /// <summary>
        /// 点击新增按钮
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Edit()
        {
            await base.SafeExecuteAsync(async () =>
            {
                var dr = await DialogService.Show<TFormComponent>("修改" + FuncName, DialogOptions).Result;
                if (dr.Canceled)
                    return;
                await dataGrid.ReloadServerData();
            });
        }
        #endregion

        #region 删除

        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool deleteIsGranted = true;
        /// <summary>
        /// 是否显示删除按钮，默认勾选了某个行且 没有正在加载数据时为true
        /// </summary>
        protected virtual bool ShouldEnableDelete => !dataGrid.Loading && !isDeleting && dataGrid.SelectedItems != default && dataGrid.SelectedItems.Any();
        /// <summary>
        /// 是否显示全局的删除确认
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在执行删除操作
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 显示删除确认
        /// </summary>
        protected virtual void ShowDeleteConfirm()
        {
            isShowDeleteConfirm = true;
        }
        /// <summary>
        /// 隐藏删除确认框
        /// </summary>
        protected virtual void HideDeleteConfirm()
        {
            isShowDeleteConfirm = false;
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<BatchOperationOutput<TPrimaryKey>> Delete()
        {
            isDeleting = true;
            var r = await SafeExecuteAsync(async () =>
            {
                var temp = await AppService.BatchDeleteAsync(new BatchOperationInput<TPrimaryKey> { Ids = dataGrid.SelectedItems?.Select(x => x.Id).ToArray() }).ContinueWith(async t =>
                {
                    if (t.IsCompletedSuccessfully)
                        _ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange

                    return t.Result;
                });
                return await temp;
            });
            isDeleting = false;
            isShowDeleteConfirm = false;
            BatchOperationMessage(r, "批量删除");
            return r;
        }

        //protected virtual void DeleteMessage()
        //{

        //}
        /// <summary>
        /// 删除单个项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task Delete(TDeleteInput input)
        {
            await SafeExecuteAsync(async () =>
            {
                var curr = dataGrid.SelectedItems.Single(c => c.Id!.Equals(input.Id));
                var item = curr as IHaveIsDeleting;
                if (item != default)
                    item.IsDeleting = true;
                try
                {
                    await AppService.DeleteAsync(input);
                }
                finally
                {
                    if (item != default)
                        item.IsDeleting = false;
                }
                //若上面异常，下面不会执行
                await dataGrid.ReloadServerData();
            });
        }
        #endregion

    }

    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    /// <typeparam name="TGetInput">获取单条数据的输入类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput,
                                               TGetInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                          TFormComponent,
                                                                                          TEntityDto,
                                                                                          TPrimaryKey,
                                                                                          TGetAllInput,
                                                                                          TCreateInput,
                                                                                          TUpdateInput,
                                                                                          TGetInput,
                                                                                          EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                             TFormComponent,
                                                                                             TEntityDto,
                                                                                             TPrimaryKey,
                                                                                             TGetAllInput,
                                                                                             TCreateInput,
                                                                                             TUpdateInput,
                                                                                             EntityDto<TPrimaryKey>>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                             TFormComponent,
                                                                                             TEntityDto,
                                                                                             TPrimaryKey,
                                                                                             TGetAllInput,
                                                                                             TCreateInput,
                                                                                             TCreateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                             TFormComponent,
                                                                                             TEntityDto,
                                                                                             TPrimaryKey,
                                                                                             TGetAllInput,
                                                                                             TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto,
                                               TPrimaryKey> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                            TFormComponent,TEntityDto,
                                                                                            TPrimaryKey,
                                                                                            PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TFormComponent">表单弹窗组件类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TFormComponent,
                                               TEntityDto> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                           TFormComponent,TEntityDto,
                                                                                           int>
        where TEntityDto : IEntityDto<int>
        where TFormComponent : ComponentBase
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}