using BXJG.Common.Dto;
using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    /// <summary>
    /// 员工批量改变工单状态的输入模型
    /// </summary>
    public class WorkOrderBatchChangeStatusInputBase : BatchOperationInputLong
    {
        ///// <summary>
        ///// 操作时间
        ///// </summary>
        //public DateTimeOffset? StatusChangedTime { get; set; }
        /// <summary>
        /// 目标状态
        /// </summary>
        [Required]
        public Status Status { get; set; }
        /// <summary>
        /// 状态变更的说明
        /// </summary>
        public string Description { get; set; }
    }
}
