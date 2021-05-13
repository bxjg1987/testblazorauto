using Abp.Domain.Entities;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单类别和类型之间多对多关联实体
    /// </summary>
    public class WorkOrderCategoryTypeEntity : Entity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 为了方便将来做数据处理，加上租户
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 工单类型名
        /// </summary>
        public string WorkOrderType { get; set; }
        /// <summary>
        /// 是否为默认类别
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
