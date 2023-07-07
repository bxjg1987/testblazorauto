using BXJG.WorkOrder.EmployeeApplication.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrderEmployee.RentOrderItem
{
    /// <summary>
    /// 维修人员端（关联租赁单明细的）工单的显示模型
    /// </summary>
    public class RentOrderItemDto : WorkOrderDtoBase
    {
        /// <summary>
        /// 租赁单明细id
        /// </summary>
        public long RentOrderItemId { get; set; }
        /// <summary>
        /// 关联的设备所属客户Id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 关联的设备所属客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 租赁Id
        /// </summary>
        public string RentOrderId { get; set; }
        /// <summary>
        /// 租赁单号
        /// </summary>
        public string RentOrderNo { get; set; }
        /// <summary>
        /// 关联的设备Id
        /// </summary>
        public long EquipmentId { get; set; }
        /// <summary>
        /// 关联的设备序列号
        /// </summary>
        public string EquipmentNo { get; set; }
        /// <summary>
        /// 关联的设备品牌名称
        /// </summary>
        public string EquipmentBrandName { get; set; }
        /// <summary>
        /// 关联的设备型号
        /// </summary>
        public string EquipmentModel { get; set; }
        /// <summary>
        /// 客户名+单号+设备品牌+型号+序列号
        /// </summary>
        public string EquipmentDisplayName { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 员工手机号
        /// </summary>
        public string EmployeePhone { get; set; }
    }
}
