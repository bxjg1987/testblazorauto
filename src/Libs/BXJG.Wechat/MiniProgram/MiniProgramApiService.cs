using BXJG.Common.Sundries;
using BXJG.WeChat.Pay;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.WeChat.MiniProgram
{
    public class MiniProgramApiService
    {
        /// <summary>
        /// 微信小程序模块选项监控器
        /// </summary>
        IOptionsMonitor<Option> option;
        HttpClient httpClient;
        public MiniProgramApiService(IOptionsMonitor<Option> opt, HttpClient httpClient)
        {
            this.option = opt;
            this.httpClient = httpClient;
        }

        public async Task<LoginResult> Code2Session(string code, CancellationToken ct = default)
        {
            var requestUrl = Helpers.AddQueryString(Const.OpenIdEndpoint, new Dictionary<string, string>
            {
                { "appid", option.CurrentValue.AppId },
                { "secret", option.CurrentValue.AppSecret },
                { "js_code",code },
                { "grant_type", "authorization_code" },
            });
            var response = await httpClient.GetStringAsync(requestUrl);
            return JsonSerializer.Deserialize<LoginResult>(response);
        }
    }
}
