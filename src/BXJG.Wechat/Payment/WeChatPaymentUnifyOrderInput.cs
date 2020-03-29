using BXJG.Utils;
using BXJG.WeChat.MiniProgram;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 统一下单，要提交给微信的数据模型；数据结构与类型完全与官方保持一致，不做任何转换
    /// https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=9_1&index=1
    /// </summary>
    public class WeChatPaymentUnifyOrderInput//: IPostToWeChat
    {
        public string appid { get; private set; }
        public string mch_id { get; private set; }
        public string device_info { get; set; }
        public string nonce_str { get; private set; }
        public string sign { get; private set; }
        public string sign_type { get; private set; } = "MD5";//目前写死 只能md5
        public string body { get; set; }
        public string detail { get; private set; }
        public string attach { get; set; }
        public string out_trade_no { get; set; }
        public string fee_type { get; set; } = "CNY";//目前写死
        public int total_fee { get; set; }
        public string spbill_create_ip { get; private set; }
        public string time_start { get; set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        public string time_expire { get; set; } = DateTime.Now.AddMinutes(2).ToString("yyyyMMddHHmmss");
        public string goods_tag { get; set; }
        public string notify_url { get; set; }
        public string trade_type { get; set; } = "JSAPI";
        public string product_id { get; set; }
        public string limit_pay { get; set; }
        public string openid { get; set; }
        public string receipt { get; set; }
        public string scene_info { get; set; }

        MiniProgramAuthenticationOptions authOptions;
        WeChatPaymentOptions paymentOptions;
        WeChatPaymentSecuret securet;

        public WeChatPaymentUnifyOrderInput(
            WeChatPaymentOptions options,
            MiniProgramAuthenticationOptions options1, 
            WeChatPaymentSecuret securet, 
            string openid,
            string body,
            string out_trade_no,
            int total_fee)
        {
            paymentOptions = options;
            authOptions = options1;
            this.securet = securet; 
            
            this.mch_id = paymentOptions.mch_id;
            ResetNonce();
            this.openid = openid;
            this.body = body;
            this.out_trade_no = out_trade_no;
            this.total_fee = total_fee;
            this.spbill_create_ip = options.ip;//先这么来，以后可能想个法获取服务器的外网ip
            this.notify_url = paymentOptions.notify_url;
            this.appid = authOptions.AppId;
        }

        /// <summary>
        /// 重置随机字符串
        /// </summary>
        public string ResetNonce(int k =12)
        {
            nonce_str = RandomHelper.RandomBase64(k);
            return nonce_str;
        }
        /// <summary>
        /// 按微信规定的方式计算签名
        /// </summary>
        /// <returns></returns>
        public string ComputationalSignature() {
            sign= securet.sign(this);
            return sign;
        }
        /// <summary>
        /// 设置单品优惠
        /// </summary>
        public void SetDetail() {
            throw new NotImplementedException();
        }
    }
}
