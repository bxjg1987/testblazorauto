using BXJG.Utils.BusinessUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Session
{
    public interface IEmployeeSession: IBusinessUserSession<string>
    {
        //string CurrentEmployeeId { get; }
    }
}
