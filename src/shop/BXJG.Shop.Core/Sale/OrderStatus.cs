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
    /// <summary>
    /// 物流状态
    /// </summary>
    public enum LogisticsStatus
    {
        /// <summary>
        /// 待发货
        /// </summary>
        WaitShip,
        /// <summary>
        /// 已发货
        /// </summary>
        Shipped,
        /// <summary>
        /// 已签收
        /// </summary>
        Signed,
        /// <summary>
        /// 已拒收
        /// </summary>
        Rejected,
        /// <summary>
        /// 不需要运送
        /// </summary>
        Unwanted
    }
}
