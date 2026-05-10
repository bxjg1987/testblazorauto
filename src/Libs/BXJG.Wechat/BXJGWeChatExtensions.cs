using BXJG.Common;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BXJG.WeChat
{
    public static class BXJGWeChatExtensions
    {
        /// <summary>
        /// 当前模块无需额外 DI 注册，此方法保留为扩展点。
        /// 微信支付相关服务由 BXJG.WeChat 模块的 AbpModule 自动注册。
        /// </summary>
        public static IServiceCollection AddBXJGWeChat(this IServiceCollection services)
        {
            return services;
        }
    }
}
