using BXJG.Common.Events;
using BXJG.Common.Http;
using BXJG.Common.RCL.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 注册Common.RCL
        /// assembly或auto模式调用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="permissionNamesProvider"></param>
        /// <returns></returns>
        public static IServiceCollection AddBXJGCommonRCL(this IServiceCollection services, Func<IServiceProvider, ValueTask<IEnumerable<string>>> permissionNamesProvider = default)
        {
            // Microsoft.AspNetCore.Authorization.Infrastructure.OperationAuthorizationRequirement
            //  operareq


            if (permissionNamesProvider == default)
                permissionNamesProvider = s => ValueTask.FromResult(Enumerable.Empty<string>());

            services.AddBXJGCommon()
                    .AddCascadingAuthenticationState()
                    //下面这俩应该是不能用try，因为我们确实需要替换asp.net core默认的
                    .AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>()
                    .AddScoped<IAuthorizationHandler, OperationAuthorizationRequirement1>()
                    //.AddSingleton<IZhongjieProvider, ZhongjieProvider>()
                    //.AddScoped<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>()
                    .TryAddKeyedScoped<Func<ValueTask<IEnumerable<string>>>>(OperationAuthorizationRequirement1.GrantedPermissionNamesProvider, (s, o) => () => permissionNamesProvider(s));
            //.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>()
            //.AddSingleton<AccessTokenProvider>()
            //.AddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AccessTokenProvider>());
            services.TryAddCascadingValue(x => x.GetRequiredService<Zhongjie>());
            //services.TryAddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            ////services.TryAddSingleton<AccessTokenProvider>();
            //services.TryAddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AuthenticationStateProvider>() as PersistentAuthenticationStateProvider);
            return services;
        }


        //这个库是客户端和服务端共享的，所以这个库没有提供服务端的AuthenticationStateProvider实现，因为服务端的实现好像会引用blazor server相关的东东
        //没有在这里提供组合的注册，比如在AddBXJGCommonRCLClient种直接就注册AddBXJGCommonRCL，主要考虑有时候调用方需要分开处理，

        /// <summary>
        /// 注册Common.RCL
        /// 注册blazor客户端部分的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBXJGCommonRCLClient(this IServiceCollection services)
        {
            //这里不能用try，因为要替换common的默认实现
            services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            services.AddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AuthenticationStateProvider>() as PersistentAuthenticationStateProvider);
            return services;
        }

        //static IServiceCollection AddCommonRCL(this IServiceCollection services)
        //{
        //    //可能被重复调用，注册时要注意
        //    return services.AddBXJGCommon();
        //}
        ////引用服务端的包，客户端启动时会报错
        //public static IServiceCollection AddCommonRCLServer(this IServiceCollection services)
        //{
        //    services.AddCommonRCL();
        //    return services;
        //}
        //public static IServiceCollection AddCommonRCLPermissionProvider(this IServiceCollection services, Func<IServiceProvider, Func<IEnumerable<string>>> permissionNamesProvider)
        //{
        //    services.AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>();
        //    services.AddKeyedTransient<Func<IEnumerable<string>>>(OperationAuthorizationRequirement.GrantedPermissionNamesProvider, (s, o) => permissionNamesProvider(s));
        //    return services;
        //}
    }
}