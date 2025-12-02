using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using System.Reflection;

namespace BXJG.Inventory
{
    [DependsOn(
        typeof(InventoryCoreModule),
        typeof(BXJG.PSI.MasterData.PSIMasterDataApplicationModule),
        typeof(BXJG.Utils.Application.BXJGUtilsApplicationModule)
    )]
    public class InventoryApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            // 注册 AutoMapper 映射
            Configuration.Modules.AbpAutoMapper().Configurators
                .Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
        
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(InventoryApplicationModule).Assembly);
        }
    }
}