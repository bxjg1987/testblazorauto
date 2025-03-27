using Abp.Dependency;
using BXJG.Common.DI;
using BXJG.Utils.DI;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseZLJWeb(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseBXJGUtilsWeb();
        }
    }

}
