using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 紧急程度
    /// </summary>
    public enum UrgencyDegree
    {
        /// <summary>
        /// 紧急的
        /// </summary>
        Urgent,
        /// <summary>
        /// 普通的
        /// </summary>
        General,
        /// <summary>
        /// 可有可无的
        /// </summary>
        Dispensable
    }
}
