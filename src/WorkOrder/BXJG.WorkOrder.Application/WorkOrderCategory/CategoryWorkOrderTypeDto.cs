using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 分类与工单类型的多对多关系的Dto模型
    /// </summary>
    public class CategoryWorkOrderTypeDto
    {
        /// <summary>
        /// 工单类型
        /// </summary>
        public string WorkOrderType { get; set; }
        /// <summary>
        /// 工单类型的名称
        /// </summary>
        public string WorkOrderTypeDisplayName { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
