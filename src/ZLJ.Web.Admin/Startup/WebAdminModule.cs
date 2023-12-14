using Abp.Modules;
using BXJG.Utils;
using Rougamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ZLJ.App.Admin;
using ZLJ.Web.Admin.Startup;

//[assembly: ExceptionInterceptor]
//全局异常处理拦截器
//[assembly: AbpBBException]


namespace ZLJ.Web.Admin.Startup
{
    //  [IgnoreMo(MoTypes = new[] { typeof(ExceptionInterceptorAttribute) })]
    [DependsOn(typeof(ZLJWebCoreModule), typeof(AbpBlazorModule), typeof(ZLJApplicationModule))]
    public class WebAdminModule : AbpModule
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
