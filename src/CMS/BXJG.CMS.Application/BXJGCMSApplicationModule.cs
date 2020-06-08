using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.CMS.Authorization;
using BXJG.GeneralTree;
using System;
using System.Reflection;

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

            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGCMSApplicationModule).GetAssembly());

            
        }
    }
}
