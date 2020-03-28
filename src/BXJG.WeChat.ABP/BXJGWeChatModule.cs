using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp;
using BXJG.Utils.Localization;
using BXJG.Utils.Enums;

namespace BXJG.WeChat.ABP
{
    /*
     * 通用公共功能模块
     */
    //[DependsOn( typeof(Abp.AbpKernelModule))]
    public class BXJGWeChatModule : AbpModule
    {
        public override void PreInitialize()
        {
            //IocManager.Register<BXJGUtilsModuleConfig>();
            //Configuration.Modules.BXJGUtils().AddEnum("gender", typeof(Gender), BXJGUtilsConsts.LocalizationSourceName);

            //BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
            IocManager.AddConventionalRegistrar(new BXJGConventionalDependencyRegistrar());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGWeChatModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
