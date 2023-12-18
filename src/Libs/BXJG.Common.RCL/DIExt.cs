using BXJG.Common.RCL.Auth;
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
        /// assembly或auto模式才调用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="permissionNamesProvider"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommonRCLForClient(this IServiceCollection services, Func<IServiceProvider, IEnumerable<string>> permissionNamesProvider)
        {
            services.AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>();
            services.AddKeyedSingleton<Func<IEnumerable<string>>>(OperationAuthorizationRequirement.GrantedPermissionNamesProvider, (s, o) =>()=> permissionNamesProvider(s));
            return services;
        }

        //public static IServiceCollection AddCommonRCLPermissionProvider(this IServiceCollection services, Func<IServiceProvider, Func<IEnumerable<string>>> permissionNamesProvider)
        //{
        //    services.AddTransient<IAuthorizationPolicyProvider, PermissionNameAuthorizationPolicyProvider>();
        //    services.AddKeyedTransient<Func<IEnumerable<string>>>(OperationAuthorizationRequirement.GrantedPermissionNamesProvider, (s, o) => permissionNamesProvider(s));
        //    return services;
        //}
    }
}
