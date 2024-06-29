using BXJG.Common.Contracts;
using BXJG.Common.Events;
using BXJG.Common.Web;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BXJGCommonWebDIExtensions
    {
        public static IServiceCollection AddBXJGCommonWeb(this IServiceCollection services)
        {
            return services.AddBXJGCommon().AddSingleton<IEnv, AspNetEnv>();
        }
    }
}
