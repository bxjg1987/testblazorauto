using Abp.Modules;
using BXJG.AbpMudBlazor;
using BXJG.Utils;
using BXJG.Utils;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Customer;

namespace ZLJ.Web.Customer
{
    [DependsOn(typeof(ZLJWebCoreModule),typeof(CustomerApplicationModule), typeof(BXJGMudBlazorModule))]
    public class WebCustomerModule:AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<CustNavigationProvider>();
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
