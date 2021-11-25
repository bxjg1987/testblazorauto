using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class BatchConfirmeBaseInput : BatchOperationInputLong
    {
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTimeOffset? StatusChangedTime { get; set; }
    }
}
