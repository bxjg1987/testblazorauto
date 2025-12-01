using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Utils.EFCore;

namespace BXJG.PSI.MasterData
{
    [DependsOn(
        typeof(PSIMasterDataCoreModule),
        typeof(EFCoreModule)
    )]
    public class PSIMasterDataEFModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PSIMasterDataEFModule).GetAssembly());
        }
    }
}