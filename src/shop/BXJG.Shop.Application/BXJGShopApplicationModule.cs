using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Shop.Authorization;
using System;
using System.Reflection;

namespace BXJG.Shop
{
    [DependsOn(
          typeof(BXJGShopCoreModule),
          typeof(AbpAutoMapperModule))]
    public class BXJGShopApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            //Configuration.Authorization.Providers.Add<BXJGShopAuthorizationProvider>();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopApplicationModule).GetAssembly());


        }
    }
}
