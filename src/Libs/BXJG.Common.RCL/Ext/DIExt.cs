using BXJG.Common.Events;
using BXJG.Common.Http;
using BXJG.Common.RCL.Auth;
using BXJG.Common.RCL.Events;
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
        public static IServiceCollection AddCommonRCL(this IServiceCollection services, Func<IServiceProvider, IEnumerable<string>> permissionNamesProvider)
        {
            services.AddBXJGCommon()
                    //.AddSingleton<IZhongjieProvider, ZhongjieProvider>()
                    //.AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>()
                    .AddKeyedSingleton<Func<IEnumerable<string>>>(OperationAuthorizationRequirement.GrantedPermissionNamesProvider, (s, o) => () => permissionNamesProvider(s))
                    .AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>();
                    //.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>()
                    //.AddSingleton<AccessTokenProvider>()
                    //.AddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AccessTokenProvider>());
         
            //services.TryAddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
            ////services.TryAddSingleton<AccessTokenProvider>();
            //services.TryAddSingleton<IAccessTokenProvider>(s => s.GetRequiredService<AuthenticationStateProvider>() as PersistentAuthenticationStateProvider);
            return services;
        }


        /// <summary>
        /// 注册Common.RCL
        /// assembly或auto模式才调用
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommonRCLClient(this IServiceCollection services)
        {
            services.AddSingleton<IZhongjieProvider, ZhongjieProvider>();
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