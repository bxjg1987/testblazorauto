using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WeChat.Common
{
    /// <summary>
    /// 注册微信小程序需要的公共服务
    /// </summary>
    public static class WeChatCommonExtensions
    {
        public static IServiceCollection AddWeChartMiniProgram(this IServiceCollection services)
        {
            services.AddHttpClient(Consts.WeChatMiniProgramHttpClientName, client =>
            {
                //微信小程序支付内部通过此httpClient来发起请求，这里可以统一对client进行配置
            });
            return services.AddSingleton<AccessTokenProvider>();
        }
    }
}
