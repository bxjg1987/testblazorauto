using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace BXJG.WeChat.Payment
{
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
            if (!options.CallbackPath.Equals( request.Path, StringComparison.OrdinalIgnoreCase))
            {
                await this._next(context);
            }

            //var rt = await context.Request.BodyReader.ReadAsync(context.RequestAborted);
            //rt.Buffer
            ////request.Body

            ////rt.Buffer
            //XDocument.Load(context.Request.BodyReader);

            await this._next(context);
        }
    }
}
