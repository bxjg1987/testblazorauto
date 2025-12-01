
using Abp.Application.Services.Dto;
using Abp.ObjectMapping;
using AutoMapper;
using BXJG.Utils.Application.Share;
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
    /// 包含查看详情页和修改，以及二者之间的切换
    /// 新增抽象组件是单独定义的，因为它是对数据从无到有的创建，而详情组件是对以后的数据进行查看和处理
    /// </summary>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class DetailUpdateBaseComponent<TEntityDto,
                                                    TPrimaryKey, //保留key，避免装箱/拆箱
                                                    TUpdateInput> : BaseComponent
        where TEntityDto : new()
        where TUpdateInput : new()
    {
        #region 字段和属性

        //ui比较独立，使用更简单的库做映射

        /// <summary>
        /// 请调用ObjectMapper
        /// </summary>
        IMapper objectMapper;
        /// <summary>
        /// 对象映射接口
        /// </summary>
        protected virtual IMapper ObjectMapper => objectMapper ??= ScopedServices.GetRequiredService<IMapper>();

        /// <summary>
        /// 此功能的名称
        /// </summary>
        protected abstract string FuncName { get; }
        /// <summary>
        /// id
        /// </summary>
        [Parameter]
        public virtual TPrimaryKey Id { get; set; }
        /// <summary>
        /// 查询模型
        /// </summary>
        protected virtual TEntityDto dto { get; set; } = new TEntityDto();
        /// <summary>
        /// 当前编辑模型
        /// </summary>
        protected TUpdateInput? editDto;
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
        protected override async Task OnInitializedAsync()
        {
            //Abp.ObjectMapping.
            isEdit = IsEdit;
            //dto = await AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
            if (isEdit)
            {
                editDto = new TUpdateInput();
                //editContext = new EditContext(editDto);
                await ResetCore();
            }
            else
                await Refresh();
            //if (isEdit)
            //{
            //    await BtnBeginEditClick();
            //    // await ResetCore();
            //}
        }
        //protected virtual ValueTask<TUpdateInput> Create
        #endregion

        #region 权限


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
        protected virtual async ValueTask InitPermission(string updatePermissionName = default, string deletePermissionName = default/*, string getPermissionName =default*/, IDictionary<string, bool> others = default)
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (updatePermissionName.IsNotNullOrWhiteSpaceBXJG())
                updateIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, updatePermissionName)).Succeeded;// await PermissionChecker.IsGrantedAsync(updatePermissionName);
            if (deletePermissionName.IsNotNullOrWhiteSpaceBXJG())
                deleteIsGranted = (await AuthorizationService.AuthorizeAsync(authState.User, deletePermissionName)).Succeeded;
            //await PermissionChecker.IsGrantedAsync(deletePermissionName);
            //if (getPermissionName.IsNotNullOrWhiteSpaceBXJG())
            //  getIsGranted = await PermissionChecker.IsGrantedAsync(getPermissionName);
            if (others != default)
            {
                foreach (var item in others)
                {
                    others[item.Key] = (await AuthorizationService.AuthorizeAsync(authState.User, item.Key)).Succeeded;
                }
            }
        }
        #endregion
        /// <summary>
        /// 当前组件是否繁忙，如：正在刷新、正在提交等...
        /// 这里没用定义submiting，因为弹窗中可能有各种各样的提交，取决于具体业务
        /// </summary>
        public virtual bool IsBusy => isFormIniting ||
                                      isReseting ||
                                      isDeleting ||
                                      isUpdating ||
                                      isRefreshing;
        #region 刷新

        /*
         * 尽管重置时，决定从后端获取最新数据后重新初始化表单
         * 但也可能在查看模式时重新加载数据，所以重置和刷新并不能合并
         */

        /// <summary>
        /// 是否显示刷新按钮
        /// </summary>
        protected virtual bool IsShowRefresh => !isEdit;
        ///// <summary>
        ///// 刷新按钮是否禁用
        ///// </summary>
        //protected virtual bool IsRefreshDisabled => isFormIniting ||
        //                                            isReseting ||
        //                                            isDeleting ||
        //                                            isUpdating ||
        //                                            isRefreshing;
        /// <summary>
        /// 是否正在加载
        /// </summary>
        protected bool isRefreshing = false;

        /// 点击刷新按钮时回调
        /// </summary>
        /// <returns></returns>
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
            //if (!EqualityComparer<TPrimaryKey>.Default.Equals(Id, default))
            //{
                dto = await HttpClient.Get<TEntityDto>(new EntityDto<TPrimaryKey>(Id));//  AppService.GetAsync(new EntityDto<TPrimaryKey>(Id));
            //}
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

            try
            {
                await RefreshCore();
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
            editDto = ObjectMapper.Map<TUpdateInput>(dto);
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
        ///// <summary>
        ///// 是否禁用保存按钮
        ///// </summary>
        //protected virtual bool IsUpdateDisabled => IsBusy||
        //                                           //frm == default ||
        //                                           editDto == null;
        /// <summary>
        /// 是否正在保存
        /// </summary>
        protected bool isUpdating = false;
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

        //绑定到Finish
        ///// <summary>
        ///// 绑定到保存按钮的事件上
        ///// </summary>
        ///// <returns></returns>
        //protected virtual async Task BtnUpdateClick() {
        //    await Update();
        //}
        /// <summary>
        /// 删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Save()
        {
            if (isUpdating)
                return;

            isUpdating = true;
            try
            {
                await SaveCore();
                isUpdating = false;
                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                //_ = InvokeAsync(async () => {
                await ShowSuccessMessage("修改成功！");
                BtnCancelEditClick();
                await Task.Yield();
                await OnUpdated.InvokeAsync(dto);
                //});



            }
            finally
            {
                isUpdating = false;
            }
        }
        protected virtual async Task SaveCore()
        {
            dto = await HttpClient.Update<TEntityDto>(editDto);   //AppService.UpdateAsync(editDto!);
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
        /// 是否正在删除
        /// </summary>
        protected bool isDeleting = false;
        /// <summary>
        /// 点击删除按钮时执行，默认弹出确认框
        /// 通常绑定到删除按钮，它将显示删除确认，而不是真正删除
        /// </summary>
        protected virtual async Task BtnDeleteClick()
        {
            await Delete();
        }
        /// <summary>
        /// 删除的UI核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task Delete()
        {
            //没有权限的按钮直接隐藏，况且应用服务还会判断权限兜底的，因此这里无需判断权限
            if (isDeleting)
                return;
            isDeleting = true;
            try
            {
                await DeleteCore();
                isDeleting = false;

                //后续逻辑都是辅助性的，因此放到异步中，加快主操作速度
                _ = InvokeAsync(async () =>
                {

                    await ShowSuccessMessage("删除成功！");
                    await OnDeleted.InvokeAsync(dto);
                }).ConfigureAwait(false);

            }
            finally
            {
                isDeleting = false;
            }
        }
        /// <summary>
        /// 删除的核心逻辑
        /// </summary>
        /// <returns></returns>
        protected virtual async Task DeleteCore()
        {
            await HttpClient.Delete<TEntityDto>(new EntityDto<TPrimaryKey>(Id));// AppService.DeleteAsync(new EntityDto<TPrimaryKey>(Id));
        }
        /// <summary>
        /// 删除后触发的事件
        /// </summary>
        [Parameter]
        public EventCallback<TEntityDto> OnDeleted { get; set; }
        #endregion
    }
}