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
        /// 所属类型，为空则不限制，否则查询指定类型的工单分类，且若ContainsNullWorkOrderType为true还将获取未指定类型的工单类别
        /// </summary>
        public IEnumerable<string> WorkOrderTypes { get; set; }
        /// <summary>
        /// 是否包含关联工单类型为空的类别
        /// </summary>
        public bool? ContainsNullWorkOrderType { get; set; } = true;
        /// <summary>
        /// 查询类型
        /// </summary>
        public CategoryTypeQueryType? CategoryTypeQueryType { get; set; }
    }
}
