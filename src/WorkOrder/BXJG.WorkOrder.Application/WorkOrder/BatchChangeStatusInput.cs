using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class BatchChangeStatusInput: BatchOperationInput
    {
        public Status Status { get; set; }
        public string Description { get; set; }
    }
}
