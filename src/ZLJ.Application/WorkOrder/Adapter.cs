using Abp.Dependency;
using BXJG.WorkOrder.Employee;
using BXJG.WorkOrder.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.WorkOrder
{
    public class EmployeeAppService :  IEmployeeAppService
    {
        static List<EmployeeDto> items = new List<EmployeeDto>{
           new EmployeeDto{ Id="1", Name="aa", Phone= "13252658457" },
           new EmployeeDto{ Id="2", Name="bbbb", Phone= "13585458475" }
        };
        public async Task<IEnumerable<EmployeeDto>> GetByIdsAsync(params string[] ids)
        {
            return items.Where(c => ids.Contains(c.Id));
        }

        public async Task<IEnumerable<string>> GetIdsByKeywordAsync(string keyword)
        {
            return items.Where(c => c.Name.Contains(keyword) || c.Phone.Contains(keyword)).Select(c => c.Id);
        }

    }
    public class EmployeeSession :  IEmployeeSession
    {
        

        public string CurrentEmployeeId => "1";
    }
}
