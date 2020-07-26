using Abp.Modules;
using System;
using BXJG.GeneralTree;
using BXJG.BaseInfo.Localization;
using System.Reflection;

namespace BXJG.BaseInfo
{
    [DependsOn(typeof(GeneralTreeModule))]
    public class BXJGBaseInfoCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //配置本地化
            BXJGBaseInfoLocalizationConfigurer.Configure(Configuration.Localization);

            //注册设置
            //Configuration.Settings.Providers.Add<BXJGBaseInfoAppSettingProvider>();

            //使用Utils模块提供的通用枚举转下拉框数据 但是它不好控制权限
            //Configuration.Modules.BXJGUtils().AddEnum("BXJGBaseInfoOrderStatus", typeof(OrderStatus), BXJGUtilsConsts.LocalizationSourceName);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
            //泛型的领域服务要在这里手动注册
            //IocManager.Register(typeof(ItemManager<>), DependencyLifeStyle.Transient);
        }
    }
}
