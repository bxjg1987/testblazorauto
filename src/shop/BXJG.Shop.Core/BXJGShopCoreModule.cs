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
using BXJG.Shop.Customer;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using System.Collections.Generic;

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

            //注册顾客session，确保一次请求一个顾客session实例
            //参考https://github.com/aspnetboilerplate/aspnetboilerplate/issues/3945
            //IocManager.Register<ICustomerSession, CustomerSession>(DependencyLifeStyle.Transient);
            //目前使用的这种方式，后期改为基于claim的形式，参考：CustomerClaimSession
            //可以考虑将则个逻辑抽离成扩展方法，CustomerClaimSession以单例注册的逻辑也可以放在扩展方法中，方便切换session实现方式
            IocManager.IocContainer.Register(
                Component
                    .For<ICustomerSession>()
                    .ImplementedBy<CustomerClaimSession>()
                    .LifestyleCustom<MsScopedLifestyleManager>()
            );

            //注册顾客登陆器
            IocManager.Register(typeof(CustomerLoginManager<,,,>), DependencyLifeStyle.Transient);

            //IocManager.Register(typeof(ItemManager<>), DependencyLifeStyle.Transient);
        }
    }
}
