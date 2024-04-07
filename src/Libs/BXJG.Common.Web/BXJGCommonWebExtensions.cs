using BXJG.Common.Contracts;
using BXJG.Common.Events;
using BXJG.Common.Web;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BXJGCommonWebExtensions
    {
        public static IServiceCollection AddBXJGCommonWeb(this IServiceCollection services)
        {
            services.AddScoped<TrackingCircuitHandler>();
            services.AddScoped<CircuitHandler>(x => x.GetRequiredService<TrackingCircuitHandler>());
            services.AddScoped<IZhongjieProvider>(x => x.GetRequiredService<TrackingCircuitHandler>());
            return services.AddBXJGCommon().AddSingleton<IEnv, AspNetEnv>();
        }
    }
}
