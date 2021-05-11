using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class CategoryWorkOrderTypeEntity : Entity<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public string WorkOrderType { get; set; }
    }
}
