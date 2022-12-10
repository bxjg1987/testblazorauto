using BXJG.Utils.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthorizationExtensionts
    {
        public static IServiceCollection AddAbpAuthorization(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthorizationHandler, AbpAuthorizationHandler>();
        }
    }
}
