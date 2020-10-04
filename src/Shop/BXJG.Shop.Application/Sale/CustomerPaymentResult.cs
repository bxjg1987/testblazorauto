using BXJG.WeChat.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前端顾客发起支付订单预支付的返回数据
    /// </summary>
    public class CustomerPaymentResult
    {
        /// <summary>
        /// 
        /// </summary>
        public WeChatPaymentUnifyOrderResult WeChatPaymentUnifyOrderResult { get;  }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatPaymentUnifyOrderResult"></param>
        public CustomerPaymentResult(WeChatPaymentUnifyOrderResult weChatPaymentUnifyOrderResult)
        {
            this.WeChatPaymentUnifyOrderResult = weChatPaymentUnifyOrderResult;
        }
    }
}
