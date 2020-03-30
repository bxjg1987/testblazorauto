using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using ZLJ.Utils.Extensions;
using Microsoft.Extensions.Options;

namespace BXJG.WeChat.Payment
{
    /// <summary>
    /// 微信小程序跟支付和安全相关的东东
    /// 配置里的key可能变动，因此不要单例，最好是一次请求一个实例 注入到ioc容器
    /// </summary>
    public class WeChatPaymentSecuret
    {
        private readonly string key;//小程序支付相关的选项对象
        public WeChatPaymentSecuret(IOptionsMonitor<WeChatPaymentOptions> o)
        {
            key = o.CurrentValue.key;
        }
        ///// <summary>
        ///// 小程序支付的签名算法 https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=4_3
        ///// </summary>
        ///// <param name="o"></param>
        //public INeedSign SetSign(INeedSign o)
        //{
        //    o.sign = GetSign(o);
        //    return o;
        //}

        public bool CheckSign(INeedSign o)
        {
            return GetSign(o) == o.sign;
        }

        public string GetSign(object o)
        {
            StringBuilder sb = new StringBuilder();

            var type = o.GetType();
            var ps = type.GetProperties().OrderBy(c => c.Name);
            foreach (var item in ps)
            {
                var val = item.GetValue(o);
                if (val != null)
                    sb.Append($"{item.Name}={val}&");
            }

            if (sb.Length == 0)
                throw new ArgumentException("微信小程序支付签名提供的参数全为空");

            sb.Append($"key={key}");
            var str = sb.ToString().TrimEnd('&');
            return str.GetMD532();
        }
    }
}
