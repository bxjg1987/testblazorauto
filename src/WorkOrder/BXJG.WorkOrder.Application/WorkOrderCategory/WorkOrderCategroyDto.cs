using BXJG.Utils.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 后台管理工单列表页显示模型
    /// </summary>
    public class WorkOrderCategroyDto : GeneralTreeGetTreeNodeBaseDto<WorkOrderCategroyDto>
    {
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
        public IEnumerable<CategoryWorkOrderTypeDto> WorkOrderTypes { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// 关联的工单类型
        /// </summary>
        public string WorkOrderTypeDisplayName { get; set; }
    }
}
