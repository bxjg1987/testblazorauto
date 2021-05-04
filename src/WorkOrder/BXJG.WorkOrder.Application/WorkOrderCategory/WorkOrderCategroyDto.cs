using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class WorkOrderCategroyDto : GeneralTreeGetTreeNodeBaseDto<WorkOrderCategroyDto>
    {   
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
        public string WorkOrderType { get; set; }
        /// <summary>
        /// 所属类型名称
        /// </summary>
        public string WorkOrderTypeName { get; set; }
        /// <summary>
        /// 是否为默认类别
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
