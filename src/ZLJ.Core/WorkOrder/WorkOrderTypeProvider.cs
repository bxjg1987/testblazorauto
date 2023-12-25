using BXJG.WorkOrder.WorkOrderType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Localization;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;

namespace ZLJ.WorkOrder
{
    public class WorkOrderTypeProvider : IWorkOrderTypeProvider
    {
        public void Create(WorkOrderTypeProviderContext context)
        {
            context.Defines.Add(new WorkOrderTypeDefine
            {
                DisplayName = "客户设备工单".GetLocalizableString(),
                Name = WorkOrderConsts.RentOrderItemWorkOrder
            });
        }
    }
}
