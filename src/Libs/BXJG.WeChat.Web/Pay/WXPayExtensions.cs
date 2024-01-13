using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Web.Pay
{
    /// <summary>
    /// 微信支付模块与web相关的扩展方法
    /// </summary>
    public static class WXPayExtensions
    {
        /// <summary>
        /// 注册微信支付结果通知中间件<br/>
        /// 在此之前请确保已初始化微信支付相关服务<br/>
        /// 参考文档：<seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top"/>
        /// </summary>
        /// <param name="appBuilder"></param>
        public static void UseWXPay(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<PayNotifyMiddleware>();
        }
    }
}
