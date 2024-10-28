using BXJG.Common.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            return appBuilder.UseMiddleware<MyCustomMiddleware>();
        }
    }

    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public MyCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMessageWriter is injected into InvokeAsync
        public async Task InvokeAsync(HttpContext httpContext)
        {
            StaticDIAccessor._serviceProvider.Value = httpContext.RequestServices;
            await _next(httpContext);
            StaticDIAccessor._serviceProvider.Value = default;//asynclocal基于线程，请求线程结束了，这里有点多余 好像。
        }
    }
}
