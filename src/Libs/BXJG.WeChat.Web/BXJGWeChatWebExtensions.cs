using BXJG.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BXJG.WeChat.Web
{
    public static class BXJGWeChatWebExtensions
    {
        /// <summary>
        /// 当前模块无需额外 DI 注册，此方法保留为扩展点。
        /// 微信支付Web相关服务由 BXJG.WeChat.Web 模块的 AbpModule 自动注册。
        /// </summary>
        public static IServiceCollection AddBXJGWeChatWeb(this IServiceCollection services)
        {
            return services;
        }
    }
}
