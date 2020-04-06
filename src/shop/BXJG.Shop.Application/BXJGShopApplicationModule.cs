using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Shop.Authorization;
using System;

namespace BXJG.Shop
{
    [DependsOn(
          typeof(BXJGShopCoreModule),
          typeof(AbpAutoMapperModule))]
    public class BXJGShopApplicationModule:AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            //Configuration.Authorization.Providers.Add<BXJGShopAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(typeof(BXJGShopApplicationModule).GetAssembly())
            );
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopApplicationModule).GetAssembly());
        }
    }
}
