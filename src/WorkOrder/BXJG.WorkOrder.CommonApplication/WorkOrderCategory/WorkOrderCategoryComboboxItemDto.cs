using Abp.Application.Services.Dto;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单类别下拉框数据源显示模型
    /// </summary>
    public class WorkOrderCategoryComboboxItemDto : GeneralTreeComboboxDto
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
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
