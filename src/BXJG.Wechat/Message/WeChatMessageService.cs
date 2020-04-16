using BXJG.WeChat.Common;
using BXJG.WeChat.MiniProgram;
using BXJG.WeChat.Payment;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static BXJG.WeChat.Common.Enums;

namespace BXJG.WeChat.Message
{
    public class WeChatMessageService
    {
        private readonly MiniProgramAuthenticationOptions _authOptions;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AccessTokenProvider _accessTokenProvider;

        public WeChatMessageService(
            IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
            IHttpClientFactory clientFactory,
            AccessTokenProvider accessTokenProvider)
        {
            _authOptions = authOptions.CurrentValue;
            _clientFactory = clientFactory;
            _accessTokenProvider = accessTokenProvider;

        }

        /// <summary>
        /// 微信小程序发送订阅消息
        /// </summary>
        /// <param name="touser">openid</param>
        /// <param name="template_id">模板Id</param>
        /// <param name="data">模板变量值</param>
        /// <param name="page">跳转页面地址</param>
        /// <param name="miniprogram_state">小程序版本</param>
        /// <param name="lang">语言</param>
        /// <returns></returns>
        public async Task<WechatResult> SendSubscriptMsgAsync(string touser, string template_id, IDictionary<string, TemplateDataItem> data, string page="", miniprogram_state miniprogram_state = miniprogram_state.formal,lang lang= lang.zh_CN )
        {
            var access_token = this._accessTokenProvider.accessToken;

            var client = _clientFactory.CreateClient(Common.Consts.WeChatMiniProgramHttpClientName);

            //dynamic data = new ExpandoObject() as IDictionary<string, object>;
            //foreach (var item in dt)
            //{
            //    data[item.Key] = item.Value;
            //}

            

            ///构建微信小程序发送订阅消息所需要的参数
            var context = new StringContent(System.Text.Json.JsonSerializer.Serialize(new
            {
                access_token,
                touser,
                template_id,
                miniprogram_state,
                lang,
                page,
                data
            }).Replace('[','}').Replace(']','}'), Encoding.UTF8, "application/json");

            ///调用微信发订阅消息接口
            var response = await client.PostAsync(WeChatMessageConsts.WechatSendMessageUrl, context);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"调用微信统一发送消息接口异常 ({response.StatusCode}).请检查.");
            ///序列化微信接口返回值
            return System.Text.Json.JsonSerializer.Deserialize<WechatResult>(await response.Content.ReadAsStringAsync());

            //return new WechatResult() { errcode = result.errcode, errmsg = result.errmsg };

        }

    }
}
