using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Utils.Application;

namespace BXJG.PSI.MasterData
{
    [DependsOn(
        typeof(PSIMasterDataCoreModule),
        typeof(BXJGUtilsApplicationModule)
    )]
    public class PSIMasterDataApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PSIMasterDataApplicationModule).GetAssembly());
        }
    }
}