using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Utils.EFCore;
using BXJG.PSI.MasterData;

namespace BXJG.Inventory
{
    [DependsOn(
        typeof(InventoryCoreModule),
        typeof(PSIMasterDataEFModule),
        typeof(EFCoreModule)
    )]
    public class InventoryEFModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InventoryEFModule).GetAssembly());
        }
    }
}