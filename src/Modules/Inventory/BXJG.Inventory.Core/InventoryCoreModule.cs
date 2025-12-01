using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Inventory.Localization;
using BXJG.PSI.MasterData;

namespace BXJG.Inventory
{
    [DependsOn(
        typeof(BXJGUtilsModule),
        typeof(PSIMasterDataCoreModule)
    )]
    public class InventoryCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            InventoryLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InventoryCoreModule).GetAssembly());
        }
    }
}