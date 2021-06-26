using BXJG.Common.Dto;
using System;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    /// <summary>
    /// 维修人员批量领取工单的输入模型
    /// </summary>
    public class WorkOrderBatchAllocateInput1Base: BatchOperationInputLong
    {
        ///// <summary>
        ///// 分配时间
        ///// </summary>
        //public DateTimeOffset? StatusChangedTime { get; set; }
        ///// <summary>
        ///// 分配给谁？如果只是想将工单状态设置为“已分配，待执行”状态则此属性可为空
        ///// </summary>
        //public string EmployeeId { get; set; }
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