using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp;
using BXJG.Utils.Localization;

namespace BXJG.Utils
{
    /*
     * 通用公共功能模块
     */
    //[DependsOn( typeof(Abp.AbpKernelModule))]
    public class BXJGUtilsModule : AbpModule
    {
        public override void PreInitialize()
        {
            BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
