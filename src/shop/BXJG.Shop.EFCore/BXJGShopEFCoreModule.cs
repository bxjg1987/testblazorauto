using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Reflection;

namespace BXJG.Shop
{
    [DependsOn(
     typeof(BXJGShopCoreModule))]
    public class BXJGShopEFCoreModule : AbpModule
    {
        public static Assembly GetAssembly()
        {
            return typeof(BXJGShopEFCoreModule).GetAssembly();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(GetAssembly());
        }
    }
}
