using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Pay
{
    public static class WXPayExtensions
    {
        /// <summary>
        /// 注册微信支付相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWXPay(this IServiceCollection services)
        {
            services.AddHttpClient(WXPayConst.HttpClientKey, c => {
                c.BaseAddress = new Uri("https://api.mch.weixin.qq.com/v3/");
            }).AddHttpMessageHandler<WXSignDelegatingHandler>();
            return services;
        }
    }
}
