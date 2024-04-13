using Abp.Threading;
using BXJG.Utils.RCL;
using Microsoft.JSInterop;
using System.Net.Http;
using ZLJ.Application.Common.ClientProxy;

namespace ZLJ.Admin.CoreRCL.Share
{
    public partial class Routes
    {
        [Inject]
        public AbpUserConfigurationService abpUserCfgService { get; set; }
        [Inject]
        public SessionAppService sessionAppService { get; set; }
        [Inject]
        public AppContainer appContainer { get; set; }
        [Inject]
        public AuthenticationStateProvider AuthenticationState { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //await base.OnInitializedAsync();
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
            });


        }
        //[Inject]
        //public IJSRuntime JSRuntime { get; set; }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        //Console.WriteLine("xxxxxxxxxxxxxxxxxxxxxxx");
        //        await JSRuntime.InvokeVoidAsync("hideLoadingDiv");
        //    }
        //}
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

