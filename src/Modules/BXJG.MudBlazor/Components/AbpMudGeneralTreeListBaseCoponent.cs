using Abp.Application.Services.Dto;
using Abp.UI;
using BXJG.AbpMudBlazor.Interceptor;
using BXJG.Common.Dto;
using BXJG.Utils.Dto;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Notification;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MudBlazor.CategoryTypes;

namespace BXJG.AbpMudBlazor.Components
{
    /*
     * c u d 某个节点后，会希望刷新下它所在的列表，也就是重新加载其父节点下的子节点。
     * mudblazor的MudTreeViewItem提供了此方法，但我们不方便拿到它的引用
     * 所以在dto的扩展属性中定义对MudTreeViewItem控件本身的引用，因为在逻辑代码中我们很容易拿到dto，进而拿到控件的引用
     * 
     * 由于treeView本身没有实现重新加载整个树，而只是 MudTreeViewItem上实现了重新加载
     * 所以在管理时，我们需要一个 顶级的，id为空0的节点，
     * 参考：https://github.com/MudBlazor/MudBlazor/issues/4434
     */

    /// <summary>
    /// 抽象的，基于MudBlazor treeView的列表页抽象组件
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>

    public abstract class AbpMudGeneralTreeListBaseCoponent<TAppService,
                                                            TEntityDto,
                                                            TCreateInput,
                                                            TEditDto,
                                                            TGetAllInput> : AbpMudBaseComponent
        //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : GeneralTreeGetTreeNodeBaseDto<TEntityDto>, IExtendableDto//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : GeneralTreeGetTreeInput, new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
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
        protected virtual async Task InitPermission(string createPermissionName = default, string updatePermissionName = default, string deletePermissionName = default/*, string getPermissionName = default*/)
        {
            if (createPermissionName.IsNotNullOrWhiteSpaceBXJG())
                isCreateGranted = await PermissionChecker.IsGrantedAsync(createPermissionName);
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                isUpdateGranted = await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                isDeleteGranted = await PermissionChecker.IsGrantedAsync(deletePermissionName);
            //if (getPermissionName.IsNotNullOrWhiteSpaceBXJG())
            //    getIsGranted = await PermissionChecker.IsGrantedAsync(getPermissionName);
        }
        /// <summary>
        /// 批量操作消息提醒
        /// 如：批量删除、批量审核时消息提醒
        /// </summary>
        /// <param name="output">批量操作结果</param>
        /// <param name="funName">操作名</param>
        protected virtual void BatchOperationMessage(BatchOperationOutputLong output, string funName = "操作")
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
        // [ExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await Reload();
        }
        #endregion

        #region 权限

        #endregion

        #region 列表
        /// <summary>
        /// 对MudTreeView的引用
        /// </summary>
        protected MudTreeView<TEntityDto>? mudTreeView;
        /// <summary>
        /// 勾选的项集合
        /// </summary>
        protected HashSet<TEntityDto>? selected;
        /// <summary>
        /// 顶级节点列表
        /// </summary>
        protected HashSet<TEntityDto> tops;
        /// <summary>
        /// 当前激活的项
        /// </summary>
        protected TEntityDto? activatedValue;
        ////protected Dictionary<long, MudTreeViewItem<TEntityDto>> mudTreeViewItem;
        ////protected HashSet<TEntityDto> items;
        /////// <summary>
        /////// 当前页码
        /////// </summary>
        ////protected virtual int pageIndex { get => dataGrid.CurrentPage; set => dataGrid.CurrentPage = value; }
        /////// <summary>
        /////// 页大小
        /////// </summary>
        ////protected virtual int PageSize{ get => dataGrid.RowsPerPage; set => dataGrid.RowsPerPage = value; }

        /////// <summary>
        /////// 总页数
        /////// </summary>
        ////protected int pageCount =1;

        /////// <summary>
        /////// 是否正在加载数据
        /////// </summary>
        ////protected virtual bool isLoading => dataGrid != default && dataGrid.Loading;// false;
        //public async Task<HashSet<TEntityDto>> LoadServerData(TEntityDto? parentNode = null)
        //{
        //    return await SafelyExecuteAsync(async () => await LoadDataCore(parentNode));
        //}


        ///// <summary>
        ///// 刷新
        ///// </summary>
        ///// <param name="code">若为空则刷新整棵树</param>
        ///// <returns></returns>
        //protected virtual async Task Refresh(TEntityDto dto = null)
        //{
        //    if (dto == null)
        //        tops = await LoadDataCore();
        //    else
        //    {
        //        await ((dto as IExtendableDto).ExtensionData.Control as MudTreeViewItem<TEntityDto>).ReloadAsync();
        //    }
        //}
        /// <summary>
        /// 是否正在加载顶级节点
        /// </summary>
        protected bool isLoadingTops;
        /// <summary>
        /// 刷新节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task<HashSet<TEntityDto>> Load(TEntityDto? parentNode = null)
        {
            var cd = new TGetAllInput();
            await FillCondtion(cd);
            if (cd.ParentCode.IsNullOrWhiteSpaceBXJG())
                cd.ParentCode = parentNode?.Code;

            var dtos = await AppService.GetAllAsync(cd);
            if (parentNode != null)
                dtos = dtos.SingleOrDefault()?.Children;

            if (dtos == null || !dtos.Any())
                return null;
            //else
            //{
            //    tops = dtos.ToHashSet();
            //    return tops;
            //}

            //_ = InvokeAsync(StateHasChanged);//让多选影响顶部按钮得以执行 包一层是因为需要加载完才执行
            //mudTreeView.r
            //dataGrid.SelectedItems.Clear();//翻页时，已选择的好像木有清空，这里手动来下
            foreach (var dto in dtos)
            {
                //  dto.State= dto.Children!=null&& dto.Children
                //  dto.Children.Clear();

                dynamic dd = new ExpandoObject();
                dd.IsDeleting = false;
                dd.IsShowDeleteConfirmation = false;
                dd.Control = new object();// new MudTreeViewItem<TEntityDto>();//准备一个变量，用于引用节点的控件
                dd.ExtData = dto.ExtensionData;
                (dto as IExtendableDto).ExtensionData = dd;
            }

            if (parentNode != null)
                parentNode.Children = dtos;

            return dtos.ToHashSet();


        }
        /// <summary>
        /// 重新加载子节点列表
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <returns></returns>
        protected virtual async Task Reload(TEntityDto? parentNode = null)
        {
            if (parentNode == null)
            {
                isLoadingTops = true;
                try
                {
                    tops = await Load();
                }
                finally
                {
                    isLoadingTops = false;
                }
            }
            else
            {


                //  var dtos = await Load(parentNode);
                //  parentNode.Children= dtos.ToList();

                var ct = (parentNode as IExtendableDto).ExtensionData.Control as MudTreeViewItem<TEntityDto>;//配合界面的ref，这里可以拿到控件引用，
                await ct.ReloadAsync();

            }
            //this.StateHasChanged();
        }

        /// <summary>
        /// 你可以重写以填充其它条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual ValueTask FillCondtion(TGetAllInput input) => ValueTask.CompletedTask;

        ///// <summary>
        ///// 已选中的项
        ///// </summary>
        //protected virtual HashSet<TEntityDto> selectedItems => dataGrid?.SelectedItems;// new HashSet<TEntityDto>();
        ///// <summary>
        ///// 批量选择变化时回调
        ///// </summary>
        ///// <param name="items"></param>
        ///// <returns></returns>
        //protected virtual Task SelectedItemsChanged(HashSet<TEntityDto> items)
        //{
        //    StateHasChanged();
        //    //selectedItems = items;
        //    return Task.CompletedTask;
        //    //_events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
        //}
        //未选中项时，直接禁用按钮
        ///// <summary>
        ///// 批量操作时，检查是否有选中项
        ///// </summary>
        ///// <returns></returns>
        //protected void CheckSelect()
        //{
        //}

        //protected string keywords = "";
        //protected virtual async Task KeywordsChanged(string keywords)
        //{
        //    await base.SafelyExecuteAsync(async () =>
        //    {
        //        this.keywords = keywords;
        //        await dataGrid.ReloadServerData();
        //    });
        //}
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
        protected bool isCreateGranted = true;
        /// <summary>
        /// 是否有修改权限
        /// </summary>
        protected bool isUpdateGranted = true;

        #endregion

        #region 删除
        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool isDeleteGranted = true;
        //没权限时不显示的，所以不加入这个判断
        /// <summary>
        /// 是否禁用批量删除按钮，出现任意情况，则为true：正在加载数据；正在删除数据；没有选择数据；
        /// </summary>
        protected virtual bool IsDisableDelete => isDeleting || selected == default || !selected.Any();

        /// <summary>
        /// 是否批量删除的确认框
        /// </summary>
        protected bool isShowDeleteConfirm = false;
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
            foreach (var item in mudTreeView.Items)
            {
                HideDeleteItemConfirm(item);
            }
        }
        /// <summary>
        /// 递归向下，隐藏所有明细的删除确认框
        /// </summary>
        protected virtual void HideDeleteItemConfirm(TEntityDto dto)
        {
            (dto as IExtendableDto).ExtensionData.IsShowDeleteConfirmation = false;
            if (dto.Children != null && dto.Children.Any())
            {
                foreach (var item in dto.Children)
                {
                    HideDeleteItemConfirm(item);
                }
            }
        }
        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task BtnDeleteClick()
        //{
        //    await SafelyExecuteAsync(DeleteCore);
        //}
        /// <summary>
        /// 是否正在执行批量删除操作
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 批量删除核心逻辑
        /// </summary>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnDeleteClick()
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            //木有选择时，删除按钮是禁用的，因此这里木有必要判断是否有选择项
            isDeleting = true;
            try
            {
                HideDeleteConfirm();

                var r = await AppService.DeleteAsync(new BatchOperationInputLong { Ids = selected.Select(x => x.Id).ToArray() });
                BatchOperationMessage(r, "批量删除");
                //BatchDeleteMessage(temp);
                if (r.Ids.Any())
                {
                    var tmp = selected.OrderBy(c => c.Code.Length).FirstOrDefault();
                    if(tmp!=null)

                    {
                        var node = mudTreeView.Items.FindRecursiveDown(tmp.ParentId.Value);
                        await Reload(node);
                    }
          else
                        await Reload();

                }

                //刷新选择节点中最短code的父节点，也就是所有选择节点的公共父节点
                //_ = InvokeAsync(dataGrid.ReloadServerData); //内部会StateChange
            }
            finally
            {
                isDeleting = false;
            }
        }


        //protected TEntityDto Find() { 

        //}

        /// <summary>
        /// 点击删除单个项的按钮时回调
        /// </summary>
        /// <param name="curr"></param>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnDeleteItemClick(TEntityDto curr)
        {
            //不要再判断权限了，因为没有权限的，按钮不会显示，且应用服务本身还会验证权限
            // var curr = dataGrid.Items.Single(c => c.Id!.Equals(input.Id));
            var item = curr as IExtendableDto;
            //  if (item != default)
            item.ExtensionData.IsDeleting = true;

            try
            {
                HideDeleteConfirm();
                var r = await AppService.DeleteAsync(new BatchOperationInputLong { Ids = new[] { curr.Id } });
                if (r.Ids.Any())
                {
                    if (curr.ParentId.HasValue)
                    {
                        var node = mudTreeView.Items.FindRecursiveDown(curr.ParentId.Value);
                        await Reload(node);
                    }
                    else
                        await Reload();
                    Snackbar.Add("删除成功！", Severity.Success);
                }
                else
                {
                    //因为有全局异常拦截器，直接抛出异常吧
                    throw new UserFriendlyException($"删除失败！{r.ErrorMessage.SingleOrDefault()?.Message}");
                    //Snackbar.Add("删除失败！" + r.ErrorMessage.SingleOrDefault()?.Message, Severity.Error);
                }
            }
            finally
            {
                //  if (item != default)
                item.ExtensionData.IsDeleting = false;
            }
        }

        /// <summary>
        /// 显示删除明细的确认框
        /// </summary>
        /// <param name="dto"></param>
        protected virtual void ShowDeleteItemConfirm(TEntityDto dto)
        {
            HideDeleteConfirm();
            (dto as IExtendableDto).ExtensionData.IsShowDeleteConfirmation = true;
        }
        #endregion
    }
    /// <summary>
    /// 抽象的，基于MudBlazor treeView的列表页抽象组件
    /// </summary>
    /// <typeparam name="TCreateDialog">新增弹窗组件</typeparam>
    /// <typeparam name="TDetailDialog">详情弹窗组件</typeparam>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    public abstract class AbpMudGeneralTreeListDialogBaseCoponent<TCreateDialog,
                                                                  TDetailDialog,
                                                                  TAppService,
                                                                  TEntityDto,
                                                                  TCreateInput,
                                                                  TEditDto,
                                                                  TGetAllInput> : AbpMudGeneralTreeListBaseCoponent<TAppService,
                                                                                                                    TEntityDto,
                                                                                                                    TCreateInput,
                                                                                                                    TEditDto,
                                                                                                                    TGetAllInput>
        where TCreateDialog : ComponentBase
        where TDetailDialog : ComponentBase
        //where TCreateInput : GeneralTreeNodeEditBaseDto //注意这里约束为TEditDto，这样强制要求继承编辑模型不合理
        where TEntityDto : GeneralTreeGetTreeNodeBaseDto<TEntityDto>, IExtendableDto//, new()
        //where TEditDto : GeneralTreeNodeEditBaseDto//父类可以对输入做一定的处理
        where TGetAllInput : GeneralTreeGetTreeInput, new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
        /// <summary>
        /// 弹窗服务
        /// </summary>
        [Inject]
        protected virtual IDialogService DialogService { get; set; }

        /// <summary>
        /// 新增时的弹窗选项对象
        /// </summary>
        protected virtual DialogOptions DialogAddOptions => new DialogOptions { CloseOnEscapeKey = true, FullWidth = true/*, DisableBackdropClick=true*/ };//DisableBackdropClick保留它，以便我们可以使用弹窗的OnBackdropClick事件
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
        [ExceptionInterceptor]
        protected virtual async Task BtnAddClick()
        {
            var ps = BuildCreateParameter();
            if (!(await DialogService.Show<TCreateDialog>("新增" + FuncName, ps, DialogAddOptions).Result).Canceled)
            {
                await dataGrid.ReloadServerData();
            }
        }
        /// <summary>
        /// 行修改按钮点击时执行
        /// 注：不要全局修改按钮，因为木有必要
        /// </summary>
        /// <param name="dto">当前dto对象</param>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnEditClick(TEntityDto dto)
        {
            var ps = new DialogParameters<TDetailDialog>();
            FillDetailParameters(ps, dto);
            ps.Add("IsEdit", true);
            if (!(await DialogService.Show<TDetailDialog>("修改" + FuncName, ps, DialogDetailOptions).Result).Canceled)
            {
                await dataGrid.ReloadServerData();
            }
        }
        /// <summary>
        /// 行详情按钮点击时执行
        /// 注：不要全局详情按钮，因为木有必要
        /// </summary>
        /// <param name="dto">当前dto对象</param>
        /// <returns></returns>
        [ExceptionInterceptor]
        protected virtual async Task BtnDetailClick(TEntityDto dto)
        {
            var ps = new DialogParameters<TDetailDialog>();
            FillDetailParameters(ps, dto);
            ps.Add("IsEdit", false);
            if (!(await DialogService.Show<TDetailDialog>("查看" + FuncName + "详情", ps, DialogDetailOptions).Result).Canceled)
            {
                //说明在详情页面 又进入了修改 且修改后保存了数据
                await dataGrid.ReloadServerData();
            }
        }
        // 树的双击是用来展开折叠的
        // protected override async ValueTask RowDoubleClick(DataGridRowClickEventArgs<TEntityDto> arg)
        // {
        //    // if (updateIsGranted)
        //   //  {
        //   //      await BtnEditClick(arg.Item); }
        //   //  else
        //  //   {
        //         await BtnDetailClick(arg.Item);
        //   //  }
         
        // }
        /// <summary>
        /// 弹出详情弹窗时传入参数
        /// 通常，复杂数据时只传入id，让详情组件自己去重新查询；简单数据时传入当前选择的dto
        /// </summary>
        /// <param name="pms"></param>
        /// <param name="dto"></param>
        protected virtual void FillDetailParameters(DialogParameters<TDetailDialog> pms, TEntityDto dto)
        {
            pms.Add("Id", dto.Id);
        }
    }
}