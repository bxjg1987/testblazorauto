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

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TCreateInput、TUpdateInput
     * 
     * 原本考虑再做一层抽象，不与具体ui框架关联，这样就可以更容易替换成其它的ui框架
     * 简单起见，暂时不搞了，若以后需要可以在这个类再抽象下
     * 
     * 只保留逻辑部分的抽象，因为这里的抽象是与具体项目无关的，所以应该由具体项目的子类去按自己需求做布局
     */

    /// <summary>
    /// 抽象的，基于MudBlazor datagrid的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpMudListBaseComponent<TAppService,
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
        /// <summary>
        /// 单击行时回调
        /// 要绑定双击事件时，也只需要绑定此事件，它内部会判断是否是双击，并执行<see cref="RowDoubleClick"/>回调
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected virtual async Task RowClick(DataGridRowClickEventArgs<TEntityDto> arg)
        {
            //短时间内点击的次数>1 就表示双击
            if (arg.MouseEventArgs.Detail > 1)
            {
                await RowDoubleClick(arg);
            }
        }
        /// <summary>
        /// 行双击时回调
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected virtual ValueTask RowDoubleClick(DataGridRowClickEventArgs<TEntityDto> arg)
        {
            return ValueTask.CompletedTask;
        }
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
        protected virtual bool ShouldDisableDelete => dataGrid.Loading || isDeleting || dataGrid.SelectedItems == default || !dataGrid.SelectedItems.Any();
        

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
            foreach (var item in dataGrid.FilteredItems)
            {
                if (item is IExtendableDto dto)
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
            await SafelyExecuteAsync(async () =>
            {
                var temp = await AppService.BatchDeleteAsync(new BatchOperationInput<TPrimaryKey> { Ids = dataGrid.SelectedItems?.Select(x => x.Id).ToArray() });
                BatchOperationMessage(temp, "批量删除");
                //BatchDeleteMessage(temp);
                if (temp.Ids.Count > 0)
                    await dataGrid.ReloadServerData();
                //_ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange
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
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            HideDeleteConfirm();
            var item = curr as IExtendableDto;
            if (item != default)
                item.ExtensionData.IsDeleting = true;
            await SafelyExecuteAsync(async () =>
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(curr.Id));
                Snackbar.Add("删除成功！", Severity.Success);
                //若上面异常，下面不会执行
                //_ = InvokeAsync(dataGrid.ReloadServerData);
                await dataGrid.ReloadServerData();
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
            HideDeleteConfirm();
            if (dto is IExtendableDto item)
                item.ExtensionData.IsShowDeleteConfirmation = true;
        }
        ///// <summary>
        ///// 隐藏删除明细的确认框
        ///// </summary>
        ///// <param name="dto"></param>
        //protected virtual void HideDeleteConfirm(TEntityDto dto)
        //{
        //    if (dto is IExtendableDto item)
        //        item.ExtensionData.IsShowDeleteConfirmation = false;
        //}
        #endregion
    }

    // 若不是使用弹窗，而是使用tab、页面等其它方式时，应提供其它子类

    /// <summary>
    /// 抽象的，基于MudBlazor datagrid的列表页 抽象组件
    /// 使用弹窗弹出新增和详情窗口
    /// </summary>
    /// <typeparam name="TCreateDialog">新增弹窗组件</typeparam>
    /// <typeparam name="TDetailDialog">详情弹窗组件</typeparam>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpMudListDialogBaseComponent<TCreateDialog,
                                                        TDetailDialog,
                                                        TAppService,
                                                        TEntityDto,
                                                        TPrimaryKey,
                                                        TGetAllInput,
                                                        TCreateInput,
                                                        TUpdateInput> : AbpMudListBaseComponent<TAppService,
                                                                                                TEntityDto,
                                                                                                TPrimaryKey,
                                                                                                TGetAllInput,
                                                                                                TCreateInput,
                                                                                                TUpdateInput>
        where TCreateDialog : ComponentBase
        where TDetailDialog : ComponentBase
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 弹窗服务
        /// </summary>
        [Inject]
        protected virtual IDialogService DialogService { get; set; }
        /// <summary>
        /// 新增时的弹窗选项对象
        /// </summary>
        protected virtual DialogOptions DialogAddOptions => new DialogOptions { CloseOnEscapeKey = true };
        /// <summary>
        /// 修改时的弹窗选项对象
        /// </summary>
        protected virtual DialogOptions DialogDetailOptions => DialogAddOptions;
        /// <summary>
        /// 获取新增时传入弹窗的参数
        /// 如：在新增商品时，把列表页当前选中的分类id传递过去
        /// </summary>
        /// <returns></returns>
        protected virtual DialogParameters BuildCreateParameter() => new DialogParameters();
        /// <summary>
        /// 点击新增按钮时执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task BtnAddClick()
        {
            await base.SafelyExecuteAsync(async () =>
            {
                var ps = BuildCreateParameter();
                if (!(await DialogService.Show<TCreateDialog>("新增" + FuncName, ps, DialogAddOptions).Result).Canceled)
                {
                    await dataGrid.ReloadServerData();
                }
            });
        }
        /// <summary>
        /// 行修改按钮点击时执行
        /// 注：不要全局修改按钮，因为木有必要
        /// </summary>
        /// <param name="dto">当前dto对象</param>
        /// <returns></returns>
        protected virtual async Task BtnEditClick(TEntityDto dto)
        {
            await base.SafelyExecuteAsync(async () =>
            {
                var ps = new DialogParameters<TDetailDialog>();
                FillDetailParameters(ps, dto);
                ps.Add("IsEdit", true);
                if (!(await DialogService.Show<TDetailDialog>("修改" + FuncName, ps, DialogDetailOptions).Result).Canceled)
                {
                    await dataGrid.ReloadServerData();
                }
            });
        }
        /// <summary>
        /// 行详情按钮点击时执行
        /// 注：不要全局详情按钮，因为木有必要
        /// </summary>
        /// <param name="dto">当前dto对象</param>
        /// <returns></returns>
        protected virtual async Task BtnDetailClick(TEntityDto dto)
        {
            await base.SafelyExecuteAsync(async () =>
            {
                var ps = new DialogParameters<TDetailDialog>();
                FillDetailParameters(ps, dto);
                ps.Add("IsEdit", false);
                if (!(await DialogService.Show<TDetailDialog>("查看" + FuncName + "详情", ps, DialogDetailOptions).Result).Canceled)
                {
                    //说明在详情页面 又进入了修改 且修改后保存了数据
                    await dataGrid.ReloadServerData();
                }
            });
        }
        protected override async ValueTask RowDoubleClick(DataGridRowClickEventArgs<TEntityDto> arg)
        {
            await BtnDetailClick(arg.Item);
        }
        /// <summary>
        /// 弹出详情弹窗时传入参数
        /// 通常，复杂数据时只传入id，让详情组件自己去重新查询；简单数据时传入当前选择的dto
        /// </summary>
        /// <param name="pms"></param>
        /// <param name="dto"></param>
        protected virtual void FillDetailParameters(DialogParameters<TDetailDialog> pms, TEntityDto dto)
        {
            pms.Add("Dto", dto);
        }
    }
}