using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Payment
{
    public static class WeChatPaymentExtensions
    {
        //public static Task ResponseWeChatAsync(this HttpContext context, string code, string msg = "")
        //{
        //    //将来可以考虑封装。
        //    //定义IWeChatResponseMessage 里面定义ToXML扩展方法，不同的消息有不同实现
        //    //可以进一步为HttpContext和HttpRequest提供扩展方法，简化调用
        //    //因为目前对微信整体开发了解不全面，不要盲目封装
        //    //var content = $@"<xml>
        //    //                    <return_code><![CDATA[{code}]]></return_code>
        //    //                    <return_msg><![CDATA[{msg}]]></return_msg>
        //    //                </xml>";
        //    //await context.Response.WriteAsync(content, context.RequestAborted);

        //    return context.Response.ResponseWeChatAsync(code, msg);
        //}
        public static async Task ResponseWeChatAsync(this HttpResponse response, string code, string msg = "")
        {
            //将来可以考虑封装。
            //定义IWeChatResponseMessage 里面定义ToXML扩展方法，不同的消息有不同实现
            //可以进一步为HttpContext和HttpRequest提供扩展方法，简化调用
            //因为目前对微信整体开发了解不全面，不要盲目封装
            var content = $@"<xml>
                                <return_code><![CDATA[{code}]]></return_code>
                                <return_msg><![CDATA[{msg}]]></return_msg>
                            </xml>";
            await response.WriteAsync(content, response.HttpContext.RequestAborted);
        }
        public static  Task ResponseWeChatSuccessAsync(this HttpResponse response)
        {
            return response.ResponseWeChatAsync(WeChatPaymentConsts.SUCCESS);
        }

        public static Task ResponseWeChatFailAsync(this HttpResponse response,string msg)
        {
            return response.ResponseWeChatAsync(WeChatPaymentConsts.FAIL);
        }



        /// <summary>
        /// 它是属于微信支付接口规定的内容，但不属于业务，所以用扩展方法
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToXml(this WeChatPaymentUnifyOrderInput x)
        {
            //也许某些参数没有值的时候可以不传，没有测试过，如果可行 可以减少传输的数据量
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
