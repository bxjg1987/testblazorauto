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
        public static IApplicationBuilder UseBXJGUtilsWeb(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseBXJGCommonWeb().UseMiddleware<AbpDIMiddleware>();
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
            //AbpKernelModule IocManager.Register<IScopedIocResolver, ScopedIocResolver>(DependencyLifeStyle.Transient);
            //所以是安全的
            var old = AbpDIStaticAccessor._resolver.Value;
            using (var temp = httpContext.RequestServices.GetRequiredService<IScopedIocResolver>())
            {
                AbpDIStaticAccessor._resolver.Value = temp;
                //Console.WriteLine("");
                //httpContext.RequestServices.GetRequiredService<ILogger>().Debug($"请求中间件查看当前IocResolver：{AbpDIStaticAccessor._resolver.Value.GetHashCode()}");
                try
                {
                    await _next(httpContext);
                }
                finally
                {
                    AbpDIStaticAccessor._resolver.Value = old;//asynclocal基于线程，请求线程结束了，这里有点多余 好像。
                }
            }
        }
    }
}
