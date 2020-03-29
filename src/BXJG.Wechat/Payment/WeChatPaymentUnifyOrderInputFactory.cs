using BXJG.WeChat.MiniProgram;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 统一下单时要准备提交的数据，此工作用来简化这个数据的创建过程
    /// 推荐使用依赖注入
    /// </summary>
    public class WeChatPaymentUnifyOrderInputFactory
    {
        private readonly MiniProgramAuthenticationOptions authOptions;
        private readonly WeChatPaymentOptions paymentOptions;
        private readonly WeChatPaymentSecuret securet;

        public WeChatPaymentUnifyOrderInputFactory(
              IOptionsMonitor<WeChatPaymentOptions> paymentOptions,
              IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
              WeChatPaymentSecuret securet)
        {
            this.paymentOptions = paymentOptions.CurrentValue;
            this.authOptions = authOptions.CurrentValue;
            this.securet = securet;
        }
        /// <summary>
        /// 创建统一下单时要准备提交的数据
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="body">商品简单描述，该字段请按照规范传递 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <param name="out_trade_no">商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*且在同一个商户号下唯一 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <param name="total_fee">订单总金额，单位为分 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_2 </param>
        /// <returns></returns>
        public WeChatPaymentUnifyOrderInput Create(
            string openid,
            string body,
            string out_trade_no,
            int total_fee)
        {
            return new WeChatPaymentUnifyOrderInput(paymentOptions, authOptions, securet, openid, body, out_trade_no, total_fee);
        }
    }
}
