using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Runtime.Session;
using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Abp.ObjectMapping;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.Abps;
using BXJG.Utils.RCL.Helpers;
using BXJG.Utils.Application.Share.Session;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 本项目，所有应用，前后端要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLJRCL(this IServiceCollection services)
        {
            
            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
            return services.AddAntDesign().UseBXJGUtilsRCL();
        }
        ///// <summary>
        ///// 本项目，所有应用，前后端都要注册的
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //static IServiceCollection AddZLJBlazor(this IServiceCollection services)
        //{
        //    services.AddAntDesign()
        //            .AddCascadingAuthenticationState()
        //            .AddTransient<ZLJ.RCL.Files.Helper>();
        //    return services;
        //}
        ///// <summary>
        ///// 本项目，所有应用，后端都要注册的
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddZLJBlazorServer(this IServiceCollection services)
        //{
        //    services.AddZLJBlazor()
        //            .AddCommonRCLServer();
        //    return services;
        //}
    }
}