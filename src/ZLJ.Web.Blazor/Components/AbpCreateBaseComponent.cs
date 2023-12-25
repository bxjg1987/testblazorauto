

namespace ZLJ.Web.Blazor.Components
{
    /// <summary>
    /// 基于BootstrapBlazor和abp的通用新增页组件
    /// 查看详情和修改数据的抽象组件是单独定义的（因为要切换查看和编辑模式，所以定义在同一个组件中的），
    /// 查看详情和修改组件是对以后的数据进行查看和处理，而新增组件它是对数据从无到有的创建，因此分开定义的。
    /// </summary>
    /// <typeparam name="TAppService">应用服务类型</typeparam>
    /// <typeparam name="TEntityDto">列表项的数据类型</typeparam>
    /// <typeparam name="TPrimaryKey">唯一id类型</typeparam>
    /// <typeparam name="TGetAllInput">获取列表时的输入参数类型</typeparam>
    /// <typeparam name="TCreateInput">新增时的输入类型</typeparam>
    /// <typeparam name="TUpdateInput">修改时的输入类型</typeparam>
    public abstract class AbpCreateBaseComponent<TAppService,
                                                 TEntityDto,
                                                 TPrimaryKey,
                                                 TGetAllInput,
                                                 TCreateInput,
                                                 TUpdateInput> : BXJG.Utils.Components.AbpCreateBaseComponent<TAppService,
                                                                                                         TEntityDto,
                                                                                                         TPrimaryKey,
                                                                                                         TGetAllInput,
                                                                                                         TCreateInput,
                                                                                                         TUpdateInput>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : new()
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TAppService : ICrudBaseAppService<TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 对表单的引用
        /// </summary>
        protected Form<TCreateInput> validateForm;
        [AbpExceptionInterceptor]
        public override async Task Reset()
        {
            await base.Reset();
            StateHasChanged();
        }
        [AbpExceptionInterceptor]
        public override async Task<SaveResult> Save()
        {
            return await base.Save();
        }
        protected override ValueTask<bool> Validate()
        {
            return ValueTask.FromResult(validateForm.Validate());
        }
        [Inject]
        public IMessageService MessageService { get; set; }

        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Error(msg);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Success(msg);
        }

        #region 生命周期方法增加统一异常处理拦截器
        [AbpExceptionInterceptor]
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }
        [AbpExceptionInterceptor]
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        [AbpExceptionInterceptor]
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
        [AbpExceptionInterceptor]
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        [AbpExceptionInterceptor]
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }
        #endregion

    }
}