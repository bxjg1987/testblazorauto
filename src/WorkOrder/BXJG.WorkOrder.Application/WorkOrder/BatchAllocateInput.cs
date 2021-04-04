using BXJG.Common.Dto;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 批量分配的输入模型
    /// </summary>
    public class WorkOrderBatchAllocateInputBase: BatchOperationInputLong
    {
        /// <summary>
        /// 调整状态的时间
        /// </summary>
        public DateTimeOffset? StatusChangedTime { get; set; }
        public string EmployeeId { get; set; }
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
    }
}