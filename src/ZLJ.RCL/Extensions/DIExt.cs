using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Localization;
using Abp.Runtime.Session;
using BXJG.Common.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZLJ.Application.Common.Share.Session;
using ZLJ.RCL.Abps;
using ZLJ.RCL;
using Abp.ObjectMapping;
using Abp.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExt
    {
        /// <summary>
        /// 本项目，所有应用，前端要注册的
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddZLJBlazorClient(this IServiceCollection services)
        {
            services.AddZLJBlazor()
                    //.AddZLJBlazorClient()
                    .AddSingleton(AppContainer.App)
                    .AddCommonRCLClient(s =>
                    {
                        var fw = s.GetRequiredService<AppContainer>();
                        if (fw.AbpUserConfiguration != null && fw.AbpUserConfiguration.Auth != default)
                        {
                            //Console.WriteLine(JsonConvert.SerializeObject(fw.AbpUserConfiguration.Auth));
                            return fw.AbpUserConfiguration.Auth.GrantedPermissions.Keys;
                        }
                        return new string[0];
                    });
            services.TryAddTransient<IAbpSession, ClientAbpSession>();
            //services.TryAddSingleton<IPermissionChecker, ClientPermissionChecker>();
            services.TryAddTransient<ISettingManager, ClientSettingManager>();
            services.TryAddTransient<IUserNavigationManager, ClientNavigationManager>();
            services.TryAddTransient<IFeatureChecker, ClientFeatureChecker>();
            services.TryAddTransient<ISessionAppService, SessionAppService>();
            services.TryAddSingleton<IObjectMapper, AutoMapperObjectMapper>();
            services.TryAddTransient<ZLJ.RCL.Files.Helper>();
            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
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