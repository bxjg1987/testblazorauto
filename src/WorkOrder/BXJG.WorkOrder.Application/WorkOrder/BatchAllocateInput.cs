using BXJG.Common.Dto;
using System;

namespace BXJG.WorkOrder.WorkOrder
{
    public class BatchAllocateInput: BatchOperationInputLong
    {
        public string EmployeeId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}