using BXJG.WeChat.MiniProgram;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BXJG.WeChat.Common
{
    public class AccessTokenProvider
    {
        //private readonly MiniProgramAuthenticationOptions options;

        //private readonly IHttpClientFactory httpClientFactory;
        //private readonly ILogger logger;

        public string accessToken { get; private set; }
        private int expire_in;
        private DateTimeOffset lastUpdate = DateTimeOffset.Now.AddDays(-1);

        const int delay = 10;//提前10秒去刷新accessToken

        public AccessTokenProvider(IOptionsMonitor<MiniProgramAuthenticationOptions> options, IHttpClientFactory httpClientFactory, ILogger logger)
        {
            options.OnChange((opt,str)=> {
                lastUpdate = DateTimeOffset.Now.AddDays(-1);
            });
            //this.options = options.CurrentValue;
            //this.httpClientFactory = httpClientFactory;
            //this.logger = logger;
           
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(delay * 1000);
                    if (!string.IsNullOrWhiteSpace(accessToken) && (DateTimeOffset.Now - lastUpdate).TotalSeconds < 7200- delay)
                        break;

                    try
                    {

                        //grant_type=client_credential&appid=APPID&secret=APPSECRET
                        var requestUrl = QueryHelpers.AddQueryString(Consts.AccessTokenUrl, new Dictionary<string, string>
                        {
                            { "grant_type", "client_credential" },
                            { "appid", options.CurrentValue.AppId },
                            { "secret", options.CurrentValue.Secret }
                        });

                        var client = httpClientFactory.CreateClient(Consts.WeChatMiniProgramHttpClientName);

                        var response = await client.GetAsync(requestUrl);
                        var msg = await response.Content.ReadAsStringAsync();
                        var result = JsonSerializer.Deserialize<AccessTokenResult>(msg);
                        this.accessToken = result.access_token;
                        this.expire_in = result.expires_in;
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "微信小程序刷新accessToken失败！");
                    }
                }
            });
        }
    }
}
