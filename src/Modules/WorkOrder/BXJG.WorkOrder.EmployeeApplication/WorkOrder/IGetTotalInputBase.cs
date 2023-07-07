using BXJG.WorkOrder.WorkOrder;
using System;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    public interface IGetTotalInputBase
    {
        string[] CategoryCodes { get; set; }
        DateTimeOffset? CompletionTimeEnd { get; set; }
        DateTimeOffset? CompletionTimeStart { get; set; }
        DateTimeOffset? EstimatedCompletionTimeEnd { get; set; }
        DateTimeOffset? EstimatedCompletionTimeStart { get; set; }
        DateTimeOffset? EstimatedExecutionTimeEnd { get; set; }
        DateTimeOffset? EstimatedExecutionTimeStart { get; set; }
        DateTimeOffset? ExecutionTimeEnd { get; set; }
        DateTimeOffset? ExecutionTimeStart { get; set; }
        string Keyword { get; set; }
        Status[] Statuses { get; set; }
        UrgencyDegree[] UrgencyDegrees { get; set; }
    }
}