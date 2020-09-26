using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.GeneralTree;
using BXJG.Shop.Authorization;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Configuration;
using BXJG.Shop.Localization;
using BXJG.Shop.Sale;
using BXJG.Utils;
using System;
using Abp.Dependency;
using Abp.Zero.Configuration;
using Abp.MultiTenancy;

namespace BXJG.Shop
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class BXJGShopCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGShopLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGShopAppSettingProvider>();
            Configuration.Modules.Zero().RoleManagement.StaticRoles.Add(new StaticRoleDefinition(BXJGShopConsts.CustomerRoleName, MultiTenancySides.Tenant));
            //Configuration.Modules.BXJGUtils().AddEnum("bxjgShopOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopCoreModule).GetAssembly());

            //IocManager.Register(typeof(ItemManager<>), DependencyLifeStyle.Transient);
            
            
        }
    }
}
