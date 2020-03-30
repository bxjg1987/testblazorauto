using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    public class WeChatPaymentConsts
    {
        public const string SUCCESS = "SUCCESS";
        public const string FAIL = "FAIL";
        /// <summary>
        /// 微信小程序支付中用来跟服务器通讯的HttpClient
        /// 参考 命名客户端：https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.1
        /// </summary>
        public const string HttpClientName = "WeChatMiniProgramPayment";
    }
}
