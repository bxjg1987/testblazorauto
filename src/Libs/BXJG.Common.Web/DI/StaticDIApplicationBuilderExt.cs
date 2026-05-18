using BXJG.Common.DI;
using BXJG.Common.Events;
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
    public static class StaticDIApplicationBuilderExt
    {
        public static IApplicationBuilder UseBXJGCommonWeb(this IApplicationBuilder appBuilder)
        {
            return appBuilder.UseMiddleware<MyCustomMiddleware>();
        } 
    }

    /// <summary>
    /// 静态DI访问器中间件，用于在请求上下文中设置AsyncLocal值
    /// <br/>历史修复：原条件判断 if (Zhongjie.Current.Value != null) 逻辑反转，导致普通Web请求中Zhongjie永远不会被设置，已改为 == null
    /// </summary>
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
            if (Zhongjie.Current.Value == null)
                Zhongjie.Current.Value = httpContext.RequestServices.GetRequiredService<Zhongjie>();

            if (StaticDIAccessor._serviceProvider.Value == null)
                StaticDIAccessor._serviceProvider.Value = httpContext.RequestServices;

            try
            {
                await _next(httpContext);
            }
            finally
            {
                StaticDIAccessor._serviceProvider.Value = default;
                Zhongjie.Current.Value = null;
            }
        }
    }
}
