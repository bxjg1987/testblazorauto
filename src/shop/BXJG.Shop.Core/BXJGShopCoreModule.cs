using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Configuration;
using BXJG.Shop.Localization;
using BXJG.Shop.Sale;
using BXJG.Utils;
using System;

namespace BXJG.Shop
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class BXJGShopCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGShopLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGShopAppSettingProvider>();
            //Configuration.Modules.BXJGUtils().AddEnum("bxjgShopOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopCoreModule).GetAssembly());
        }
    }
}
