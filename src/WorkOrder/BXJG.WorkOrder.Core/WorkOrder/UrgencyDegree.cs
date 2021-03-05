using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    //怕将来改代码 移动顺序，因此值定义死
    /// <summary>
    /// 紧急程度
    /// </summary>
    public enum UrgencyDegree
    {
        /// <summary>
        /// 紧急的
        /// </summary>
        Urgent = 0,
        /// <summary>
        /// 普通的
        /// </summary>
        Normalize = 1,
        /// <summary>
        /// 可有可无的
        /// </summary>
        Dispensable = 2
    }
}
