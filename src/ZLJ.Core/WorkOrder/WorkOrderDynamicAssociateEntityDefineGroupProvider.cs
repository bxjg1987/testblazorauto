using Abp.Dependency;
using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder
{
    public class WorkOrderDynamicAssociateEntityDefineGroupProvider : IDynamicAssociateEntityDefineGroupProvider
    {
        public Dictionary<string, List<DynamicAssociateEntityDefine>> GetDefines(DynamicAssociateEntityDefineGroupProviderContext context)
        {
            return new Dictionary<string, List<DynamicAssociateEntityDefine>>
            {
                {
                    "workOrder",
                    context.Defines.Take(2).ToList()
                }
            };
        }
    }
}
