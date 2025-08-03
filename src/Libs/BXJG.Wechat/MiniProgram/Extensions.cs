using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.MiniProgram
{
    public static class Extensions
    {
        //public static HttpClient CreateClientMiniProgram(this IHttpClientFactory httpClientFactory)
        //{
        //    return httpClientFactory.CreateClient(Const.HttpClientName);
        //}
        public static IServiceCollection AddWXMiniProgramHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient<MiniProgramApiService>( c =>
            {
                c.BaseAddress = new Uri(Const.HttpClientBaseAddress);
            });
            services.AddTransient<MiniProgramApiService>();
            return services;
        }
        //目前没有服务需要注册
        //Option对象使用原生方法就可以了
        //public static IServiceCollection AddWeChatMiniProgram(this IServiceCollection services)
        //{

        //}
    }
}
