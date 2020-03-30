using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 参考官方文档：https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=9_1&index=1
    /// 微信返回的信息，没必要允许修改，所以全是只读的
    /// </summary>
    public class WeChatPaymentUnifyOrderResult : INeedSign
    {
        public string return_code { get; private set; }
        public string return_msg { get; private set; }

        public string appid { get; private set; }
        public string mch_id { get; private set; }
        public string device_info { get; private set; }
        public string nonce_str { get; private set; }
        public string sign { get; private set; }
        public string result_code { get; private set; }
        public string err_code { get; private set; }
        public string err_code_des { get; private set; }

        public string trade_type { get; private set; }
        public string prepay_id { get; private set; }
        public string code_url { get; private set; }

        //构造函数无法异步，所以从Strem初始化实例就不放构造函数了
        private WeChatPaymentUnifyOrderResult() { }

        //简单起见，用个静态方法吧。高级点是单独定义个类 WeChatPaymentSecuret就可以直接注入了
        //但是那种方式 又无法访问WeChatPaymentUnifyOrderResult私有构造函数了
        //试试内部类呢？因为对于调用方来说 屏蔽WeChatPaymentSecuret是一个不错的选择
        /*public static async Task<WeChatPaymentUnifyOrderResult> LoadAsync(Stream stream, WeChatPaymentSecuret securet, CancellationToken cancellation = default)
        {
            var xd = await XDocument.LoadAsync(stream, LoadOptions.None, cancellation);
            var c = xd.Root;

            var p = new WeChatPaymentUnifyOrderResult();

            p.return_code = (string)c.Element("return_code");
            p.return_msg = (string)c.Element("return_msg");
            if (p.return_code != "SUCCESS")
                return p;

            p.appid = (string)c.Element("appid");
            p.mch_id = (string)c.Element("mch_id");
            p.device_info = (string)c.Element("device_info");
            p.nonce_str = (string)c.Element("nonce_str");
            p.sign = (string)c.Element("sign");
            p.result_code = (string)c.Element("result_code");//最好搞成枚举
            p.err_code = (string)c.Element("err_code");
            p.err_code_des = (string)c.Element("err_code_des");
            if (p.result_code != "SUCCESS")
                return p;

            p.trade_type = (string)c.Element("trade_type");
            p.prepay_id = (string)c.Element("prepay_id");
            p.code_url = (string)c.Element("code_url");



            return p;
        }
        */

        //因为依赖了WeChatPaymentSecuret  而它又要求一次请求一个实例，因此此类也有这种要求
        public class WeChatPaymentUnifyOrderResultFactory
        {
            WeChatPaymentSecuret securet;
            public WeChatPaymentUnifyOrderResultFactory(WeChatPaymentSecuret securet)
            {
                this.securet = securet;
            }

            public async Task<WeChatPaymentUnifyOrderResult> LoadAsync(Stream stream, CancellationToken cancellation = default)
            {
                var xd = await XDocument.LoadAsync(stream, LoadOptions.None, cancellation);
                var c = xd.Root;

                var p = new WeChatPaymentUnifyOrderResult();

                p.return_code = (string)c.Element("return_code");
                p.return_msg = (string)c.Element("return_msg");
                if (p.return_code != "SUCCESS")
                    return p;

                p.appid = (string)c.Element("appid");
                p.mch_id = (string)c.Element("mch_id");
                p.device_info = (string)c.Element("device_info");
                p.nonce_str = (string)c.Element("nonce_str");
                p.sign = (string)c.Element("sign");
                p.result_code = (string)c.Element("result_code");//最好搞成枚举
                p.err_code = (string)c.Element("err_code");
                p.err_code_des = (string)c.Element("err_code_des");
                if (p.result_code != "SUCCESS")
                    return p;

                p.trade_type = (string)c.Element("trade_type");
                p.prepay_id = (string)c.Element("prepay_id");
                p.code_url = (string)c.Element("code_url");

                if (!securet.CheckSign(p))
                    throw new Exception();//这里最好返回一个微信小程序支付 特有的异常对象

                return p;
            }
        }
    }
}