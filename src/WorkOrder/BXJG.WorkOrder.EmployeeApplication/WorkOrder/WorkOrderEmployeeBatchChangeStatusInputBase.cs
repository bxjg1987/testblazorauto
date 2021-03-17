using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 状态批量调整的输入模型
    /// </summary>
    public class WorkOrderEmployeeBatchChangeStatusInputBase: BatchOperationInputLong
    {
        /// <summary>
        /// 状态变更的说明
        /// </summary>
        public string Description { get; set; }
    }
}
