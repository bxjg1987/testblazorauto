using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder.RentOrderItem.Admin
{
    /// <summary>
    /// 获取（关联租赁单明细的）工单时的输入模型
    /// </summary>
    public class GetAllInput : GetAllInputBase<GetTotalInput>
    {
        ///// <summary>
        ///// 租赁单明细Id
        ///// </summary>
        //public long? RentOrderItemId { get; set; }
    }
}
