using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Employee
{
    public class EmployeeEntity : IEmployeeEntity
    {
        public EmployeeEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public virtual string Id { get; private set; }

        public virtual string Name { get; private set; }
    }
}
