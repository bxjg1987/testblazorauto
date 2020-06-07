using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.CMS.Authorization;
using BXJG.GeneralTree;
using System;

namespace BXJG.CMS
{
    [DependsOn(
          typeof(BXJGCMSCoreModule),
          typeof(AbpAutoMapperModule))]
    public class BXJGCMSApplicationModule:AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            //Configuration.Authorization.Providers.Add<BXJGCMSAuthorizationProvider>();

            //Adding custom AutoMapper configuration
        
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGCMSApplicationModule).GetAssembly());

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            cfg => cfg.AddMaps(typeof(BXJGCMSApplicationModule).GetAssembly())
        );
        }
    }
}
