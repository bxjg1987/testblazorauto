using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.WorkOrder.EmployeeApplication.WorkOrder;

namespace ZLJ.WorkOrderEmployee.RentOrderItem
{
    /// <summary>
    /// 维修人员获取客户设备工的单数量或列表时的输入模型
    /// </summary>
    public class GetTotalInput : GetTotalInputBase
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public long? CustomerId { get; set; }
        /// <summary>
        /// 租赁单id
        /// </summary>
        public long? RentOrderId { get; set; }
        /// <summary>
        /// 租赁明细id
        /// </summary>
        public long? RentOrderItemId { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public long? EquipmentId { get; set; }
    }
}
