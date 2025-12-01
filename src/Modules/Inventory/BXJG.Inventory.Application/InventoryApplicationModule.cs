using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Utils.Application;
using BXJG.PSI.MasterData;

namespace BXJG.Inventory
{
    [DependsOn(
        typeof(InventoryCoreModule),
        typeof(PSIMasterDataApplicationModule),
        typeof(BXJGUtilsApplicationModule)
    )]
    public class InventoryApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InventoryApplicationModule).GetAssembly());
        }
    }
}