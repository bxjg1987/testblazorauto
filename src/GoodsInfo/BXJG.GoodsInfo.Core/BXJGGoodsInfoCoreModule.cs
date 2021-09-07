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
using BXJG.Utils;
using BXJG.GoodsInfo.Localization;

namespace BXJG.GoodsInfo
{
    [DependsOn(typeof(BXJGUtilsModule))]
    public class BXJGGoodsInfoCoreModule : Abp.Modules.AbpModule
    {
        public override void PreInitialize()
        {
            //物品模块配置对象
            IocManager.Register<Configuration>();
            //Configuration.Modules.BXJGUtils().AddEnum("gender", typeof(Gender), BXJGUtilsConsts.LocalizationSourceName);
            LocalizationConfigurer.Configure(Configuration.Localization);
            //Configuration.Settings.Providers.Add<BXJGUtilsFileSettingProvider>();
        }

        public override void Initialize()
        {
            //注册非abp依赖的公共库Common中的服务
            //base.IocManager.RegisterIfNot<IClock, LocalClock>(DependencyLifeStyle.Singleton);
            //base.IocManager.RegisterIfNot<IEnv,BXJG.Common.DefaultWebEnv>(DependencyLifeStyle.Singleton);
            IocManager.RegisterAssemblyByConvention(typeof(BXJGGoodsInfoCoreModule).GetAssembly());
            ////IocManager.Register<IClock, AbpClock>();
            //IocManager.RegService(services => services.AddBXJGCommon());
            //IocManager.Register(typeof(DynamicPropertyManager<>), DependencyLifeStyle.Singleton);

            //IocManager.Register(typeof(AttachmentManager<>), DependencyLifeStyle.Transient);
            //调试模式时默认实现获取的路径是 ..\bin\debug\wwwroot
            //而asp.net core默认读取是在ZLJ.Web.Host\wwwroot 导致上传的文件看不到效果
            //发布到服务器后不存在这个问题，调试时需要在web.core模块PreInitialize中替换服务，注意经过测试一定要在PreInitialize中替换
            //IocManager.Register<IEnv, Utils.File.DefaultEnv>(Abp.Dependency.DependencyLifeStyle.Singleton);
        }

        public override void PostInitialize()
        {
            var cfg = IocManager.Resolve<Configuration>();
            var ctx = new GoodsInfoTypeDefineAddContex();
            var list = new List< GoodsInfoTypeDefine>();
            foreach (var item in cfg.GoodsInfoTypeProviders)
            {
                list.Add(item(ctx));
            }
            cfg.GoodsInfoTypeProviders = null;
            var m = new GoodsInfoTypeDefineManager(list);
            //GoodsInfoTypeManager本身继承DomainService，会在Initialize注册，这里重复注册应该是后者生效，若不行考虑使用ReplaceService
            //base.Configuration.ReplaceService(typeof(GoodsInfoTypeManager), () => IocManager...);
            //单例 实例 注册
            IocManager.RegService(s=>s.Add(new Microsoft.Extensions.DependencyInjection.ServiceDescriptor(typeof(GoodsInfoTypeDefineManager),m)));
           
        }
    }
}
