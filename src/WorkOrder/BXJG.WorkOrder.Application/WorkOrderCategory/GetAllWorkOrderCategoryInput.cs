using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class GetAllWorkOrderCategoryInput: GeneralTreeGetTreeInput
    {   
        /// <summary>
        /// 所属类型，为空则表示所有类型的工单公用
        /// </summary>
       [StringLength(CoreConsts.WorkOrderClsTypeMaxLength)]
        public string WorkOrderType { get; set; }
    }
}
