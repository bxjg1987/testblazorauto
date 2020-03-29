using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    public class WeChatPaymentNoticeContext
    {
        public HttpContext HttpContext { get; }
        public WeChatPaymentOptions Options { get; }
        public WeChatPaymentNoticeResult PaymentNoticeResult { get; }

        public WeChatPaymentNoticeContext(HttpContext httpContext, WeChatPaymentOptions options, WeChatPaymentNoticeResult paymentNoticeResult)
        {
            HttpContext = httpContext;
            Options = options;
            PaymentNoticeResult = paymentNoticeResult;
        }
    }
}
