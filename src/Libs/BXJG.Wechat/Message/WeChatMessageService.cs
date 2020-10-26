using BXJG.WeChat.Common;
using BXJG.WeChat.MiniProgram;
using BXJG.WeChat.Payment;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using static BXJG.WeChat.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BXJG.WeChat.Message
{
    public class WeChatMessageService
    {
        private readonly MiniProgramAuthenticationOptions _authOptions;
        private readonly IHttpClientFactory _clientFactory;
        private readonly AccessTokenProvider _accessTokenProvider;

        //  private readonly WechatTemplateOptions _options;

        public WeChatMessageService(
            IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
            IHttpClientFactory clientFactory,
            AccessTokenProvider accessTokenProvider
            //IOptionsMonitor< WechatTemplateOptions> options
            )
        {
            _authOptions = authOptions.CurrentValue;
            _clientFactory = clientFactory;
            _accessTokenProvider = accessTokenProvider;
            //_options = options.CurrentValue;

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
        public async Task<WechatResult> SendSubscriptMsgAsync(string touser, string template_id, IDictionary<string, TemplateDataItem> data, string page = "", miniprogram_state miniprogram_state = miniprogram_state.formal, lang lang = lang.zh_CN)
        {
            //if (!_options.TemplateList.Contains(template_id, StringComparer.Ordinal))
            //    throw new ArgumentException("发送订阅消息失败！");

            //var user = await _userManager.FindByLoginAsync(MiniProgramConsts.AuthenticationScheme, touser);
            //var claimns = await _userManager.GetClaimsAsync(user);
            //var templateAry = System.Text.Json.JsonSerializer.Deserialize<string[]>(claimns.Single(c => c.Type == Common.Consts.TemplateMessageClaimType).Value);
            //if (!templateAry.Contains(template_id, StringComparer.OrdinalIgnoreCase))
            //    throw new ArgumentException($"发送订阅消息失败！当前模板id{template_id}用户未同意，请核对！");

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
            }).Replace('[', '}').Replace(']', '}'), Encoding.UTF8, "application/json");

            ///调用微信发订阅消息接口
            var response = await client.PostAsync(WeChatMessageConsts.WechatSendMessageUrl, context);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"调用微信统一发送消息接口异常 ({response.StatusCode}).请检查.");
            ///序列化微信接口返回值
            return System.Text.Json.JsonSerializer.Deserialize<WechatResult>(await response.Content.ReadAsStringAsync());

            //return new WechatResult() { errcode = result.errcode, errmsg = result.errmsg };

        }

        //public async Task<WechatResult> SendSubscriptMsgAsync<TKey>(TKey touser, string template_id, IDictionary<string, TemplateDataItem> data, string page = "", miniprogram_state miniprogram_state = miniprogram_state.formal, lang lang = lang.zh_CN)
        //{
        //    var identity = new ClaimsIdentity();
        //    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, touser.ToString()));

        //    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        //   // _userManager.GetUserAsync
        //}
    }
}
