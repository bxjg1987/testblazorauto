using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Shop.Authorization;
using BXJG.Shop.Configuration;
using System;

namespace BXJG.Shop
{
    public class BXJGShopCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGShopLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGShopAppSettingProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopCoreModule).GetAssembly());
        }
    }
}
