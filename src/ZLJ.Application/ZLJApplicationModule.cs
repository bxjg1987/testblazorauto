using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System.Reflection;
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
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }

        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(typeof(ZLJApplicationModule).GetAssembly());

            
        }
    }
}
