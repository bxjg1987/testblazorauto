using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ZLJ.Authorization;

namespace ZLJ
{
    [DependsOn(
        typeof(ZLJCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class ZLJApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<ZLJAuthorizationProvider>();
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(typeof(ZLJApplicationModule).GetAssembly())
            );
        }

        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(typeof(ZLJApplicationModule).GetAssembly());

           
        }
    }
}
