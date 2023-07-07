using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    //某些逻辑中将其转换为int后比较大小，因此写明枚举值
    /// <summary>
    /// 工单状态
    /// </summary>
    [Flags]
    public enum Status
    {
        /// <summary>
        /// 待确认
        /// </summary>
        ToBeConfirmed = 0,
        /// <summary>
        /// 已确认，待分配
        /// </summary>
        ToBeAllocated = 1,
        /// <summary>
        /// 已分配，待执行
        /// </summary>
        ToBeProcessed = 2,
        /// <summary>
        /// 执行中
        /// </summary>
        Processing = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 4,
        /// <summary>
        /// 已拒绝
        /// </summary>
        Rejected = 5
    }
}
