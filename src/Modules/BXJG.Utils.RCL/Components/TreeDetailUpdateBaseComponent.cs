
using Abp.Application.Services.Dto;
using Abp.ObjectMapping;
using BXJG.Common.Contracts;
using BXJG.Utils.Application.Share.GeneralTree;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
namespace BXJG.Utils.RCL.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput和TCreateInput
     * 不要考虑同一个组件反复编辑不同的数据，那样稍微复杂了点，是特殊情况，就具体项目中特殊处理，暂时不考虑放抽象类中
     * 编辑时可能要加载些下拉框数据，这通常比较耗时，若在初始化组件时就加载，在用户仅仅查看时就比较浪费，所以在首次进入编辑模式时再做这些操作。
     * 
     * blazor文档中推荐不要在组件内部修改 [Parameter]属性，这种属性仅仅用于外部组件向其传递参数用，可能由于外部组件的刷新，导致此组件状态异常
     * 
     * 虽然有些简单的数据列表页可能直接传递dto过来进行处理，但有时候列表数据过于复杂，需要重新查询下，简单起见统一为根据id重新查询。
     * 若方法是虚的，则返回类型通通使用ValueTask，因为子类重写时可能不是异步的
     */

    /// <summary>
    /// 基于antblazor和abp的通用详情页组件，它包含查看详情页和修改，以及二者之间的切换
    /// 新增抽象组件是单独定义的，因为它是对数据从无到有的创建，而详情组件是对以后的数据进行查看和处理
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TEditDto">修改时的输入类型</typeparam>
    public abstract class TreeDetailUpdateBaseComponent<TAppService,
                                                        TEntityDto,
                                                        TCreateInput,
                                                        TEditDto,
                                                        TGetAllInput> : BaseComponent
        where TEntityDto : IGeneralTree<TEntityDto>, IExtendableObj,new()
        //where TGetAllInput : new()
        where TEditDto : IHaveParentId<long>,new()
        where TAppService : IGeneralTreeBaseAppService<TEntityDto, TCreateInput, TEditDto, TGetAllInput>
    {
        #region 字段和属性

        //ui比较独立，使用更简单的库做映射

        /// <summary>
        /// 请调用ObjectMapper
        /// </summary>
        IObjectMapper objectMapper;
        /// <summary>
        /// 对象映射接口
        /// </summary>
        protected virtual IObjectMapper ObjectMapper => objectMapper ??= ScopedServices.GetRequiredService<IObjectMapper>();
        /// <summary>
        /// 缓存当前主服务对象
        /// </summary>
        TAppService? appService;
        /// <summary>
        /// 获取主服务
        /// </summary>
        protected virtual TAppService AppService => appService ??= ScopedServices.GetRequiredService<TAppService>();
        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected virtual string FuncName => $"请重写{nameof(FuncName)}属性";
        /// <summary>
        /// id
        /// </summary>
        [Parameter]
        public virtual long Id { get; set; }
        /// <summary>
        /// 查询模型
        /// </summary>
        protected TEntityDto? dto=new TEntityDto();
        /// <summary>
        /// 当前编辑模型
        /// </summary>
        protected TEditDto? editDto;
        //ant好像木有很好的支持这俩
        ///// <summary>
        ///// 编辑上下文
        ///// </summary>
        //protected EditContext? editContext;
        ///// <summary>
        ///// 验证消息存储器
        ///// </summary>
        //protected ValidationMessageStore? validationMessageStore;
     
        #endregion

        #region 生命周期
        /// <summary>
        /// 初始化时回调，默认根据id从应用服务接口获取单个数据，然后判断并进入编辑模式
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected override async Task OnInitializedAsync()
        {
            //Abp.ObjectMapping.
            isEdit = IsEdit;
            //dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
            if (isEdit)
            {
               editDto = new TEditDto();
                //editContext = new EditContext(editDto);
                await ResetCore();
            }
            else
                await RefreshCore();
            //if (isEdit)
            //{
            //    await BtnBeginEditClick();
            //    // await ResetCore();
            //}
        }
        //protected virtual ValueTask<TUpdateInput> Create
        #endregion

        #region 权限

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        /// <summary>
        /// 请使用AuthorizationService
        /// </summary>
        IAuthorizationService authorizationService;
        /// <summary>
        /// 授权检查服务
        /// </summary>
        protected virtual IAuthorizationService AuthorizationService => authorizationService ??= ScopedServices.GetRequiredService<IAuthorizationService>();
        ///// <summary>
        ///// 是否有查看权限
        ///// </summary>
        //protected bool getIsGranted = true;
        /// <summary>
        /// 是否有修改权限
        /// </summary>
        protected bool updateIsGranted = true;
        /// <summary>
        /// 是否有删除权限
        /// </summary>
        protected bool deleteIsGranted = true;

        //木有必要管查询权限，因为UI层面的权限本就是让界面更友好，应用服务本身有权限判断兜底了，没有查询权限时，外出组件不应显示响应按钮导航或显示此组件

        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async ValueTask InitPermission(string updatePermissionName = default, string deletePermissionName = default/*, string getPermissionName =default*/)
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, updatePermissionName)).Succeeded;// await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, deletePermissionName)).Succeeded;//await PermissionChecker.IsGrantedAsync(deletePermissionName);
            //if (getPermissionName.IsNotNullOrWhiteSpaceBXJG())
            //    getIsGranted = await PermissionChecker.IsGrantedAsync(getPermissionName);
        }
        #endregion

        #region 刷新

        /*
         * 尽管重置时，决定从后端获取最新数据后重新初始化表单
         * 但也可能在查看模式时重新加载数据，所以重置和刷新并不能合并
         */

        /// <summary>
        /// 是否显示刷新按钮
        /// </summary>
        protected virtual bool IsShowRefresh => !isEdit;
        /// <summary>
        /// 刷新按钮是否禁用
        /// </summary>
        protected virtual bool IsRefreshDisabled => isFormIniting ||
                                                    isReseting ||
                                                    isDeleting ||
                                                    isUpdating;
        /// <summary>
        /// 是否正在加载
        /// </summary>
        protected bool isRefreshing = false;

        /// 点击刷新按钮时回调
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task BtnRefreshClick()
        {
            await Refresh();
        }
        protected virtual async Task Refresh()
        {
            if (isRefreshing)
                return;

            isRefreshing = true;
            try
            {
                await RefreshCore();
            }
            finally
            {
                isRefreshing = false;
            }
        }
        /// <summary>
        /// 刷新核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task RefreshCore()
        {

            dto = await AppService.GetAsync(new EntityDto<long>(Id));

        }
        #endregion

        #region 重置
        ///// <summary>
        ///// 是否显示重置按钮
        ///// </summary>
        //protected virtual bool IsShowReset => IsShowUpdate;//有点多次一举，只是为了统一编程思路
        /// <summary>
        /// 正在执行重置
        /// </summary>
        protected bool isReseting = false;
        ///// <summary>
        ///// 是否禁用重置按钮
        ///// </summary>
        //protected virtual bool IsResetDisabled => isDeleting ||
        //                                          isRefreshing ||
        //                                          isFormIniting ||
        //                                          isReseting ||
        //                                          isUpdating;
        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task BtnResetClick()
        {
            await ResetCore();
        }
        /// <summary>
        /// 重置核心
        /// </summary>
        /// <returns></returns>
        protected virtual async Task ResetCore()
        {
            if (isReseting)
                return;

            isReseting = true;

            await RefreshCore();

            try
            {
                //dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
                await DtoMapToEditDto();

            }
            finally
            {
                isReseting = false;
            }
        }
        /// <summary>
        /// 显示模型转换为编辑模型，默认使用automapper
        /// </summary>
        protected virtual ValueTask DtoMapToEditDto()
        {
            editDto = ObjectMapper.Map<TEditDto>(dto);
            //editContext = new EditContext(editDto!);
            //validationMessageStore = new ValidationMessageStore(editContext);
            return ValueTask.CompletedTask;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 是否显示保存按钮和进入只读模式的按钮
        /// </summary>
        protected virtual bool IsShowUpdate => isEdit && updateIsGranted;
        /// <summary>
        /// 是否显示进入编辑模式的按钮
        /// </summary>
        protected virtual bool IsShowBeginEdit => !isEdit && updateIsGranted;
        /// <summary>
        /// true修改模式，false查看模式
        /// </summary>
        [Parameter]
        public bool IsEdit { get; set; }
        /// <summary>
        /// true修改模式，false查看模式
        /// 参考：https://learn.microsoft.com/zh-cn/aspnet/core/blazor/components/overwriting-parameters?view=aspnetcore-8.0
        /// </summary>
        protected bool isEdit;
        /// <summary>
        /// 取消编辑按钮点击时执行
        /// </summary>
        protected virtual void BtnCancelEditClick()
        {
            isEdit = false;
            //  StateHasChanged();
        }

        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //    isEdit = IsEdit;
        //}

        /// <summary>
        /// 表单是否初始化过了，通常为表单中的下拉框初始化
        /// </summary>
        protected bool editInited = false;
        /// <summary>
        /// 是否正在对表单进行首次初始化
        /// </summary>
        protected bool isFormIniting = false;
        ///// <summary>
        ///// 放弃编辑
        ///// </summary>
        //protected virtual void CancelEdit()
        //{
        //    isEdit = false;
        //    MudDialog.SetTitle($"查看{FuncName}详情");
        //}
        /// <summary>
        /// 进入编辑模式时执行
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task BtnBeginEditClick()
        {
            await BeginEditCore();
        }
        /// <summary>
        /// 进入编辑模式的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask BeginEditCore()
        {
            isEdit = true;

            // StateHasChanged();
            // MudDialog.SetTitle($"修改{FuncName}");//需要处理图标，子类自己去处理吧

            //首次进入下拉框时可能需要做些 初始化下拉框值的操作
            if (editInited)
                return;

            if (isFormIniting)
                return;

            isFormIniting = true;
            //    StateHasChanged();
            try
            {
                await InitForm();
            }
            finally
            {
                isFormIniting = false;
            }

            if (editDto == null)
                await DtoMapToEditDto();

            editInited = true;
        }


        /// <summary>
        /// 首次进入编辑模式时初始化表单，如：初始化加载下拉框数据
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask InitForm()
        {
            //this.DtoMapToEditDto();
            //editContext = new EditContext(editDto);
            //vms = new ValidationMessageStore(editContext);
            return ValueTask.CompletedTask;
        }

        /// <summary>
        /// 是否禁用保存按钮
        /// </summary>
        protected virtual bool IsUpdateDisabled => isDeleting ||
                                                   isRefreshing ||
                                                   isFormIniting ||
                                                   isReseting ||
                                                   isUpdating ||
                                                   //frm == default || 复杂编辑页面的按钮可能在body而不是footer里，所以不能判断这个
                                                   editDto == null;
        /// <summary>
        /// 是否正在保存
        /// </summary>
        protected bool isUpdating = false;
      
        /// <summary>
        /// 保存的核心逻辑
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task OnFinish(EditContext editContext)
        {
            await Update();
        }
        /// <summary>
        /// 删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Update()
        {
            if (isUpdating)
                return;

            isUpdating = true;
            try
            {
                await UpdateCore();
            }
            finally
            {
                isUpdating = false;
            }
        }
        protected virtual async Task UpdateCore()
        {

            dto = await AppService.UpdateAsync(editDto!);
            await ShowSuccessMessage("修改成功！");
            await AfterUpdated();

        }
        /// <summary>
        /// 保存后回调
        /// </summary>
        protected virtual async ValueTask AfterUpdated()
        {
            await OnUpdated.InvokeAsync(dto);
        }
        /// <summary>
        /// 保存后触发的事件
        /// </summary>
        [Parameter]
        public EventCallback<TEntityDto> OnUpdated { get; set; }
        #endregion

        #region 删除
        /// <summary>
        /// 是否显示进入编辑模式的按钮
        /// </summary>
        protected virtual bool IsShowDelete => deleteIsGranted;
        /// <summary>
        /// 是否显示删除确认
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在删除
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 点击删除按钮时执行，默认弹出确认框
        /// 通常绑定到删除按钮，它将显示删除确认，而不是真正删除
        /// </summary>
        protected virtual void BtnDeleteClick()
        {
            isShowDeleteConfirm = true;
        }
        /// <summary>
        /// 点击删除确认框的取消按钮时执行
        /// </summary>
        protected virtual void BtnCancelDelete()
        {
            isShowDeleteConfirm = false;
        }
        /// <summary>
        /// 点击删除确认按钮时执行
        /// </summary>
        /// <returns></returns>
#if !DEBUG
        [AbpExceptionInterceptor]
#endif
        protected virtual async Task BtnOkDeleteClick()
        {
            await Delete();
        }
        /// <summary>
        /// 删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Delete()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            isShowDeleteConfirm = false;
            if (isDeleting)
                return;
            isDeleting = true;
            try
            {

                await DeleteCore();
            }
            finally
            {
                isDeleting = false;
            }
        }
        protected virtual async Task DeleteCore()
        {

            var r = await AppService.DeleteAsync(new() { Ids = new[] {Id } });

            // _ = BatchOperationMessage(r);//这里木有必要await
            //BatchDeleteMessage(temp);
            if (r.Ids.Any())
            {
                await ShowSuccessMessage(msg: "删除成功！");
                await AfterDelete();
            }
            else
            {
                await ShowFailMessage(title: "删除失败！", r.ErrorMessage.FirstOrDefault()?.Message);
            }


            //await AppService.DeleteAsync(new BatchOperationInputLong { Ids = new[] { Id} });
            //_ = MessageService.Success($"删除成功！");
            //await AfterDelete();

        }
        /// <summary>
        /// 删除之后之后回调
        /// </summary>
        /// <returns></returns>
        protected virtual async ValueTask AfterDelete()
        {
            await OnDeleted.InvokeAsync(dto);
        }
        /// <summary>
        /// 删除后触发的事件
        /// </summary>
        [Parameter]
        public EventCallback<TEntityDto> OnDeleted { get; set; }
        #endregion
    }
}