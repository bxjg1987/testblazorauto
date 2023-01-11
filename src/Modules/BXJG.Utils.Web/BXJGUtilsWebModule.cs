using Abp.Modules;
using System;
using BXJG.Common;
using Abp.Reflection.Extensions;
using Abp.Dependency;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
//using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Authorization;
using BXJG.Utils.Authorization;
using System.Collections.Generic;
using Abp.Configuration.Startup;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BXJG.Utils
{
    [DependsOn(typeof(BXJGUtilsApplicationModule))]
    public class BXJGUtilsWebModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            IocManager.RegService(services =>
            {
                //services.AddAuthorization(c => { 
                //    c.AddPolicy
                //});
                //services.PostConfigure<AuthorizationOptions>(opt => {
                //    opt.AddPolicy("", new IAuthorizationPolicyProvider (new IAuthorizationRequirement[] {   }));
                //});
                //本来想在这里将权限作为授权策略（它很轻量）注册，后来缓存AbpAuthorizationPolicyProvider的方式，运行时创建策略了
                services.AddBXJGCommonWeb();
                //使用iocmanager替换无效
                services.Replace(new ServiceDescriptor(typeof(IAuthorizationPolicyProvider),typeof(AbpAuthorizationPolicyProvider), ServiceLifetime.Singleton));
            });
            IocManager.Register<IAuthorizationHandler, AbpAuthorizationHandler>();
            //var services = new ServiceCollection();
            //services.AddBXJGCommonWeb();
            //IocManager.IocContainer.AddServices(services);
            // DefaultAuthorizationPolicyProvider
           // Configuration.ReplaceService<IAuthorizationPolicyProvider, AbpAuthorizationPolicyProvider>(DependencyLifeStyle.Singleton);
        }

        public override void PostInitialize()
        {

            //
            // var sdf = context.RequestServices.GetService<IEnumerable<IAuthorizationPolicyProvider>>();

            //var sdf2 = context.RequestServices.GetService<IAuthorizationPolicyProvider>();
           // var xx = IocManager.Resolve<IAuthorizationPolicyProvider>();
            //var  yyy=  IocManager.Resolve<IEnumerable<IAuthorizationPolicyProvider>>();

          
            // asp.net core 默认的是 Transient生命周期的
           // IocManager.Register<IAuthorizationPolicyProvider, AbpAuthorizationPolicyProvider>(DependencyLifeStyle.Transient);
            // base.PostInitialize();
           // Configuration.ReplaceService<IAuthorizationPolicyProvider, AbpAuthorizationPolicyProvider>(DependencyLifeStyle.Singleton);
        }
    }
}