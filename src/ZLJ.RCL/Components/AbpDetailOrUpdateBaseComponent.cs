
using Abp.ObjectMapping;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;
namespace ZLJ.RCL.Components
{
    /*
     * 由于abp的crud接口和抽象类把crud搞一起了，不想动它，所以这里的应用服务中包含TGetAllInput和TCreateInput
     * 不要考虑同一个组件反复编辑不同的数据，那样稍微复杂了点，是特殊情况，就具体项目中特殊处理，暂时不考虑放抽象类中
     * 编辑时可能要加载些下拉框数据，这通常比较耗时，若在初始化组件时就加载，在用户仅仅查看时就比较浪费，所以在首次进入编辑模式时再做这些操作。
     * 
     * blazor文档中推荐不要在组件内部修改 [Parameter]属性，这种属性仅仅用于外部组件向其传递参数用，可能由于外部组件的刷新，导致此组件状态异常
     * 
     * 虽然有些简单的数据列表页可能直接传递dto过来进行处理，但有时候列表数据过于复杂，需要重新查询下，简单起见统一为根据id重新查询。
     */

    /// <summary>
    /// 基于antblazor和abp的通用详情页组件，它包含查看详情页和修改，以及二者之间的切换
    /// 新增抽象组件是单独定义的，因为它是对数据从无到有的创建，而详情组件是对以后的数据进行查看和处理
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpDetailOrUpdateBaseComponent<TAppService,
                                                         TEntityDto,
                                                         TPrimaryKey,
                                                         TGetAllInput,
                                                         TCreateInput,
                                                         TUpdateInput> : AbpBaseComponent
        where TEntityDto : IEntityDto<TPrimaryKey>
        //where TGetAllInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ZLJ.Application.Common.Share.ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
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
        public TPrimaryKey Id { get; set; }
        /// <summary>
        /// 查询模型
        /// </summary>
        protected TEntityDto? dto;
        /// <summary>
        /// 当前编辑模型
        /// </summary>
        protected TUpdateInput? editDto;
        /// <summary>
        /// 编辑上下文
        /// </summary>
        protected EditContext? editContext;
        /// <summary>
        /// 验证消息存储器
        /// </summary>
        protected ValidationMessageStore? validationMessageStore;


        /// <summary>
        /// 正在执行重置
        /// </summary>
        protected bool isReseting = false;
        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async ValueTask BtnResetClick()
        {
            isReseting = true;
            try
            {
                await DtoMapToEditDto();
                editContext = new EditContext(editDto!);
                validationMessageStore = new ValidationMessageStore(editContext);
            }
            finally
            {
                isReseting = false;
            }
        }


        /// <summary>
        /// 初始化时回调，默认根据id从应用服务接口获取单个数据，然后判断并进入编辑模式
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
            isEdit = IsEdit;



            if (isEdit)
            {
                await BtnBeginEditClick();
                // await ResetCore();
            }
        }

        /// <summary>
        /// 显示模型转换为编辑模型，默认使用automapper
        /// </summary>
        protected virtual ValueTask DtoMapToEditDto()
        {
            editDto = ObjectMapper.Map<TUpdateInput>(dto);
            return ValueTask.CompletedTask;
        }

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
        /// <summary>
        /// 初始化权限状态
        /// </summary>
        /// <param name="updatePermissionName"></param>
        /// <param name="deletePermissionName"></param>
        /// <returns></returns>
        protected virtual async Task InitPermission(string updatePermissionName = default, string deletePermissionName = default/*, string getPermissionName =default*/)
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

        /// <summary>
        /// true修改模式，false查看模式
        /// </summary>
        [Parameter]
        public bool IsEdit { get; set; }
        /// <summary>
        /// true修改模式，false查看模式
        /// 参考：https://learn.microsoft.com/zh-cn/aspnet/core/blazor/components/overwriting-parameters?view=aspnetcore-6.0
        /// </summary>
        protected bool isEdit;
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //    await base.SetParametersAsync(parameters);

        //}

        /// <summary>
        /// 取消编辑按钮点击时执行
        /// </summary>
        protected virtual void BtnEndEditClick()
        {
            isEdit = false;
            //  StateHasChanged();
        }

        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //    isEdit = IsEdit;
        //}
        #region 修改
        /// <summary>
        /// 表单是否初始化过了
        /// </summary>
        protected bool editInited = false;

        /// <summary>
        /// 进入编辑模式时执行
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnBeginEditClick()
        {
            isEdit = true;
            // StateHasChanged();
            // MudDialog.SetTitle($"修改{FuncName}");//需要处理图标，子类自己去处理吧

            //首次进入下拉框时可能需要做些 初始化下拉框值的操作
            if (editInited)
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
                await BtnResetClick();

            editInited = true;
        }

        /// <summary>
        /// 是否正在对表单进行首次初始化
        /// </summary>
        protected bool isFormIniting = false;
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
        ///// <summary>
        ///// 放弃编辑
        ///// </summary>
        //protected virtual void CancelEdit()
        //{
        //    isEdit = false;
        //    MudDialog.SetTitle($"查看{FuncName}详情");
        //}
        /// <summary>
        /// 是否显示保存按钮和进入只读模式的按钮
        /// </summary>
        protected virtual bool IsShowSave => isEdit && updateIsGranted;
        /// <summary>
        /// 是否显示进入编辑模式的按钮
        /// </summary>
        protected virtual bool IsShowBeginEdit => !isEdit && updateIsGranted;

        /// <summary>
        /// 是否禁用保存按钮
        /// </summary>
        public virtual bool IsSaveDisabled => isDeleting ||
                                              isFormIniting ||
                                              isReseting ||
                                              isSaving ||
                                              editDto == null ||
                                              editContext == default || editContext.GetValidationMessages().Any();
        /// <summary>
        /// 是否正在保存
        /// </summary>
        protected bool isSaving = false;

        /// <summary>
        /// 保存的核心逻辑
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnSaveClick()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限

            if (!editContext!.Validate())
                return;

            isSaving = true;
            try
            {
                dto = await AppService.UpdateAsync(editDto!);
                _ = base.MessageService.Success("修改成功！");
                await AfterSave();
            }
            finally
            {
                isSaving = false;
            }
        }
        /// <summary>
        /// 保存后回调
        /// </summary>
        protected virtual ValueTask AfterSave() => ValueTask.CompletedTask;
        #endregion
        #region 删除
        /// <summary>
        /// 是否显示删除确认
        /// </summary>
        protected bool isShowDeleteConfirm = false;
        /// <summary>
        /// 是否正在删除
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 显示删除确认框
        /// 通常绑定到删除按钮，它将显示删除确认，而不是真正删除
        /// </summary>
        protected virtual void BtnDeleteClick()
        {
            isShowDeleteConfirm = true;
        }
        /// <summary>
        /// 隐藏删除确认框
        /// </summary>
        protected virtual void BtnCancelDelete()
        {
            isShowDeleteConfirm = false;
        }

        /// <summary>
        /// 删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        [AbpExceptionInterceptor]
        protected virtual async Task BtnOkDeleteClick()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            isShowDeleteConfirm = false;
            isDeleting = true;
            try
            {
                await AppService.DeleteAsync(new EntityDto<TPrimaryKey>(Id));
                _ = MessageService.Success($"删除成功！");
                await AfterDelete();
            }
            finally
            {
                isDeleting = false;
            }
        }
        /// <summary>
        /// 删除之后之后回调
        /// </summary>
        /// <returns></returns>
        protected virtual ValueTask AfterDelete() => ValueTask.CompletedTask;
        #endregion
    }
}