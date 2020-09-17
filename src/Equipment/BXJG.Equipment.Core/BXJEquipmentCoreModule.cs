using Abp.Modules;
using BXJG.Equipment.Configuration;
using BXJG.Equipment.EquipmentInfo;
using BXJG.Equipment.Localization;
using BXJG.Equipment.Protocol;
using BXJG.GeneralTree;
using BXJG.Utils;
using SuperSocket.Client;
using SuperSocket.ProtoBase;
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

            //包解析用的，我们当前的方案不需要它，但是EasyClient构造函数依赖它
            IocManager.Register<IPipelineFilter<OxygenChamberPackage>, OxygenChamberPackagePipelineFilter>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //开源针对每种包的发送对应一个client
            //IocManager.Register<IEasyClient<OxygenChamberPackage, OxygenChamberStatePackage>, EasyClient<OxygenChamberPackage, OxygenChamberStatePackage>>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //IocManager.Register<IEasyClient<OxygenChamberPackage, cykz>, EasyClient<OxygenChamberPackage, cykz>>(Abp.Dependency.DependencyLifeStyle.Singleton);
            //若如上注册，那么我们的EquipmentManager里需要注入多个对象，使用下面简化形式
            // fszt : IPackageEncoder<OxygenChamberStatePackage>
            IocManager.Register<IEasyClient<OxygenChamberPackage>, EasyClient<OxygenChamberPackage>>(Abp.Dependency.DependencyLifeStyle.Singleton);
            IocManager.Register<IPackageEncoder<OxygenChamberStatePackage>, fszt>(Abp.Dependency.DependencyLifeStyle.Singleton);
            IocManager.Register<IPackageEncoder<cykz>, fscy>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }
    }
}
