using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using System.Reflection;

namespace BXJG.PSI.MasterData
{
    [DependsOn(
        typeof(PSIMasterDataCoreModule),
        typeof(BXJG.Utils.Application.BXJGUtilsApplicationModule)
    )]
    public class PSIMasterDataApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 注册 AutoMapper 映射
            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
        
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PSIMasterDataApplicationModule).Assembly);
        }
    }
}