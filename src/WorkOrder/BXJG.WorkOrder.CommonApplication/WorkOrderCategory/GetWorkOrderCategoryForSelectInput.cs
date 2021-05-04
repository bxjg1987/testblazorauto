using BXJG.Common.Dto;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 获取工单类别下拉框数据源时的输入模型
    /// </summary>
    public class GetWorkOrderCategoryForSelectInput : GeneralTreeGetForSelectInput
    {
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
        [StringLength(CoreConsts.WorkOrderClsTypeMaxLength)]
        public string WorkOrderType { get; set; }
    }
}
