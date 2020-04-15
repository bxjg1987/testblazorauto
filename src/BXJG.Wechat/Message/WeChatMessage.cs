using BXJG.WeChat.Common;
using BXJG.WeChat.MiniProgram;
using BXJG.WeChat.Payment;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Message
{
    public class WeChatMessage
    {
        private readonly MiniProgramAuthenticationOptions _authOptions;
        private readonly IHttpClientFactory _clientFactory;
        public WeChatMessage(
            IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
            IHttpClientFactory clientFactory)
        {
            _authOptions = authOptions.CurrentValue;
            _clientFactory = clientFactory;

        }

        /// <summary>
        /// 微信小程序发送统一服务消息
        /// 参考微信统一服务消息接口：https://developers.weixin.qq.com/miniprogram/dev/api-backend/open-api/uniform-message/uniformMessage.send.html
        /// </summary>
        /// <param name="touseropenid"></param>
        /// <param name="accesstoken"></param>
        /// <param name="weappmsg"></param>
        /// <returns></returns>

        public async Task<WechatResult> SendAsync(string touseropenid, string accesstoken, weapp_template_msg weappmsg)
        {
            //var requestUrl = QueryHelpers.AddQueryString(WeChatMessageConsts.WechatSendMessageUrl, new Dictionary<string, string>
            //{
            //    { "access_token", "" },
            //});

            var client = _clientFactory.CreateClient();

            ///构建微信小程序发送消息所需要的参数
            var context = new StringContent(JsonConvert.SerializeObject(new
            {
                access_token = accesstoken,
                touser = touseropenid,
                weapp_template_msg = weappmsg
            }), Encoding.UTF8, "application/json");

            ///调用微信发用消息接口
            var response = await client.PostAsync(WeChatMessageConsts.WechatSendMessageUrl, context);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"调用微信统一发送消息接口异常 ({response.StatusCode}).请检查.");
            ///序列化微信接口返回值
            var result = JsonConvert.DeserializeObject<WechatResult>(await response.Content.ReadAsStringAsync());

            return new WechatResult() { errcode = result.errcode, errmsg = result.errmsg };

        }

    }
}
