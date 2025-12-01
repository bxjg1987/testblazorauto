
using Abp.Application.Features;
using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Common.Events;
using BXJG.Utils.Application.Share.Session;
using Microsoft.Extensions.DependencyInjection;


namespace BXJG.Utils.RCL.Components
{
    /// <summary>
    /// 抽象组件，提供abp相关和antblazor相关功能
    /// 不跨项目、跨应用 共享
    /// </summary>
    public abstract class BaseComponent : BXJG.Common.RCL.CommonBaseComponent
    {
        [Inject]
        public Task<GetCurrentLoginInformationsOutput> CurrentLoginInformations { get; set; }
        protected GetCurrentLoginInformationsOutput currentLoginInformations { get; private set; }
        //await消息显示时，好像会等到消息因此时才结束，没严格测试
        //不过测试发现消息异步显示，并等待200毫秒，消息提示更丝滑
        //
        [Inject] public Task<AbpUserConfigurationDto> AbpUserConfiguration { get; set; }
        protected AbpUserConfigurationDto abpUserConfiguration{ get; private set; }
        /// <summary>
        /// 专门给肉夹馍aop用的，你不该调用这个
        /// </summary>
        [Inject]
        public IServiceProvider Services { get; set; }
        /// <summary>
        /// 身份验证状态，server、wasm的实现不同
        /// </summary>
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        ///// <summary>
        ///// 请使用AuthorizationService
        ///// </summary>
        //IAuthorizationService authorizationService;
        /// <summary>
        /// 授权检查服务
        /// </summary>
        [Inject]
        protected virtual IAuthorizationService AuthorizationService { get; set; }
        protected HttpClient httpClient;

        [Inject]
        protected IHttpClientFactory HttpClientFactory { get; set; }

        /// <summary>
        /// 与后端交互，它提供了常用扩展方法
        /// </summary>
        protected virtual HttpClient HttpClient => httpClient ??= BXJGHttpClientExt.DefaultFctory(HttpClientFactory);

        //[Inject]
        public ICancellationTokenProvider CancellationTokenProvider => ScopedServices.GetService<ICancellationTokenProvider>() ?? NullCancellationTokenProvider.Instance;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            currentLoginInformations = await CurrentLoginInformations;
            abpUserConfiguration = await AbpUserConfiguration;
        }
  

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
