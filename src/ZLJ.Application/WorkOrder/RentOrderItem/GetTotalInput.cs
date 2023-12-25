using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder.RentOrderItem.Admin
{
    /// <summary>
    /// 后台管理员获取客户设备工的单数量或列表时的输入模型
    /// </summary>
    public class GetTotalInput : BXJG.WorkOrder.WorkOrder.GetTotalInputBase
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
        public string EquipmentId { get; set; }
    }
}
