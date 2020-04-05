using Abp.Modules;
using Abp.Reflection.Extensions;
using System;

namespace BXJG.Shop
{
    [DependsOn(
     typeof(BXJGShopCoreModule))]
    public class BXJGShopEFCoreModule:AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGShopEFCoreModule).GetAssembly());
        }
    }
}
