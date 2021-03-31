using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 状态批量调整的输入模型
    /// </summary>
    public class WorkOrderBatchChangeStatusInputBase: BatchOperationInputLong
    {
        [Required]
        public Status Status { get; set; }
        /// <summary>
        /// 状态变更的说明
        /// </summary>
        public string Description { get; set; }
    }
}
