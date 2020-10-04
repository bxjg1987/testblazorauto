using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp;
using BXJG.Utils.Localization;
using BXJG.Utils.Enums;
using Abp.Threading.BackgroundWorkers;
using BXJG.Utils.File;
using BXJG.Common;
using Abp.Dependency;

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
            IocManager.Register<BXJGUtilsModuleConfig>();
            Configuration.Modules.BXJGUtils().AddEnum("gender", typeof(Gender), BXJGUtilsConsts.LocalizationSourceName);

            BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGUtilsFileSettingProvider>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsModule).GetAssembly());
            //调试模式时默认实现获取的路径是 ..\bin\debug\wwwroot
            //而asp.net core默认读取是在ZLJ.Web.Host\wwwroot 导致上传的文件看不到效果
            //发布到服务器后不存在这个问题
            //可气的是abp提供的Configuration.ReplaceService<IEnv, NetCoreEnv>(DependencyLifeStyle.Singleton);没什么暖用
            //IocManager.RegisterIfNot<IEnv, Utils.File.DefaultEnv>(Abp.Dependency.DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<RemoveUploadFileWorker>());
        }
    }
}
