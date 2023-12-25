using Abp.Runtime.Session;
using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using ZLJ.Web.Blazor.Abp;
using ZLJ.Web.Blazor.Auth;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 本项目，所有应用，前端要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLJBlazorClient(this IServiceCollection services, Func<IServiceProvider, IEnumerable<string>> permissionNamesProvider)
        {
            services.AddZLJBlazor()
                    .AddCommonRCLClient(permissionNamesProvider)
                    .AddTransient<IAbpSession, MyAbpSession>();
            return services;
        }
        /// <summary>
        /// 本项目，所有应用，前后端都要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        static IServiceCollection AddZLJBlazor(this IServiceCollection services)
        {
            services.AddAntDesign()
                    .AddCascadingAuthenticationState();
            return services;
        }
        /// <summary>
        /// 本项目，所有应用，后端都要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLJBlazorServer(this IServiceCollection services)
        {
            services.AddZLJBlazor()
                    .AddCommonRCLServer();
            return services;
        }
    }
}