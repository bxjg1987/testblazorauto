using BXJG.WeChat.MiniProgram;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WeChat.Payment
{
    public class WeChatPaymentService
    {
        private readonly MiniProgramAuthenticationOptions authOptions;
        private readonly WeChatPaymentOptions paymentOptions;
        //private readonly WeChatPaymentSecuret securet;

        public WeChatPaymentService(
              IOptionsMonitor<WeChatPaymentOptions> paymentOptions,
              IOptionsMonitor<MiniProgramAuthenticationOptions> authOptions)
        {
            this.paymentOptions = paymentOptions.CurrentValue;
            this.authOptions = authOptions.CurrentValue;
        }

        //public Task<WeChatPaymentUnifyOrderResult> PayAsync(WeChatPaymentUnifyOrderInput data) 
        //{

        //}
    }
}
