
using Abp.Application.Features;
using Abp.Configuration;
using Abp.Runtime.Session;
using BXJG.Common;
using Microsoft.Extensions.DependencyInjection;
using ZLJ.RCL.Interceptors;

namespace ZLJ.RCL.Components
{
    /// <summary>
    /// 抽象组件，提供abp相关和antblazor相关功能
    /// 不跨项目、跨应用 共享
    /// </summary>
    public abstract class AbpBaseComponent : BXJG.Common.RCL.CommonBaseComponent
    {
        /// <summary>
        /// 界面消息服务
        /// </summary>
        [Inject]
        public IMessageService MessageService { get; set; }
        /// <summary>
        /// 请使用AbpSession
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
        /// <summary>
        /// 请使用SettingManager
        /// </summary>
        private ISettingManager settingManager;
        /// <summary>
        /// 设置，通常仅用于读取，若需用于修改则组件必须是server模式
        /// </summary>
        public ISettingManager SettingManager=> settingManager ??= ScopedServices.GetRequiredService<ISettingManager>();
        /// <summary>
        /// 请使用FeatureChecker
        /// </summary>
        private IFeatureChecker featureChecker;
        /// <summary>
        /// 特征检查器
        /// </summary>
        public IFeatureChecker FeatureChecker=> featureChecker??= ScopedServices.GetRequiredService<IFeatureChecker>();

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
