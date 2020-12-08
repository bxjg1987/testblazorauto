using BXJG.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Pay
{
    public static class WXPayExtensions
    {
        /// <summary>
        /// 注册微信支付相关服务<br/>
        /// 在此之前请确保BXJG.Common中的服务已注册
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWXPay(this IServiceCollection services)
        {
            services.AddSingleton<SecretHelper>();
            services.AddHttpClient(WXPayConst.HttpClientKey, c => {
                c.BaseAddress = new Uri("https://api.mch.weixin.qq.com/v3/");
            }).AddHttpMessageHandler<WXSignDelegatingHandler>();
            return services;
        }
        /// <summary>
        /// 注册微信支付及其依赖的相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWXPayFull(this IServiceCollection services)
        {
            return services.AddBXJGCommon().AddWXPay();
        }
        /// <summary>
        /// 创建微信支付模块使用的httpClient
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <returns></returns>
        internal static HttpClient CreateClientPay(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(WXPayConst.HttpClientKey);
        }
    }
}
