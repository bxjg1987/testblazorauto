using BXJG.Common.RCL;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBXJGCommonRCL(this IServiceCollection services) {
           return services.AddSingleton<CircuitStateContainer>();
        }
    }
}
