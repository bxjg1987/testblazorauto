using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using System.IO;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 微信支付结果通知处理中间件
    /// 拦截请求直接处理，不用进入“复杂的"mvc/webApi流程
    /// </summary>
    public class WeChatPaymentMiddleware
    {
        private readonly RequestDelegate _next;

        public WeChatPaymentMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;

            //若考虑配置文件更新时自动更新选项，则使用IOptionsMonitor<>.CurrentValue，否则用IOptions<>.Value
            var options = context.RequestServices.GetRequiredService<IOptionsMonitor<WeChatPaymentOptions>>().CurrentValue;

            //若当前请求不是支付结果回调请求，则跳过处理，直接执行后续中间件
            if (!options.CallbackPath.Equals(request.Path, StringComparison.OrdinalIgnoreCase))
            {
                await this._next(context);
            }

            //var rt = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
            
            //使用linq to xml 从请求体中得到XDocumnet
            var doc = await XDocument.LoadAsync(request.Body, LoadOptions.None, context.RequestAborted);
           

           
           
        }
    }
}
