using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.Abps;
using BXJG.Utils.RCL.Helpers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExt
    {
        public static IServiceCollection UseBXJGUtilsRCL(this IServiceCollection services)
        {
            services.AddCommonRCL(s =>
            {
                var fw = s.GetRequiredService<AppContainer>();
                if (fw.AbpUserConfiguration != null && fw.AbpUserConfiguration.Auth != default)
                {
                    //Console.WriteLine(JsonConvert.SerializeObject(fw.AbpUserConfiguration.Auth));
                    return fw.AbpUserConfiguration.Auth.GrantedPermissions.Keys;
                }
                return [];
            })
            .AddCascadingAuthenticationState()
            .AddTransient<FileHelper>()
            //.AddZLJBlazorClient()
            .AddSingleton(AppContainer.App);
            services.TryAddTransient<IAbpSession, ClientAbpSession>();
            //services.TryAddSingleton<IPermissionChecker, ClientPermissionChecker>();
            services.TryAddTransient<ISettingManager, ClientSettingManager>();
            services.TryAddTransient<IUserNavigationManager, ClientNavigationManager>();
            services.TryAddTransient<IFeatureChecker, ClientFeatureChecker>();
            services.TryAddTransient<ISessionAppService, SessionAppService>();
            //services.TryAddSingleton<IObjectMapper, AutoMapperObjectMapper>();
            services.AddAutoMapper(typeof(AppContainer));

            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
            return services;
        }
    }
}
