using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        WaitingForPayment,
        /// <summary>
        /// 已支付
        /// </summary>
        Paid,
        /// <summary>
        /// 申请退款
        /// </summary>
        ApplyForRefund,
        /// <summary>
        /// 已全额退款
        /// </summary>
        Refunded,
        /// <summary>
        /// 已部分退款
        /// </summary>
        RefundedInPart
    }
}
