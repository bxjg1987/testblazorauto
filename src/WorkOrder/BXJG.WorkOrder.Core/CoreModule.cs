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
using System.Reflection;
using BXJG.WorkOrder.WorkOrder;
using Abp.Localization;
using BXJG.WorkOrder.WorkOrderType;
using Microsoft.Extensions.DependencyInjection;
namespace BXJG.WorkOrder
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class CoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            LocalizationConfigurer.Configure(Configuration.Localization);
            IocManager.Register<BXJGWorkOrderConfig>();
          
            //Configuration.Settings.Providers.Add<AppSettingProvider>();
            //Configuration.Modules.BXJGUtils().AddEnum("bxjgShopOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);
            //Configuration.DynamicEntityProperties.Providers.Add<ProductDynamicEntityPropertyDefinition>();
            //Configuration
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            var workOrderTypeProviders = IocManager.ResolveAll<IWorkOrderTypeProvider>();
            var ctx = new WorkOrderTypeProviderContext();
            foreach (var item in workOrderTypeProviders)
            {
                item.Create(ctx);
            }
            var manager = new WorkOrderTypeManager(ctx.Defines);

            IocManager.RegService(s => s.AddSingleton(manager));
         
        }
    }
}
