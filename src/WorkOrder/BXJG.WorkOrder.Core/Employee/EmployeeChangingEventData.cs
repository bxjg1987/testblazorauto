using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Employee
{
    public class EmployeeChangingEventData : EntityChangingEventData<IEmployeeEntity>
    {
        public EmployeeChangingEventData(IEmployeeEntity entity) : base(entity)
        {
        }
    }
}
