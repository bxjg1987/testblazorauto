using Abp.Application.Navigation;
using Abp.Notifications;
using Abp.Threading;
using AntDesign;
using BXJG.Common.Events;
using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.RCL;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.CoreRCL.Share
{
    public partial class Routes : IAsyncDisposable
    {
        //[Inject]
        //public CommonConnection Connection { get; set; }
        [Inject]
        public Zhongjie Zhongjie { get; set; }
        //[Inject]
        //public AbpUserConfigurationService abpUserCfgService { get; set; }
        //[Inject]
        //public SessionAppService sessionAppService { get; set; }


        //[Inject]
        //public AppContainer AppContainer { get; set; }

        [Inject(Key = Consts.TongyongLianjie)]
        public HubConnection HubConnection { get; set; }

        //[Inject]
        //public AuthenticationStateProvider AuthenticationState { get; set; }
        [Inject]
        public ILogger<Routes> Logger { get; set; }
        //public override async Task SetParametersAsync(ParameterView parameters)
        //{
        //    await base.SetParametersAsync(parameters);
        //    AppContainer.AbpUserConfiguration.Localization.Values = new Dictionary<string, Dictionary<string, string>>();
        //    foreach (var item in Bendihua)
        //    {
        //        AppContainer.AbpUserConfiguration.Localization.Values.Add(item.Key, item.Items);
        //    }
        //}
        IDisposable qjxxsj;
        protected override void OnInitialized()
        {
            base.OnInitialized();

            #region 全局消息
            // this.Logger.LogDebug($"路由中的init执行，准备连接后端signalR...");

            qjxxsj = Zhongjie.Zhuce<TenantNotification>(x =>
            {
                object msg = null, title = null;
                //if (x.Data is MessageNotificationData a)
                //    msg = a.Message;
                //else if (x.Data is LocalizableMessageNotificationData b)
                //    msg = b.Message.ToString();//这里需要前端做本地化处理
                //else
                //{
                //    //其它消息类型
                //}
                x.Data.Properties.TryGetValue("Message", out msg);
                x.Data.Properties.TryGetValue("Title", out title);

                var nf = new NotificationConfig
                {
                    Message = title?.ToString(),
                    Description = msg?.ToString(),
                    NotificationType = NotificationType.None
                };
                switch (x.Severity)
                {
                    case NotificationSeverity.Info:
                        nf.NotificationType = NotificationType.Info;
                        break;
                    case NotificationSeverity.Success:
                        nf.NotificationType = NotificationType.Success;
                        break;
                    case NotificationSeverity.Warn:
                        nf.NotificationType = NotificationType.Warning;
                        break;
                    case NotificationSeverity.Fatal:
                    case NotificationSeverity.Error:
                        nf.NotificationType = NotificationType.Error;
                        break;
                }
                this.MessageService.Open(nf);
                //其它类型的实时消息通知 这里继续判断处理
                return ValueTask.CompletedTask;
            });
            #endregion

            //  Logger.LogDebug("blazor路由中的appcontainer："+AppContainer.GetHashCode());

            //   AppContainer.DeepCloneTo(App); 
            // Logger.LogDebug("appcontainer：" + AppContainer.GetHashCode());

            //var r = await AuthenticationState.GetAuthenticationStateAsync();
            //if (r.User?.Identity != default && r.User.Identity.IsAuthenticated)
            //    AppContainer.CurrentLoginInformations = sessionAppService.GetCurrentLoginInformations();

            //AppContainer.CurrentLoginInformations = AuthenticationState.GetAuthenticationStateAsync().ContinueWith(async t => {
            //    var r = t.Result;
            //    if (r.User?.Identity != default && r.User.Identity.IsAuthenticated)
            //        return await sessionAppService.GetCurrentLoginInformations();
            //    return await Task.FromResult<GetCurrentLoginInformationsOutput>(null);
            //}).Unwrap();

            //var r = await AuthenticationState.GetAuthenticationStateAsync();
            //if (r.User?.Identity != default && r.User.Identity.IsAuthenticated)
            //{
            //    ts.Add(sessionAppService.GetCurrentLoginInformations().ContinueWith(t =>
            //    {
            //        AppContainer.CurrentLoginInformations = t.Result;
            //    }));

            //    // AppContainer.CurrentLoginInformations = await sessionAppService.GetCurrentLoginInformations();
            //    //appContainer.T1 = sessionAppService.GetCurrentLoginInformations().ContinueWith(t =>
            //    //{
            //    //    appContainer.CurrentLoginInformations = t.Result;
            //    //});
            //}
            //AppContainer.AbpUserConfiguration = abpUserCfgService.GetAll();

        }
        protected override async Task OnInitializedAsync()
        {
            //Logger.LogWarning($"连接前，id：{HubConnection?.ConnectionId}");
            //Console.WriteLine($"连接前2，id：{HubConnection?.ConnectionId}");
            if (RendererInfo.IsInteractive)
                await HubConnection.StartAsync();

            ////await Console.Out.WriteLineAsync("路由中的初始化执行了，正在初始化appContainer"+ appContainer.GetHashCode());
            //ts.Add(abpUserCfgService.GetAll().ContinueWith(async t =>
            // {
            //    // await Task.Delay(3000);
            //     AppContainer.AbpUserConfiguration = t.Result;
            // }).Unwrap());
            //记得Unwrap
            // await Task.WhenAll(ts);
            //   await InvokeAsync(  StateHasChanged);//没用
            //     StateHasChanged();//没用
            //     await Zhongjie.Chufa("appContainerInited");
            // StateHasChanged();//测试了，这个没用
            // AppContainer.AbpUserConfiguration = await  abpUserCfgService.GetAll();



            //     StateHasChanged();  
            //await Task.Delay(5000);
            //appContainer.AbpUserConfiguration.Nav.Menus.ForEach(x =>
            //{

            //    x.Value.Items.RecursionDown((a, b) =>
            //    {
            //        b.Items = b.Items.OrderBy(m => m.Order).ToList();


            //        return true;
            //    });

            //    x.Value.Items = x.Value.Items.OrderBy(c => c.Order).ToList();
            //});


        }
        [Inject]
        public INotificationService MessageService { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("hideLoadingDiv");
        }

        public ValueTask DisposeAsync()
        {
            // Zhongjie.Zhuxiao();
            qjxxsj?.Dispose();
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

