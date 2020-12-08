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
        /// 在此之前请确保BXJG.Common中的服务已注册<br/>
        /// 当集成到abp时建议不用调用此方法，而是通过abp的模块化来注册相关服务，但任然需要调用AddWXPayHttpClient注册微信支付内部使用的HttpClient
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWXPay(this IServiceCollection services)
        {
            return services.AddSingleton<SecretHelper>()
                           .AddSingleton<IWXCertificateProvider, WXCertificateDefaultProvider>()
                           .AddSingleton<PayServiceV3>();
        }
        /// <summary>
        /// 注册微信支付模块内部使用的HttpClient
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWXPayHttpClient(this IServiceCollection services)
        {
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
            return services.AddBXJGCommon().AddWXPay().AddWXPayHttpClient();
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
