using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.CMS.Authorization;
using BXJG.CMS.Localization;
using BXJG.GeneralTree;
//using BXJG.CMS.Configuration;
using System;

namespace BXJG.CMS
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class BXJGCMSCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGCMSLocalizationConfigurer.Configure(Configuration.Localization);
            //Configuration.Settings.Providers.Add<BXJGCMSAppSettingProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGCMSCoreModule).GetAssembly());
        }
    }
}
