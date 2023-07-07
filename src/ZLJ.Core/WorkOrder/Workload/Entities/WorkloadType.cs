using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.Workload
{
    /// <summary>
    /// 工作量类型
    /// </summary>
    public enum WorkloadType
    {
        /// <summary>
        /// 积分模式,确认工单时必须设置工单相应的积分值
        /// </summary>
        [Description("积分模式")]
        ByPoints,
        /// <summary>
        /// 工单量模式,确认工单时不需要设置工单积分，默认1积分
        /// </summary>
        [Description("工单量模式")]
        ByWorkOrderCount

    }
}
