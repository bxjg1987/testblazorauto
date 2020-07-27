using Abp.Modules;
using BXJG.Equipment.Configuration;
using BXJG.Equipment.Localization;
using BXJG.GeneralTree;
using BXJG.Utils;
using System;

namespace BXJG.Equipment
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class BXJEquipmentCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGEquipmentLocalizationConfigurer.Configure(Configuration.Localization);
            //Configuration.Settings.Providers.Add<BXJGEquipmentAppSettingProvider>();
    
            //使用Utils模块提供的通用枚举转下拉框数据
            //但是它不好控制权限
            //Configuration.Modules.BXJGUtils().AddEnum("BXJGEquipmentOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);
            //泛型的领域服务要在这里手动注册
            //IocManager.Register(typeof(ItemManager<>), DependencyLifeStyle.Transient);
        }
    }
}
