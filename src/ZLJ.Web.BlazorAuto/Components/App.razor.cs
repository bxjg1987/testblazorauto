using BXJG.Utils.RCL;
using Force.DeepCloner;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ZLJ.Application.Common.ClientProxy;
using ZLJ.RCL;
namespace ZLJ.Web.BlazorAuto.Components
{
    public partial class App
    {
        [CascadingParameter]
        private HttpContext HttpContext { get; set; } = default!;
       
        // bool isAjax => HttpContext.Request.IsAjaxRequestBXJG();

  

      //  AppContainer app=new AppContainer();
        [Inject]
        public ILogger<App> Logger { get; set; }
     
        //protected override void OnInitialized()
        //{
        //    // app = Test;
        //    app.CurrentLoginInformations = Test.CurrentLoginInformations.DeepClone();
        //    app.AbpUserConfiguration = new Abp.Web.Models.AbpUserConfiguration.AbpUserConfigurationDto();
        //    app.AbpUserConfiguration.Custom= Test.AbpUserConfiguration.Custom.DeepClone();
        //    app.AbpUserConfiguration.MultiTenancy = Test.AbpUserConfiguration.MultiTenancy.DeepClone();

        //    app.AbpUserConfiguration.Session = Test.AbpUserConfiguration.Session.DeepClone();


        //       app.AbpUserConfiguration.Localization = Test.AbpUserConfiguration.Localization.DeepClone();

        //通过参数传递到路由时，这个本地化的Values会导致报错
        //    //app.AbpUserConfiguration.Localization.Values = default;


        //    app.AbpUserConfiguration.Features = Test.AbpUserConfiguration.Features.DeepClone();
        //    app.AbpUserConfiguration.Auth = Test.AbpUserConfiguration.Auth.DeepClone();

        //    app.AbpUserConfiguration.Nav = Test.AbpUserConfiguration.Nav.DeepClone();
        //    app.AbpUserConfiguration.Setting = Test.AbpUserConfiguration.Setting.DeepClone();
        //    app.AbpUserConfiguration.Clock = Test.AbpUserConfiguration.Clock.DeepClone();
        //    app.AbpUserConfiguration.Timing = Test.AbpUserConfiguration.Timing.DeepClone();
        //    app.AbpUserConfiguration.Security = Test.AbpUserConfiguration.Security.DeepClone();


        //    // Test.AbpUserConfiguration.DeepCloneTo(app.AbpUserConfiguration);
        //    this.Logger.LogDebug($"服务端最外层的app组件执行了,appcontainer：{Test.GetHashCode()}");
        //    base.OnInitialized();
        //}
        private IComponentRenderMode? RenderModeForPage 
        {
            get {

                if (HttpContext.Request.Path.StartsWithSegments("/Account"))
                    return default;
                if (HttpContext.Request.Path.StartsWithSegments("/404"))
                    return default;

                if (HttpContext.Request.Path.StartsWithSegments("/error"))
                    return default;

                //  return new InteractiveAutoRenderMode(false);
                return new InteractiveServerRenderMode(false);
                // return new InteractiveWebAssemblyRenderMode(false);
            }
        } 
    }
}
