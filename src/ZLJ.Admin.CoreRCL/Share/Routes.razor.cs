using Abp.Application.Navigation;
using Abp.Notifications;
using Abp.Threading;
using BXJG.Common.Events;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.CoreRCL.Share
{
    public partial class Routes : IAsyncDisposable
    {
        [Inject]
        public CommonConnection Connection { get; set; }
        [Inject]
        public Zhongjie Zhongjie { get; set; }
        [Inject]
        public AbpUserConfigurationService abpUserCfgService { get; set; }
        [Inject]
        public SessionAppService sessionAppService { get; set; }
        [Inject]
        public AppContainer appContainer { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationState { get; set; }
        [Inject]
        public ILogger<Routes> Logger { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Console.WriteLine("blazor路由中的appcontainer："+appContainer.GetHashCode());
            var r = await AuthenticationState.GetAuthenticationStateAsync();
            if (r.User.Identity.IsAuthenticated)
            {
                appContainer.T1 = sessionAppService.GetCurrentLoginInformations().ContinueWith(t =>
                {
                    appContainer.CurrentLoginInformations = t.Result;
                });
            }

            //await Console.Out.WriteLineAsync("路由中的初始化执行了，正在初始化appContainer"+ appContainer.GetHashCode());
            appContainer.T2 = abpUserCfgService.GetAll().ContinueWith(t =>
            {
                appContainer.AbpUserConfiguration = t.Result;

                //appContainer.AbpUserConfiguration.Nav.Menus.ForEach(x =>
                //{

                //    x.Value.Items.RecursionDown((a, b) =>
                //    {
                //        b.Items = b.Items.OrderBy(m => m.Order).ToList();


                //        return true;
                //    });

                //    x.Value.Items = x.Value.Items.OrderBy(c => c.Order).ToList();
                //});
            });
            this.Logger.LogDebug($"路由中的init执行，准备连接后端signalR...");
            await Connection.ExecuteAsync();

            this.Zhongjie.Zhuce<TenantNotification>(x =>
            {
                var msg = "";
                if (x.Data is MessageNotificationData a)
                    msg = a.Message;
                else if (x.Data is LocalizableMessageNotificationData b)
                    msg = b.Message.ToString();//这里需要前端做本地化处理
                else
                { 
                    //其它消息类型
                }



                switch (x.Severity)
                {
                    case NotificationSeverity.Info:
                        MessageService.Info(msg);
                        break;
                    case NotificationSeverity.Success:
                        MessageService.Success(msg);
                        break;
                    case NotificationSeverity.Warn:
                        MessageService.Warning(msg);
                        break;
                    case NotificationSeverity.Fatal:
                    case NotificationSeverity.Error:
                        MessageService.Error(msg);
                        break;
                }
                //其它类型的实时消息通知 这里继续判断处理
                return ValueTask.CompletedTask;
            }, nameof(MessageNotificationData));
        }
        [Inject]
        public IMessageService MessageService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("hideLoadingDiv");
        }

        public ValueTask DisposeAsync()
        {
            Zhongjie.Zhuxiao();
            return ValueTask.CompletedTask;
        }
    }
    //protected override async Task onin
    //{
    //    base.OnInitialized();
    //    //     var ssd = HttpContext.User.Identity.IsAuthenticated;
    //    // if (OperatingSystem.IsBrowser())
    //    // {
    //    //var abpUserCfgService = ScopedServices.GetRequiredService<AbpUserConfigurationService>();
    //    //var sessionAppService = ScopedServices.GetRequiredService<SessionAppService>();
    //    //var appContainer = ScopedServices.GetRequiredService<AppContainer>();
    //    var t1 = abpUserCfgService.GetAll().ContinueWith(r =>
    //    {
    //        appContainer.AbpUserConfiguration = r.Result;
    //    });
    //    var sdff =  

    //    appContainer.CurrentLoginInformations = sessionAppService.GetCurrentLoginInformations();
    //    // Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto
    //    // }
    //    // appContainer.AbpUserConfiguration = await abpUserCfgService.GetAll();
    //    //  Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(appContainer));
    //}

    // protected override void o
    // {
    ////     var ssd = HttpContext.User.Identity.IsAuthenticated;
    //     // if (OperatingSystem.IsBrowser())
    //     // {
    //     //var abpUserCfgService = ScopedServices.GetRequiredService<AbpUserConfigurationService>();
    //     //var sessionAppService = ScopedServices.GetRequiredService<SessionAppService>();
    //     //var appContainer = ScopedServices.GetRequiredService<AppContainer>();
    //     var t1 = abpUserCfgService.GetAll().ContinueWith(r =>
    //     {
    //         appContainer.AbpUserConfiguration = r.Result;
    //     });


    //     appContainer.CurrentLoginInformations = sessionAppService.GetCurrentLoginInformations();
    //     // Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto
    //     // }
    //     // appContainer.AbpUserConfiguration = await abpUserCfgService.GetAll();
    //     //  Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(appContainer));
    // }
}

