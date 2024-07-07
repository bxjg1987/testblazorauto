using Abp.Modules;
using System;
using Abp.Reflection.Extensions;
using Abp.Dependency;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
//using Castle.Windsor.MsDependencyInjection;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Abp.Configuration.Startup;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BXJG.Common.Web;
using BXJG.Utils.Application;

using BXJG.Common.Contracts;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Abp.AspNetCore;
using BXJG.Common;
using BXJG.Utils.Share;
using Castle.MicroKernel.Registration;
using Abp.RealTime;
using Microsoft.AspNetCore.SignalR;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.Domain.Uow;
using Abp.Notifications;

namespace BXJG.Utils.Web
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
                services.AddSingleton<IEnv, AspNetEnv>();//.AddBXJGCommonWeb(); //core已经注册内部的服务了
                                                         //使用iocmanager替换无效
                                                         //  services.Replace(new ServiceDescriptor(typeof(IAuthorizationPolicyProvider),typeof(AbpAuthorizationPolicyProvider), ServiceLifetime.Singleton));
            });
            //  IocManager.Register<IAuthorizationHandler, AbpAuthorizationHandler>();
            //var services = new ServiceCollection();
            //services.AddBXJGCommonWeb();
            //IocManager.IocContainer.AddServices(services);
            // DefaultAuthorizationPolicyProvider
            // Configuration.ReplaceService<IAuthorizationPolicyProvider, AbpAuthorizationPolicyProvider>(DependencyLifeStyle.Singleton);

            #region 后端修改了前端关心的状态时，推送
            ////我们用的老版本的abp，它严重依赖windosr，且不打算更换的，所以我们的代码中即使强依赖windsor也没有问题
            ////不晓得如何在构造函数注入windsor的键控服务，那就用反模式，解析器直接解析
            ////由于是单例的，所以可以在使用时注入IocManager，同构iocManager.Container

            ////很多其它事件其实都可以用同一种方式，这里可以遍历多种事件，委托可以公用，获取由YanchiChuli.Request绝对触发前端的何种事件
            ////或者结合abp本身的entity变动事件，也要支持主动触发，打通abp的事件和前端事件。
            //IocManager.IocContainer.Register(Component.For<YanchiChuli>().Instance(new YanchiChuli(async (state, cts) =>
            //{
            //    //using var fw = IocManager.CreateScope();
            //    //var fw = IocManager.IocContainer;
            //    //需要调试看看，若这里是单例的，就不需要创建范围
            //    //var cm = fw.Resolve<IOnlineClientManager>();
            //    var hc = IocManager.Resolve<IHubContext<AbpCommonHub>>();
            //    // var cl = await cm.GetAllClientsAsync();
            //    await hc.Clients.All.SendAsync(BXJGUtilsConsts.OnApplicationStateChanged, cts);
            //}, 10000)).Named(BXJGUtilsConsts.OnApplicationStateChanged));
            //IocManager.IocContainer.Register(Component.For<YanchiChuli>().Instance(new YanchiChuli(async (state, cts) =>
            //{
            //    // var cm = IocManager.Resolve<IOnlineClientManager>();
            //    var tentId = Convert.ToInt32(state);
            //    //using var fw = IocManager.CreateScope();
            //    //var fw = IocManager.IocContainer;
            //    //需要调试看看，若这里是单例的，就不需要创建范围
            //    var cm = IocManager.Resolve<IOnlineClientManager>();
            //    var hc = IocManager.Resolve<IHubContext<AbpCommonHub>>();
            //    var cl = await cm.GetAllClientsAsync();
            //    foreach (var item in cl)
            //    {
            //        if (item.TenantId != tentId)
            //            continue;

            //        var signalRClient = hc.Clients.Client(item.ConnectionId);
            //        if (signalRClient == null)
            //        {
            //            //  Logger.Debug("Can not get user " + userNotification.ToUserIdentifier() + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
            //            continue;
            //        }
            //        await signalRClient.SendAsync(BXJGUtilsConsts.OnTenantStateChanged, cts);
            //    }
            //    //await hc.Clients.All.SendAsync(BXJGUtilsConsts.OnTenantStateChanged, cts);
            //}, 10000)).Named(BXJGUtilsConsts.OnTenantStateChanged));
            //IocManager.IocContainer.Register(Component.For<YanchiChuli>().Instance(new YanchiChuli(async (state, cts) =>
            //{
            //    // var cm = IocManager.Resolve<IOnlineClientManager>();
            //    var userId = Convert.ToInt64(state);
            //    //using var fw = IocManager.CreateScope();
            //    //var fw = IocManager.IocContainer;
            //    //需要调试看看，若这里是单例的，就不需要创建范围
            //    var cm = IocManager.Resolve<IOnlineClientManager>();
            //    var hc = IocManager.Resolve<IHubContext<AbpCommonHub>>();
            //    var cl = await cm.GetAllClientsAsync();
            //    foreach (var item in cl)
            //    {
            //        if (item.UserId != userId)
            //            continue;

            //        var signalRClient = hc.Clients.Client(item.ConnectionId);
            //        if (signalRClient == null)
            //        {
            //            //  Logger.Debug("Can not get user " + userNotification.ToUserIdentifier() + " with connectionId " + onlineClient.ConnectionId + " from SignalR hub!");
            //            continue;
            //        }
            //        await signalRClient.SendAsync(BXJGUtilsConsts.OnUserStateChanged, cts);
            //    }
            //    //await hc.Clients.All.SendAsync(BXJGUtilsConsts.OnTenantStateChanged, cts);
            //}, 10000)).Named(BXJGUtilsConsts.OnUserStateChanged));
            #endregion
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>().AddApplicationPartsIfNotAddedBefore(Assembly.GetExecutingAssembly());
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