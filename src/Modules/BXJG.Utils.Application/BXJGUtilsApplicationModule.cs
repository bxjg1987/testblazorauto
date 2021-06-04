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
using System.Reflection;
using Abp.AutoMapper;

namespace BXJG.Utils
{
    [DependsOn(typeof(BXJGUtilsModule),
               typeof(AbpAutoMapperModule))]
    public class BXJGUtilsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            //Configuration.Authorization.Providers.Add<BXJGShopAuthorizationProvider>();

            //需要模块调用方提供必要的泛型参数，所以映射的配置由调用方主动来执行，参考BXJGShopMapProfile
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile(new MapProfile(configuration)));

            //此行必加
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //注册附件应用服务，它不实现abp的应用服务，所以不会生成动态webApi
            //IocManager.Register(typeof(AttachmentAppService<>), DependencyLifeStyle.Transient);
        }
    }
}
