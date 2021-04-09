using Abp.Dependency;
using BXJG.DynamicAssociateEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.DynamicAssociateEntity
{
    public class WorkOrderDynamicAssociateEntityHelper : DynamicAssociateEntityHelper, ITransientDependency
    {
        public WorkOrderDynamicAssociateEntityHelper(DynamicAssociateEntityDefineManager dynamicAssociateEntityDefineManager,  IIocResolver iocResolver) : base(dynamicAssociateEntityDefineManager, Class2.DynamicAssociateEntity, iocResolver)
        {
        }
    }
}
