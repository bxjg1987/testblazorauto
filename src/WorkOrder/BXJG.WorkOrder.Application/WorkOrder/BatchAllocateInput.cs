using BXJG.Common.Dto;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    /// <summary>
    /// 后台管理批量分配工单的输入模型基类
    /// </summary>
    public class WorkOrderBatchAllocateInputBase: BatchOperationInputLong
    {
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTimeOffset? StatusChangedTime { get; set; }
        /// <summary>
        /// 分配给谁？如果只是想将工单状态设置为“已分配，待执行”状态则此属性可为空
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 预计开始时间
        /// </summary>
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        /// <summary>
        /// 预计结束时间
        /// </summary>
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
    }
}