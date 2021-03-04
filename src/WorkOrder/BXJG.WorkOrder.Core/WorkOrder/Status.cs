using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 工单状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 待确认
        /// </summary>
        ToBeConfirmed,
        /// <summary>
        /// 待执行
        /// </summary>
        ToBeProcessed,
        /// <summary>
        /// 执行中
        /// </summary>
        Processing ,
        /// <summary>
        /// 已拒绝
        /// </summary>
        Rejected,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed
    }
}
