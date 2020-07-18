using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Equipment;
using System;
using System.Reflection;

namespace BXJG.Shop
{
    [DependsOn(typeof(BXJEquipmentCoreModule))]
    public class BXJGEquipmentEFCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
