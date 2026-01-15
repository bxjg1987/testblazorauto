using BXJG.Common.Http;
using BXJG.Utils.RCL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExt
    {
        /// <summary>
        /// 后台管理应用，服务端和客户端共同需要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdminBlazor(this IServiceCollection services)
        {
            var sdfsfd = SimpleLogger.Instance;
            sdfsfd.LogDebug("hulalalall");

            services.AddHybridCache(opt => {
                opt.DefaultEntryOptions = new Caching.Hybrid.HybridCacheEntryOptions
                {
                    Flags = Caching.Hybrid.HybridCacheEntryFlags.DisableDistributedCache,
                    LocalCacheExpiration = TimeSpan.FromSeconds(10)
                };
            });
            services.AddECharts();

            BXJGHttpClientExt.DefaultFctory = f => f.CreateHttpClientAdmin();
            return services.AddZLJRCL().AddAutoMapper(x=>x.AddMaps(typeof(DependencyInjectionExt)));
        }
    }
}
