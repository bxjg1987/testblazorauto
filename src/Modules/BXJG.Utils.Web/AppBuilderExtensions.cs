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

        // 这里只是个兜底，应用层使用了拦截器再次设置
        public async Task InvokeAsync(HttpContext httpContext)
        {
            AbpDIStaticAccessor._resolver.Value = httpContext.RequestServices.GetRequiredService<IScopedIocResolver>();
            //Console.WriteLine("");
            //httpContext.RequestServices.GetRequiredService<ILogger>().Debug($"请求中间件查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value.GetHashCode()}");
            await _next(httpContext);
            AbpDIStaticAccessor._resolver.Value = default;//asynclocal基于线程，请求线程结束了，这里有点多余 好像。
        }
    }
}
