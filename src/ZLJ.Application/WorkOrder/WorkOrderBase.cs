using BXJG.WorkOrder.WorkOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Admin.WorkOrder
{
    public class WorkOrderUpdateBaseDto : UpdateInputBase { 
        public int? Points { get; set; }
    }
    public class WorkOrderCreateBaseDto : CreateInputBase
    {
        public int? Points { get; set; }
    }
    public class WorkOrderBaseDto: DtoBase
    {
        public int? Points { get; set; }
    }
    /// <summary>
    /// 批量确认工单时的输入模型
    /// </summary>
    public class BatchConfirmeInput : BatchConfirmeBaseInput {
        public int? Points { get; set; }
    }
}
