using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    public static class WeChatPaymentMiddlewareExtensions
    {
        /// <summary>
        /// 注册微信支付结果通知中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatPayment(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WeChatPaymentMiddleware>();
        }
    }
}
