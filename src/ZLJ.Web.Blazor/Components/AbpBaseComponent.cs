
using Abp.Configuration;
using Abp.Runtime.Session;
using BXJG.Common;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.Web.Blazor.Interceptors;

namespace ZLJ.Web.Blazor.Components
{
    /// <summary>
    /// 与Abp和BB有关的抽象组件（注意它与crud抽象组件是平级的）
    /// </summary>
    public abstract class AbpBaseComponent : BXJG.Common.RCL.CommonBaseComponent
    //where TUser : AbpUser<TUser>
    //where TRole : AbpRole<TUser>, new()
    //where TUserManager : AbpUserManager<TRole, TUser>
    {
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }
        /// <summary>
        /// 请使用MessageService
        /// </summary>
        private IAbpSession abpSession;
        /// <summary>
        /// 获取当前session
        /// </summary>
        protected virtual IAbpSession AbpSession => abpSession ??= ScopedServices.GetRequiredService<IAbpSession>();
        /// <summary>
        /// 界面事件总线提供器
        /// </summary>
        [Inject]
        protected virtual IZhongjieProvider ZhongjieProvider { get; private set; }

        private ISettingManager settingManager;

        public ISettingManager SettingManager=> settingManager ??= ScopedServices.GetRequiredService<ISettingManager>();

        protected override async ValueTask ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        {
            await MessageService.Error(msg);
        }
        protected override async ValueTask ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        {
            await MessageService.Success(msg);
        }
     
        
        #region 生命周期方法增加统一异常处理拦截器
        //肉夹馍不会拦截子类，但往往子类才是主要逻辑，才是出错的地方，所以父类加这个意义不大
        //[AbpExceptionInterceptor]
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //   await base.SetParametersAsync(parameters);
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnParametersSet()
        //{
        //    base.OnParametersSet();
        //}
        //[AbpExceptionInterceptor]
        //protected override async Task OnParametersSetAsync()
        //{
        //    await base.OnParametersSetAsync();
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();
        //}
        //[AbpExceptionInterceptor]
        //protected override async Task OnInitializedAsync()
        //{
        //    await base.OnInitializedAsync();
        //}
        //[AbpExceptionInterceptor]
        //protected override void OnAfterRender(bool firstRender)
        //{
        //    base.OnAfterRender(firstRender);
        //}
        //[AbpExceptionInterceptor]
        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    return base.OnAfterRenderAsync(firstRender);
        //}
        #endregion
    }
}
