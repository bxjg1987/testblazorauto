using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 订单开票类型
    /// </summary>
    public enum InvoiceType
    {
        /// <summary>
        /// 不开票
        /// </summary>
        None,
        /// <summary>
        /// 个人
        /// </summary>
        Personal,
        /// <summary>
        /// 企业
        /// </summary>
        Business
    }
}
