using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.GeneralTree;
using BXJG.Utils;
using System;
using Abp.Dependency;
using Abp.Zero.Configuration;
using Abp.MultiTenancy;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using System.Collections.Generic;

namespace BXJG.WorkOrder
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class CoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            LocalizationConfigurer.Configure(Configuration.Localization);
            //Configuration.Settings.Providers.Add<AppSettingProvider>();
            //Configuration.Modules.Zero().RoleManagement.StaticRoles.Add(new StaticRoleDefinition(CoreConsts.CustomerRoleName, MultiTenancySides.Tenant));
            //Configuration.Modules.BXJGUtils().AddEnum("bxjgShopOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);

            //Configuration.DynamicEntityProperties.Providers.Add<ProductDynamicEntityPropertyDefinition>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CoreModule).GetAssembly());

            //注册顾客session，确保一次请求一个顾客session实例
            //参考https://github.com/aspnetboilerplate/aspnetboilerplate/issues/3945
            //IocManager.Register<ICustomerSession, CustomerSession>(DependencyLifeStyle.Transient);
            //目前使用的这种方式，后期改为基于claim的形式，参考：CustomerClaimSession
            //可以考虑将则个逻辑抽离成扩展方法，CustomerClaimSession以单例注册的逻辑也可以放在扩展方法中，方便切换session实现方式

            //CustomerClaimSession的父类已单例注册了
            //IocManager.IocContainer.Register(Component.For<ICustomerSession>()
            //                                          .ImplementedBy<CustomerClaimSession>()
            //                                          .LifestyleCustom<MsScopedLifestyleManager>()
            //                                          .Named("sdf234sdf"));

            //注册顾客登陆器
            //IocManager.Register(typeof(CustomerLoginManager<,,,>), DependencyLifeStyle.Transient);

            //这样不行，因为使用时ICustomerLoginManager<User>，而具体实现有4个泛型，所以需要在主程序是注册ios 或者直接使用上面的
            //IocManager.Register(typeof(ICustomerLoginManager<>), typeof(CustomerLoginManager<,,,>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(ItemManager<>), DependencyLifeStyle.Transient);
        }
    }
}
