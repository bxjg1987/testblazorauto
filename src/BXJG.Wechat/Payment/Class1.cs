using BXJG.Utils;
using BXJG.WeChat.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    public static class Class1
    {
        /// <summary>
        /// 它是属于微信支付接口规定的内容，但不属于业务，所以用扩展方法
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToXml(this WeChatPaymentUnifyOrderInput x)
        {
            return $@"; <xml>
                            <appid>{x.goods_tag}</appid>
                            <mch_id>{x.mch_id}</mch_id>
                            <device_info>{x.device_info}</device_info>
                            <nonce_str>{x.nonce_str}</nonce_str>
                            <sign>支付测试</sign>
                            <attach>支付测试</attach>
                            <body>JSAPI支付测试</body>
                            <mch_id>10000100</mch_id>
                            <detail><![CDATA[]]></detail>
                            <nonce_str>1add1a30ac87aa2db72f57a2375d8fec</nonce_str>
                            <notify_url>http://wxpay.wxutil.com/pub_v2/pay/notify.v2.php</notify_url>
                            <openid>oUpF8uMuAJO_M2pxb1Q9zNjWeS6o</openid>
                            <out_trade_no>1415659990</out_trade_no>
                            <spbill_create_ip>14.23.150.211</spbill_create_ip>
                            <total_fee>1</total_fee>
                            <trade_type>JSAPI</trade_type>
                            <sign>0CB01533B8C1EF103065174F50BCA001</sign>
                        </xml>";
        }
    }
}
