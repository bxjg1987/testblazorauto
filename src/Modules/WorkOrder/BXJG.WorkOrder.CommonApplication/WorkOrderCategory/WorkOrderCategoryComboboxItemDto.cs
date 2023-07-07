using Abp.Application.Services.Dto;
using BXJG.Utils.GeneralTree;
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
        /// 是否是默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
