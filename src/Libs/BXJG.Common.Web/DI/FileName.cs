using BXJG.Common.Web.DI;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class StaticDIApplicationBuilderExt
    {
        public static IApplicationBuilder UseStaticDI(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<DISetter>();
        }
    }
}
