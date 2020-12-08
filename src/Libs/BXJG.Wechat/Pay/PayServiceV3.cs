using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BXJG.Common;
using BXJG.WeChat.Pay.Entities;
using Microsoft.Extensions.Options;

namespace BXJG.WeChat.Pay
{
    /// <summary>
    /// 微信支付V3接口<br/>
    /// <seealso href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pages/Overview.shtml" />
    /// </summary>
    public class PayServiceV3
    {
        /// <summary>
        /// 微信支付模块选项对象
        /// </summary>
        WXPayOption option;
        /// <summary>
        /// 用来访问的微信支付平台接口的httpClient，它通过消息处理器来实施签名和验签
        /// </summary>
        IHttpClientFactory wxClientFactory;
        /// <summary>
        /// 时钟
        /// 用于获取准确的当前时间
        /// </summary>
        IClock clock;
        /// <summary>
        /// web环境相关信息
        /// </summary>
        IEnv webEnvironment;
        public PayServiceV3(IOptionsMonitor<WXPayOption> wxPaymentOption, IHttpClientFactory wxClientFactory, IClock clock, IEnv webEnvironment)
        {
            this.option = wxPaymentOption.CurrentValue;
            this.wxClientFactory = wxClientFactory;
            this.clock = clock;
            this.webEnvironment = webEnvironment;
        }

        /// <summary>
        /// JSAPI/小程序下单API
        /// <seealso href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_2.shtml#top" />
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ReadyToPayForJSAPIOrMiniProgramResult> ReadyToPayForJSAPIOrMiniProgramAsync(ReadyToPayForJSAPIOrMiniProgramInput input)
        {
            /*
             * 大部分情况调用方提供的参数都是正确的，所以参数检查大部分情况下是浪费
             * 即使参数有问题，微信那么也会检查后告诉我们
             * 因此我们这里省略参数验证
             */

            //微信支付模块内部赋值
            input.mchid = option.Mchid;
            if (input.time_expire == default)
                input.time_expire = (await clock.GetNowAsync()).AddMinutes(5);//默认过期时间，可以考虑做成配置
            input.notify_url = webEnvironment.RootUrl + WXPayConst.PayNotifyUrl;

            //调用微信支付平台api并返回结果
            var response = await wxClientFactory.CreateClientPay().PostAsJsonAsync("pay/transactions/jsapi", input);
            return await response.Content.ReadAsAsync<ReadyToPayForJSAPIOrMiniProgramResult>();
        }
    }
}
