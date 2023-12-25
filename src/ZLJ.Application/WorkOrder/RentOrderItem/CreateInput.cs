using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder.RentOrderItem.Admin
{
    public class CreateInput : WorkOrderCreateBaseDto
    {
        /// <summary>
        /// 关联的租赁单明细Id
        /// </summary>
        [Required]
        public long RentOrderItemId { get; set; }
    }
}
