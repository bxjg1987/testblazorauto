using BXJG.Common.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    /// <summary>
    /// 工单处理人批量执行工单的输入模型
    /// </summary>
    public class EmployeeBatchExecuteWorkOrderInputBase : BatchOperationInputLong
    {
        /// <summary>
        /// 状态改变说明或备注信息
        /// </summary>
        [StringLength(CoreConsts.OrderStatusChangedDescriptionMaxLength)]
        public string StatusChangedDescription { get; set; }
    }
}