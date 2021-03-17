using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Session
{
    public interface IEmployeeSession
    {
        string CurrentEmployeeId { get; }
    }
}
