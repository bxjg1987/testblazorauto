using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class CategoryEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        public byte[] RowVersion { get; private set; }
        public string ExtensionData { get; set; }
    }
}
