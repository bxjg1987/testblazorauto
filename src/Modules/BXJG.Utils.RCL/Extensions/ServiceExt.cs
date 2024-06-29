using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.Helpers;
using BXJG.Utils.RCL.SignalR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUglify.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExt
    {
        /// <summary>
        /// blazor server和wasm都要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseBXJGUtilsRCL(this IServiceCollection services)
        {
            services.AddCommonRCL(async s =>
            {
                var fw = s.GetRequiredService<AppContainer>();

                var r =(await fw.AbpUserConfiguration)?.Auth?.GrantedPermissions?.Keys;
                if (r == null)
                    return [];

                return r;

            });
            //.AddTransient<FileHelper>()
            //.AddZLJBlazorClient()
            //.AddScoped(AppContainer.App);
            services.AddScoped<AppContainer>();
           
            //services.TryAddSingleton<IObjectMapper, AutoMapperObjectMapper>();
            services.TryAddScoped<CommonConnection>();
            services.AddAutoMapper(typeof(AppContainer));

            services.TryAddTransient<FileHelper>();

            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
            return services;
        }
    }


}
