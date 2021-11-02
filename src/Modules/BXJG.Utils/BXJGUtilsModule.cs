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
using BXJG.Utils.DynamicProperty;
using Abp.AutoMapper;
using System.Reflection;
using Abp.Domain.Entities;
using System.Collections.Generic;
using AutoMapper;
using BXJG.Common.Dto;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BXJG.Utils
{
    /*
     * 通用公共功能模块
     */
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class BXJGUtilsModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<BXJGUtilsModuleConfig>();
            //Configuration.Modules.BXJGUtils().AddEnum(typeof(Gender), "gender", BXJGUtilsConsts.LocalizationSourceName);
            Configuration.Modules.BXJGUtils().EnumLocalizationProviders.Add(() => new[] {
                new EnumLocalizationDefine(typeof(Gender), "gender"),
                new EnumLocalizationDefine(typeof(Gender), "bool"),
            });
            BXJGUtilsLocalizationConfigurer.Configure(Configuration.Localization);
            Configuration.Settings.Providers.Add<BXJGUtilsFileSettingProvider>();
        }

        public override void Initialize()
        {
            //注册非abp依赖的公共库Common中的服务
            //base.IocManager.RegisterIfNot<IClock, LocalClock>(DependencyLifeStyle.Singleton);
            //base.IocManager.RegisterIfNot<IEnv,BXJG.Common.DefaultWebEnv>(DependencyLifeStyle.Singleton);


            IocManager.RegisterAssemblyByConvention(typeof(BXJGUtilsModule).GetAssembly());
            //IocManager.Register<IClock, AbpClock>();
            IocManager.RegService(services => services.AddBXJGCommon());
            IocManager.Register(typeof(DynamicPropertyManager<>), DependencyLifeStyle.Singleton);

            IocManager.Register(typeof(AttachmentManager<>), DependencyLifeStyle.Transient);
            //调试模式时默认实现获取的路径是 ..\bin\debug\wwwroot
            //而asp.net core默认读取是在ZLJ.Web.Host\wwwroot 导致上传的文件看不到效果
            //发布到服务器后不存在这个问题，调试时需要在web.core模块PreInitialize中替换服务，注意经过测试一定要在PreInitialize中替换
            //IocManager.Register<IEnv, Utils.File.DefaultEnv>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }

        public override void PostInitialize()
        {
            var utilsCfg = Configuration.Modules.BXJGUtils();
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            //运行ZLJ.Migrator时会确实依赖服务，所以这里try下
            try
            {
                workManager.Add(IocManager.Resolve<RemoveUploadFileWorker>());
            }
            catch
            {

            }

            #region 本地化枚举系统
            var list = new List<EnumLocalizationDefine>();
            foreach (var item in utilsCfg.EnumLocalizationProviders)
            {
                var items = item();

                foreach (var item2 in items)
                {
                    var temp = list.SingleOrDefault(c => c.Name == item2.Name);
                    if (temp != null)
                    {
                        list.Remove(temp);
                    }
                    list.Add(item2);
                }
            }
            utilsCfg.EnumLocalizationProviders = null;
            var sdf = new EnumLocalizationContainer(list);
            base.IocManager.RegService(c => c.TryAddSingleton(sdf));
            #endregion
        }
    }
}
