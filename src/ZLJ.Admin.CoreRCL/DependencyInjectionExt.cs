using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExt
    {
        public static IServiceCollection Adddd(this IServiceCollection services,IConfiguration _appConfiguration) 
        {
            services.AddAntDesign();
            services.Configure<ProSettings>(_appConfiguration.GetSection("ProSettings"));
            return services;
        }
    }
}
