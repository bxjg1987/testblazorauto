using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using BXJG.Utils;
using System;
using System.Reflection;

namespace BXJG.WorkOrder
{
    [DependsOn(typeof(BXJGCommonApplicationModule))]
    public class BXJGWorkOrderEmployeeApplicationModule : AbpModule
    {
        //IConfiguration configuration;

        //public ApplicationModule(IConfiguration configuration)
        //{
        //    this.configuration = configuration;
        //}

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
            //因为要生成动态webapi，所以这样注册不行，不过在主程序中去试试应该可以
            //IocManager.Register( typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<,,,,,>), DependencyLifeStyle.Transient);
            //IocManager.Register( typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<>), DependencyLifeStyle.Transient);
        }
    }
}
