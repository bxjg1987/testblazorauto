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
        Option option;
        //IHttpClientFactory httpClientFactory;
        HttpClient httpClient;
        public MiniProgramApiService(IOptionsSnapshot<Option> opt,/* IHttpClientFactory httpClientFactory,*/ HttpClient httpClient)
        {
            this.option = opt.Value;
            //this.httpClientFactory = httpClientFactory;
            this.httpClient = httpClient;
        }

        public async Task<LoginResult> Code2Session(string code, CancellationToken ct = default)
        {
            var requestUrl = Helpers.AddQueryString(Const.OpenIdEndpoint, new Dictionary<string, string>
            {
                { "appid", option.AppId },
                { "secret", option.AppSecret },
                { "js_code",code },
                { "grant_type", "authorization_code" },
            });
            var response = await httpClient.GetStringAsync(requestUrl);
            return JsonSerializer.Deserialize<LoginResult>(response);
        }
    }
}
