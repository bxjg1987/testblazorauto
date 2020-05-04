using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前台顾客发起订单支付时提供的数据
    /// </summary>
    public class CustomerPaymentInput
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public long OrderId { get; set; }
    }
}
