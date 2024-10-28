using Abp.Dependency;
using BXJG.Common.DI;
using BXJG.Utils.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Web
{
    public static class AppBuilderExtensions
    {
        public static IApplicationBuilder UseStaticAbpDI(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<AbpDIMiddleware>();  
        }
    }

    public class AbpDIMiddleware
    {
        private readonly RequestDelegate _next;

        public AbpDIMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMessageWriter is injected into InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext)
        {
            AbpDIStaticAccessor.iocResolver.Value = httpContext.RequestServices.GetRequiredService<IScopedIocResolver>();
            await _next(httpContext);
            AbpDIStaticAccessor.iocResolver.Value = default;//asynclocal基于线程，请求线程结束了，这里有点多余 好像。
        }
    }
}
