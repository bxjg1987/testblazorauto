using BXJG.WeChat.Pay.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BXJG.WeChat.Pay;
namespace BXJG.WeChat.Web.Pay
{
    /// <summary>
    /// 微信支付结果通知owin中间件<br/>
    /// 拦截请求 -> <see cref="WXSignValidator">验签</see>（判断请求是否确实来自微信支付服务器）-> <see cref="SecretHelper">解密</see> -> 回调你的<see cref="IPayNotifyHandler">处理器</see> -> 响应微信<br/>
    /// 参考文档：<seealso cref="" href="https://pay.weixin.qq.com/wiki/doc/apiv3/wxpay/pay/transactions/chapter3_11.shtml#top" />
    /// </summary>
    public class PayNotifyMiddleware
    {
        /// <summary>
        /// 微信支付模块选项监控器
        /// </summary>
        private readonly IOptionsMonitor<Option> option;
        private readonly SecretHelper secretHelper;
        private readonly RequestDelegate next;

        public PayNotifyMiddleware(RequestDelegate next,
                                   IOptionsMonitor<Option> option,
                                   SecretHelper secretHelper)
        {

            this.next = next;
            this.secretHelper = secretHelper;
            this.option = option;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            //1、若没有匹配上则直接执行下一个中间件
            if (!request.Path.Value.Equals(Const.PayNotifyUrl, StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }

            var cancellationToken = context.RequestAborted;

            //2、验签，确定请求一定是微信服务器发送来的
            var ps = request.Headers.ToDictionary(c => c.Key, c => c.Value.FirstOrDefault() ?? string.Empty, StringComparer.OrdinalIgnoreCase);
            string body;
            using (var sr = new StreamReader(request.Body))
            {
                body = await sr.ReadToEndAsync();
            }
            //var json = await requestContent.ReadAsAsync<PayNotifyResult>(cancellationToken);
            var isSign = await secretHelper.VerifyAsync(ps, body, cancellationToken);
            if (!isSign)
            {
                // 验签失败属于异常请求（非微信服务器发送），throw返回500属于5XX范围，微信会重发通知，不会造成业务数据丢失。
                // 虽然响应格式不完全符合微信V3文档规范（应返回{"code":"FAIL","message":"..."}的JSON），但实际不影响功能，暂不处理。
                throw new Exception("验签失败！");
            }

            //3、解密得到通知的数据
            var json = System.Text.Json.JsonSerializer.Deserialize<PayNotifyResult>(body);
            //忽略部分验证
            var resource = secretHelper.AesGcmDecrypt(json.resource.associated_data, json.resource.nonce, json.resource.ciphertext);
            var json2 = System.Text.Json.JsonSerializer.Deserialize<PayNotifySuccessResult>(resource);

            //4、回调
            //如果将来需要，这里可以换成可替换的工厂
            await (context.RequestServices.GetRequiredService<IPayNotifyHandler>()).PrecessAsync(json2, cancellationToken);

            //5、响应
            await context.Response.WriteAsync(StateDto.SuccessJsonString/*, cancellationToken*/);//传入cancellationToken也许是多此一举，如果response够聪明的话，关联的context已经有cancelToken了
        }
    }
}
