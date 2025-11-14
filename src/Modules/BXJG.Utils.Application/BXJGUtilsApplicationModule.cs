using Abp;
using Abp.Application.Services;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Abp.Timing;
using BXJG.Common;
using BXJG.Utils.Application.Feedback;
using BXJG.Utils.Application.File;
using BXJG.Utils.Application.GeneralTree;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.GeneralTree;
using BXJG.Utils.Application.User;
using BXJG.Utils.Concurrency;
using BXJG.Utils.DynamicProperty;
using BXJG.Utils.Enums;
using BXJG.Utils.Feedback;
//using BXJG.Utils.CAP;
//using DotNetCore.CAP;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Localization;
using Castle.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace BXJG.Utils.Application
{
    [DependsOn(typeof(BXJGUtilsModule),
               typeof(AbpAutoMapperModule))]
    public class BXJGUtilsApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //StaticDIAccessInterceptor.Initialize(IocManager);
            //Adding authorization providers
            //Configuration.Authorization.Providers.Add<BXJGShopAuthorizationProvider>();

            //需要模块调用方提供必要的泛型参数，所以映射的配置由调用方主动来执行，参考BXJGShopMapProfile
            //Configuration.Modules.AbpAutoMapper().Configurators.Add(cfg => cfg.AddProfile(new MapProfile(configuration)));
            //Configuration.Authorization.Providers.Add<PermissionProvider>();


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
            IocManager.Register(typeof(FeedbackFrontAppService<,,>), DependencyLifeStyle.Transient);
            IocManager.Register(typeof(FeedbackAdminAppService<,,>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(UserAppService<,,,,,,,>), DependencyLifeStyle.Transient);
            IocManager.Register(typeof(CrudBaseAppService<,,,,,,,>),typeof(ICrudBaseAppService<,,,,,,>), DependencyLifeStyle.Transient);
            IocManager.Register(typeof(GeneralTreeBaseAppService<,,,,,>),typeof(IGeneralTreeBaseAppService<,,,>), DependencyLifeStyle.Transient);

            //IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<StaticDIAccessInterceptor>), DependencyLifeStyle.Transient);

            //IocManager.Register(typeof(AbpAsyncDeterminationInterceptor<AbpCapSubscriptInterceptor>), DependencyLifeStyle.Transient);
            //IocManager.Register(typeof(GeneralTreeManager<>), DependencyLifeStyle.Transient);
            //Configuration.ReplaceService(typeof(GeneralTreeManager<>), () => default,DependencyLifeStyle.Transient);

            //注册附件应用服务，它不实现abp的应用服务，所以不会生成动态webApi
            //IocManager.Register(typeof(AttachmentAppService<>), DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            base.PostInitialize();

            IocManager.RegService(services => {

                //   ConfigurationBuilder cb = new ConfigurationBuilder();
                //  var sdfsdf =  IocManager.Resolve<IConfigurationBuilder>();
                var cfg = IocManager.Resolve<IConfiguration>();
                //var sddf1 = xxx["CaptchaOptions"];
                // var sddf = xxx["CaptchaOptions:IgnoreCase"];
                //cb.sett
                services.AddCaptcha(cfg);
            });
        }
    }
}
