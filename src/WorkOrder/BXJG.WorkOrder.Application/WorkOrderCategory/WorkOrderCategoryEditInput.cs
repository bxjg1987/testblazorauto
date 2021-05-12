using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class WorkOrderCategoryEditInput : GeneralTreeNodeEditBaseDto
    {
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
        public IEnumerable<WorkOrderTypeDto> WorkOrderTypes { get; set; }
    }
    /// <summary>
    /// 新增或修改工单类别时关联工单类型的输入模型
    /// </summary>
    public class WorkOrderTypeDto
    {
        /// <summary>
        /// 工单类型
        /// </summary>
        [StringLength(CoreConsts.WorkOrderTypeMaxLength)]
        public string WorkOrderType { get; set; }
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
