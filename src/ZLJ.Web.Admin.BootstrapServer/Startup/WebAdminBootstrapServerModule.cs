using Abp.Modules;
using BXJG.AbpBootstrapBlazor;
using BXJG.Utils;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using ZLJ.App.Admin;

//[assembly: ExceptionInterceptor]



namespace ZLJ.Web.Admin.BootstrapServer.Startup
{
    //  [IgnoreMo(MoTypes = new[] { typeof(ExceptionInterceptorAttribute) })]
    [DependsOn(typeof(ZLJWebCoreModule), typeof(ZLJApplicationModule))]
    public class WebAdminBootstrapServerModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<AdminNavigationProvider>();
        }
        //builder.Services.AddMudServices();
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            //IocManager.RegService(services => {

            //    //这里的注册与配置都说全局的
            //    //应在主程序中配置，各app有自己独立的配置
            //    services.AddMudServices(config => {
            //        config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            //    });
            //});
        }
    }
}
