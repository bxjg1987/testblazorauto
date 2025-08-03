using BXJG.Common.Contracts;
using BXJG.Common.Events;
using BXJG.Common.Session;
using BXJG.Common.Web;
using BXJG.Common.Web.Session;
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
            services.AddHttpContextAccessor()
                    .TryAddScoped<BXJG.Common.Session.ISession, ReqSession>();
            services.AddBXJGCommon()
                     //这里别try，因为要替换common中的空实现
                     .AddSingleton<IEnv, AspNetEnv>();
            return services;
        }
    }
}
