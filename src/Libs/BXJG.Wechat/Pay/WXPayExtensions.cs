using BXJG.Common;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddWXPay(this IServiceCollection services, Action<WXPayOption> act)
        {
            return services.AddWXPayCore().Configure(act);
        }
        public static IServiceCollection AddWXPay(this IServiceCollection services, IConfiguration config)
        {
            return services.AddWXPayCore().Configure<WXPayOption>(config);
        }
        public static IServiceCollection AddWXPayFull(this IServiceCollection services, Action<WXPayOption> act)
        {
            return services.AddBXJGCommon().AddWXPay(act).AddWXPayHttpClient();
        }
        public static IServiceCollection AddWXPayFull(this IServiceCollection services, IConfiguration act)
        {
            return services.AddBXJGCommon().AddWXPay(act).AddWXPayHttpClient();
        }

        public static IServiceCollection AddWXPayCore(this IServiceCollection services)
        {
            return services.AddSingleton<SecretHelper>()
                           .AddSingleton<IWXCertificateProvider, WXCertificateDefaultProvider>()
                           .AddTransient<PayServiceV3>();
        }
        public static IServiceCollection AddWXPayHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient(WXPayConst.HttpClientKey, c =>
            {
                c.BaseAddress = new Uri("https://api.mch.weixin.qq.com/v3/");
            }).AddHttpMessageHandler<WXSignDelegatingHandler>();
            return services;
        }
        internal static HttpClient CreateClientPay(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(WXPayConst.HttpClientKey);
        }
    }
}
