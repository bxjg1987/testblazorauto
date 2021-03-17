using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.WorkOrder.Session;

namespace BXJG.WorkOrder
{
    /// <summary>
    /// 员工对工单处理的应用服务抽象类
    /// </summary>
    public abstract class EmployeeAppServiceBase : AppServiceBase
    {
        protected readonly IEmployeeSession employeeSession;

        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        protected EmployeeAppServiceBase(IEmployeeSession employeeSession)
        {
            LocalizationSourceName = CoreConsts.LocalizationSourceName;
            this.employeeSession = employeeSession;
        }
    }
}
