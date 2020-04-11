using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 订单状态
    /// 订单还有物流状态和支付状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 前台用户确认订单后将生成订单，此时用户可能随时取消订单
        /// </summary>
        Created,
        /// <summary>
        /// 进行中 已付款、退款申请中、各种处理中的状态...
        /// </summary>
        Processing,
        /// <summary>
        /// 因为任何问题导致订单最终未成交则为已取消状态
        /// </summary>
        Cancelled,
        /// <summary>
        /// 正常完成的订单状态
        /// </summary>
        Completed
    }
}
