using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BXJG.WeChat.Payment
{
    public static class WeChatPaymentExtensions
    {
        #region 注册服务和中间件
        /// <summary>
        /// 向中间件管道注册微信支付结果通知中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeChatPayment(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WeChatPaymentNoticeMiddleware>();
        }
        /// <summary>
        /// 向依赖注入容器添加支付功能需要的相关服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static IServiceCollection AddWeChatPayment(this IServiceCollection services, Action<WeChatPaymentOptions> act)
        {
            return services.AddWeChatPaymentCore().Configure(act);
        }
        public static IServiceCollection AddWeChatPayment(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddWeChatPaymentCore().Configure<WeChatPaymentOptions>(configuration);
        }
        private static IServiceCollection AddWeChatPaymentCore(this IServiceCollection services)
        {
            return services
                    .AddScoped<WeChatPaymentSecuret>()
                    .AddScoped<WeChatPaymentUnifyOrderResult.WeChatPaymentUnifyOrderResultFactory>();
        }
        #endregion

        //public static Task ResponseWeChatAsync(this HttpContext context, string code, string msg = "")
        //{
        //    //将来可以考虑封装。
        //    //定义IWeChatResponseMessage 里面定义ToXML扩展方法，不同的消息有不同实现
        //    //可以进一步为HttpContext和HttpRequest提供扩展方法，简化调用
        //    //因为目前对微信整体开发了解不全面，不要盲目封装
        //    //var content = $@"<xml>
        //    //                    <return_code><![CDATA[{code}]]></return_code>
        //    //                    <return_msg><![CDATA[{msg}]]></return_msg>
        //    //                </xml>";
        //    //await context.Response.WriteAsync(content, context.RequestAborted);

        //    return context.Response.ResponseWeChatAsync(code, msg);
        //}
        public static async Task ResponseWeChatAsync(this HttpResponse response, string code, string msg = "")
        {
            //将来可以考虑封装。
            //定义IWeChatResponseMessage 里面定义ToXML扩展方法，不同的消息有不同实现
            //可以进一步为HttpContext和HttpRequest提供扩展方法，简化调用
            //因为目前对微信整体开发了解不全面，不要盲目封装
            var content = $@"<xml>
                                <return_code><![CDATA[{code}]]></return_code>
                                <return_msg><![CDATA[{msg}]]></return_msg>
                            </xml>";
            await response.WriteAsync(content, response.HttpContext.RequestAborted);
        }
        public static Task ResponseWeChatSuccessAsync(this HttpResponse response)
        {
            return response.ResponseWeChatAsync(WeChatPaymentConsts.SUCCESS);
        }
        public static Task ResponseWeChatFailAsync(this HttpResponse response, string msg)
        {
            return response.ResponseWeChatAsync(WeChatPaymentConsts.FAIL);
        }
    }
}
