using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public static class BXJGCommonWebExtensions
    {
        public static IServiceCollection AddBXJGCommon(this IServiceCollection services)
        {
            return services.AddSingleton<IEnv, AspNetEnv>();
        }
    }
}
