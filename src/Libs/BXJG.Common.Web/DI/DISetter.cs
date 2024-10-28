using BXJG.Common.DI;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Web.DI
{
    public class DISetter : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            StaticDIAccessor._serviceProvider.Value = context.RequestServices;
            await next(context);
            StaticDIAccessor._serviceProvider.Value = default;//asynclocal基于线程，请求线程结束了，这里有点多余 好像。
        }
    }
}
