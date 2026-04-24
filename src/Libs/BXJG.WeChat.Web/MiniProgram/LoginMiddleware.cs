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
using Microsoft.AspNetCore.WebUtilities;
using BXJG.WeChat.MiniProgram;

namespace BXJG.WeChat.Web.MiniProgram
{
    public class LoginMiddleware
    {
        /// <summary>
        /// 微信小程序模块选项监控器
        /// </summary>
        private readonly IOptionsMonitor<Option> option;
        private readonly RequestDelegate next;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly MiniProgramApiService miniProgramApiService;

        public LoginMiddleware(RequestDelegate next, IOptionsMonitor<Option> option, IHttpClientFactory httpClientFactory, MiniProgramApiService miniProgramApiService)
        {
            this.next = next;
            this.httpClientFactory = httpClientFactory;
            this.option = option;
            this.miniProgramApiService = miniProgramApiService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            //1、若没有匹配上则直接执行下一个中间件
            if (!request.Path.Value.Equals(Const.LoginEndpoint, StringComparison.OrdinalIgnoreCase))
            {
                await next(context);
                return;
            }
            

            Input input;
            using (var sr = new StreamReader(request.Body))
            {
                var str =await sr.ReadToEndAsync();
                input = System.Text.Json.JsonSerializer.Deserialize<Input>(str);
            }
            var token = await miniProgramApiService.Code2Session(input.code,context.RequestAborted);
            var handler = context.RequestServices.GetService<ILoginHandler>();
            await handler.LoginAsync(new LoginContext { Context = context, Option = option.CurrentValue, Token = token });
        }
    }
}
