
using Abp.Application.Features;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.Common.Events;
using Microsoft.Extensions.DependencyInjection;


namespace BXJG.Utils.RCL.Components
{
    /// <summary>
    /// 抽象组件，提供abp相关和antblazor相关功能
    /// 不跨项目、跨应用 共享
    /// </summary>
    public abstract class BaseComponent : BXJG.Common.RCL.CommonBaseComponent
    {
        //[Inject]
        public ICancellationTokenProvider CancellationTokenProvider => ScopedServices.GetService<ICancellationTokenProvider>() ?? NullCancellationTokenProvider.Instance;
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
        public ISettingManager SettingManager => settingManager ??= ScopedServices.GetRequiredService<ISettingManager>();
        /// <summary>
        /// 请使用FeatureChecker
        /// </summary>
        private IFeatureChecker featureChecker;
        /// <summary>
        /// 特征检查器
        /// </summary>
        public IFeatureChecker FeatureChecker => featureChecker ??= ScopedServices.GetRequiredService<IFeatureChecker>();

        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //

        //protected override async Task ShowFailMessage(string title = "操作提示", string msg = "操作失败！")
        //{
        //    _= MessageService.Error(msg);
        //    await Task.Delay(200);
        //}
        //protected override async Task ShowSuccessMessage(string title = "操作提示", string msg = "操作成功！")
        //{
        //    _ = MessageService.Error(msg);
        //    await Task.Delay(200);
        //}

    }
}
