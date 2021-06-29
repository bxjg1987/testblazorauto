using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.EmployeeApplication.WorkOrder
{
    public class BatchChangeStatusInputBase
    {
        /// <summary>
        /// 状态改变说明或备注信息
        /// </summary>
        [StringLength(CoreConsts.OrderStatusChangedDescriptionMaxLength)]
        public string StatusChangedDescription { get; set; }
    }
}
