using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BXJG.Utils;
using Abp.Domain.Uow;
using Abp.Authorization;

namespace BXJG.WorkOrder.Employee
{
    [AbpAuthorize]
    [UnitOfWork(false)]
    public class EmployeeAppService : AppServiceBase
    {
        private readonly IEmployeeAppService employeeAppService;

        public EmployeeAppService(IEmployeeAppService employeeAppService)
        {
            this.employeeAppService = employeeAppService;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(GetAllInput input)
        {
            var list = (await employeeAppService.GetAllAsync(input.Keyword)).ToList();

            if (input.ForType <= 0)
                return list;

            if (!input.ParentText.IsNullOrWhiteSpace())
            {
                list.Insert(0, new EmployeeDto { Name = L(input.ParentText) });
                return list;
            }
            if (input.ForType <= 2)
            {
                list.Insert(0, new EmployeeDto { Name = "==" + L("员工") + "== " });
                return list;
            }
           
            if (input.ForType <= 4)
            {
                list.Insert(0, new EmployeeDto { Name = "==请选择==".UtilsL() });
                return list;
            }

            throw new ApplicationException();
        }
    }
}
