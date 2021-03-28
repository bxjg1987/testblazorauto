using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.Employee
{

    //数据量大时修改冗余字段太恐怖了，还是决定在应用层查询组合

    public interface IEmployeeAppService
    {
        Task<IEnumerable<EmployeeDto>> GetByIdsAsync(params string[] ids);
        Task<IEnumerable<EmployeeDto>> GetAllAsync(string keyword);
        Task<IEnumerable<string>> GetIdsByKeywordAsync(string keyword);
    }
}
