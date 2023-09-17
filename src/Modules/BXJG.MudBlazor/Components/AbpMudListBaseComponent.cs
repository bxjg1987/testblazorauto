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
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Components
{
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public class AbpMudListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput,
                                               TUpdateInput> : AbpMudBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
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
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => $"请重写{nameof(FuncName)}属性";

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

        /// <summary>
        /// 初始化权限状态
        /// 我们只需要最终是否有某个状态，不需要保留原本的权限字符串，所以使用方法定义，而非虚属性
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
        ///// <summary>
        ///// 批量删除消息提醒
        ///// </summary>
        ///// <param name="output"></param>
        //protected virtual void BatchDeleteMessage(BatchOperationOutput<TPrimaryKey> output) => BatchOperationMessage(output, "删除");
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
            return await SafelyExecuteAsync(async () =>
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
                dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下

                //给每行属性附加额外状态
                if (typeof(IExtendableDto).IsAssignableFrom(typeof(TEntityDto)))
                {
                    foreach (var item in dtos.Items)
                    {
                        var dto = item as IExtendableDto;
                        dynamic dd = new ExpandoObject();
                        dd.IsDeleting = false;
                        dd.IsShowDeleteConfirmation = false;
                        dto.ExtensionData = dd;
                    }
                }

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
            await base.SafelyExecuteAsync(async () =>
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
        ///// <summary>
        ///// 新增或修改的弹窗
        ///// </summary>
        //[CascadingParameter]
        //protected MudDialogInstance MudDialog { get; set; }
        ///// <summary>
        ///// 新增或修改弹窗的配置对象
        ///// </summary>
        //protected virtual DialogOptions DialogOptions => new DialogOptions { CloseOnEscapeKey = true };
        ///// <summary>
        ///// 点击新增按钮
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task Add()
        //{
        //    await base.SafelyExecuteAsync(async () =>
        //    {
        //        var dr = await DialogService.Show<TFormComponent>("新增" + FuncName, DialogOptions).Result;
        //        if (dr.Canceled)
        //            return;
        //        //this.pageIndex = 1;
        //        await dataGrid.ReloadServerData();
        //    });
        //}
        ///// <summary>
        ///// 点击新增按钮
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task Edit()
        //{
        //    await base.SafelyExecuteAsync(async () =>
        //    {
        //        var dr = await DialogService.Show<TFormComponent>("修改" + FuncName, DialogOptions).Result;
        //        if (dr.Canceled)
        //            return;
        //        await dataGrid.ReloadServerData();
        //    });
        //}
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
            foreach (var item in dataGrid.FilteredItems)
            {
                HideDeleteConfirm(item);
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Delete()
        {
            HideDeleteConfirm();
            isDeleting = true;
            await SafelyExecuteAsync(async () =>
            {
                var temp = await AppService.BatchDeleteAsync(new BatchOperationInput<TPrimaryKey> { Ids = dataGrid.SelectedItems?.Select(x => x.Id).ToArray() });
                BatchOperationMessage(temp, "批量删除");
                //BatchDeleteMessage(temp);
                if (temp.Ids.Count > 0)
                    _ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange
            });
            isDeleting = false;
        }
        //protected virtual void DeleteMessage()
        //{

        //}
        /// <summary>
        /// 删除单个项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task Delete(TEntityDto curr)
        {
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            HideDeleteConfirm(curr);
            var item = curr as IExtendableDto;
            if (item != default)
                item.ExtensionData.IsDeleting = true;
            await SafelyExecuteAsync(async () =>
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(curr.Id));
                Snackbar.Add("删除成功！", Severity.Success);
                //若上面异常，下面不会执行
                _ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange
            });
            if (item != default)
                item.ExtensionData.IsDeleting = false;
        }
        /// <summary>
        /// 显示删除明细的确认框
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void ShowDeleteConfirm(TEntityDto dto)
        {
            if (dto is IExtendableDto item)
                item.ExtensionData.IsShowDeleteConfirmation = true;
        }
        /// <summary>
        /// 隐藏删除明细的确认框
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void HideDeleteConfirm(TEntityDto dto)
        {
            if (dto is IExtendableDto item)
                item.ExtensionData.IsShowDeleteConfirmation = false;
        }
        #endregion

    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput,
                                               TCreateInput> : AbpMudListBaseComponent<TAppService,
                                                                                             TEntityDto,
                                                                                             TPrimaryKey,
                                                                                             TGetAllInput,
                                                                                             TCreateInput,
                                                                                             TCreateInput>
        where TCreateInput : IEntityDto<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
    {
    }

    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey,
                                               TGetAllInput> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                             TEntityDto,
                                                                                             TPrimaryKey,
                                                                                             TGetAllInput,
                                                                                             TEntityDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TEntityDto,
                                               TPrimaryKey> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                            TEntityDto,
                                                                                            TPrimaryKey,
                                                                                            PagedAndSortedResultRequestDto>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey>
    {
    }
    /// <summary>
    /// 抽象的，基于MudBlazor的列表页 抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    public class AbpMudBlazorListBaseComponent<TAppService,
                                               TEntityDto> : AbpMudBlazorListBaseComponent<TAppService,
                                                                                           TEntityDto,
                                                                                           int>
        where TEntityDto : IEntityDto<int>
        where TAppService : ICrudBaseAppService<TEntityDto>
    {
    }
}