using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Organizations;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using BXJG.Common.Contracts;
using BXJG.Utils;
using BXJG.Utils.Enums;
using BXJG.Utils.Interceptor;
using BXJG.Utils.Localization;
using BXJG.Utils.OU;
using BXJG.Utils.Settings;
using System.Reflection;

namespace BXJG.PSI
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class PSICoreModule : AbpModule
    {
        public override void PreInitialize()
        {
         
            IocManager.Register<PSIModuleConfig>();
          

            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

            PSILocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<PSISettingProvider>();

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
