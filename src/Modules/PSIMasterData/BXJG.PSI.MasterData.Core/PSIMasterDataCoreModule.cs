using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.PSI.MasterData.Localization;

namespace BXJG.PSI.MasterData
{
    [DependsOn(
        typeof(BXJGUtilsModule)
    )]
    public class PSIMasterDataCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            PSIMasterDataLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PSIMasterDataCoreModule).GetAssembly());
        }
    }
}