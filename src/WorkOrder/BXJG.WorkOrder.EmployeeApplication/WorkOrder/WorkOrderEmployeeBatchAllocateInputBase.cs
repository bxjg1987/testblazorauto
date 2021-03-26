using BXJG.Common.Dto;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 批量分配的输入模型
    /// </summary>
    public class WorkOrderEmployeeBatchAllocateInputBase: BatchOperationInputLong
    {
        //public string EmployeeId { get; set; }
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
    }
}