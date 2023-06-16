using BXJG.Common.DongtaiZhongjie;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common
{
    public static class BXJGCommonExtensions
    {
        public static IServiceCollection AddBXJGCommon(this IServiceCollection services)
        {
            return services.AddSingleton<IClock, LocalClock>().AddSingleton<Zhongjie>();
        }
    }
}
