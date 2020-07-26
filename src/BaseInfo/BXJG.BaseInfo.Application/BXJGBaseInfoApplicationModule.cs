using Abp.AutoMapper;
using Abp.Modules;
using BXJG.BaseInfo.Authorization;
using System;
using System.Reflection;

namespace BXJG.BaseInfo
{
    [DependsOn(typeof(BXJGBaseInfoCoreModule),
               typeof(AbpAutoMapperModule))]
    public class BXJGBaseInfoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //为了模块调用方能方便的将权限和菜单插入指定节点下，已提供扩展方法的形式，因此这里不做权限和菜单的注册
            //权限
            //Configuration.Authorization.Providers.Add<BXJGBaseInfoAuthorizationProvider>();

            //菜单 abp默认项目模板是在XX.Web.Core中注册的，由于我们做模块化开发时没有考虑web层，所以在这里注册
            //Configuration.Navigation.Providers.Add<BXJGBaseInfoNavigationProvider>();
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            //经过测试，泛型的应用服务即便是通过以下方式手动注册，也无法生成动态webApi，只能在主程序中去注册
            //IocManager.Register( typeof(IBXJGShopItemAppService), typeof(BXJGShopItemAppService<,,,,,>), DependencyLifeStyle.Transient);
            //IocManager.Register( typeof(IBXJGShopFrontItemAppService), typeof(BXJGShopFrontItemAppService<>), DependencyLifeStyle.Transient);

            //注册automapper映射，abp默认项目模板automapper映射是在Initialize()中注册的
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
        }
    }
}
