using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class GetAllWorkOrderCategoryInput : GeneralTreeGetTreeInput
    {
        /// <summary>
        /// 所属类型，为空则不限制，否则查询指定类型的工单分类，且若ContainsNullWorkOrderType为true还将获取未指定类型的工单类别
        /// </summary>
        [StringLength(CoreConsts.WorkOrderClsTypeMaxLength)]
        public string WorkOrderType { get; set; }
        
        public bool ContainsNullWorkOrderType { get; set; } = true;
    }
}
