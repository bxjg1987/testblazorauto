using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 微信小程序支付相关的选项对象
    /// </summary>
    public class WeChatPaymentOptions
    {
        /// <summary>
        /// 商户id
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// http://xxxx.com/wechat-miniprogram-payment-notify
        /// </summary>
        public string notify_url { get; set; }
    }
}
