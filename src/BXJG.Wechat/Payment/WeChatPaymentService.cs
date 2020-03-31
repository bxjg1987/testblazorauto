using BXJG.WeChat.MiniProgram;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static BXJG.WeChat.Payment.WeChatPaymentUnifyOrderResult;

namespace BXJG.WeChat.Payment
{
    public class WeChatPaymentService
    {
        private readonly MiniProgramAuthenticationOptions authOptions;
        private readonly WeChatPaymentOptions paymentOptions;
        private readonly WeChatPaymentUnifyOrderResultFactory weChatPaymentUnifyOrderResultFactory;
        //按asp.net core 身份验证系统的设计，它的OAuth2的方式会允许Options对象来提供HttpClient，我们这里为了简单直接注入IHttpClientFactory
        //参考官网HttpClientFacotry
        private readonly IHttpClientFactory clientFactory;

        public WeChatPaymentService(
              IOptionsMonitor<WeChatPaymentOptions> paymentOptions,
              IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions,
               WeChatPaymentUnifyOrderResultFactory weChatPaymentUnifyOrderResultFactory,
              IHttpClientFactory clientFactory)
        {
            this.paymentOptions = paymentOptions.CurrentValue;
            this.authOptions = authOptions.CurrentValue;
            this.weChatPaymentUnifyOrderResultFactory = weChatPaymentUnifyOrderResultFactory;
            this.clientFactory = clientFactory;
        }

        public async Task<WeChatPaymentUnifyOrderResult> PayAsync(WeChatPaymentUnifyOrderInput data, System.Threading.CancellationToken cancellationToken = default)
        {
            var client = clientFactory.CreateClient(WeChatPaymentConsts.HttpClientName);
            var str = data.ToXml();
            var ct = new StringContent(str);
            var response = await client.PostAsync(paymentOptions.UnifyOrderUrl, ct, cancellationToken);
            return await weChatPaymentUnifyOrderResultFactory.LoadAsync(await response.Content.ReadAsStreamAsync(), cancellationToken);
        }
    }
}
