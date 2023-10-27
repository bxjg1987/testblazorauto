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
using Abp.Domain.Entities;
using BXJG.Utils.AutoMapper;
using BXJG.Common.Dto;
using BXJG.Utils.Concurrency;
using System.Linq;
using Abp.Application.Services;
using Castle.Core;
//using BXJG.Utils.CAP;
//using DotNetCore.CAP;
using BXJG.Utils.Notification;
using BXJG.Utils.GeneralTree;
using Abp.Configuration.Startup;

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
            Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            {
                //cfg.map
                //cfg.
                //cfg.ForAllMaps((a, b) =>
                //{
                //   // a.IncludedMembersNames
                //   if(  a.IncludedMembersNames.Contains("ConcurrencyStamp"))
                //  //  if (a.DestinationType == typeof(IHasConcurrencyStamp))
                //    {
                //        b.ForMember("ConcurrencyStamp", d => d.MapFrom("ConcurrencyStamp"));
                //    }
                //});
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });
            ////https://aspnetboilerplate.com/Pages/Documents/Articles/Aspect-Oriented-Programming-using-Interceptors/index.html
            //IocManager.IocContainer.Kernel.ComponentRegistered += (key, handler) =>
            //{
            //    if (typeof(ICapSubscribe).IsAssignableFrom(handler.ComponentModel.Implementation))
            //    {
            //        handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AbpAsyncDeterminationInterceptor<AbpCapSubscriptInterceptor>)));
            //    }
            //};

            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));
            //统一配置abp扩展属性映射
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg =>
            //{
            //    cfg.ForAllMaps((a, b) =>
            //    {
            //        if (a.SourceType.GetInterface(typeof(IExtendableObject)?.FullName) != default && a.DestinationType.GetInterface(typeof(IExtendableDto)?.FullName) != default)
            //        {
            //            b.ForMember("ExtensionData", c => c.MapFrom(new sss(), "ExtensionData"));
            //        }
            //    });
            //});
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
           //IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<AbpCapSubscriptInterceptor>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(GeneralTreeManager<>), DependencyLifeStyle.Transient);
            //Configuration.ReplaceService(typeof(GeneralTreeManager<>), () => default,DependencyLifeStyle.Transient);

            //注册附件应用服务，它不实现abp的应用服务，所以不会生成动态webApi
            //IocManager.Register(typeof(AttachmentAppService<>), DependencyLifeStyle.Transient);
        }
    }
}
