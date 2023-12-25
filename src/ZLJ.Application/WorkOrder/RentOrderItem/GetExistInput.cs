using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder.RentOrderItem
{
   /// <summary>
   /// 获取指定设备已存在的，未完结（未拒绝且未完成）的工单的输入模型
   /// </summary>
    public class GetExistInput
    {
        /// <summary>
        /// 租赁明细id
        /// </summary>
        public long RentOrderItemId { get; set; }
    }
}
