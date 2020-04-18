using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
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
