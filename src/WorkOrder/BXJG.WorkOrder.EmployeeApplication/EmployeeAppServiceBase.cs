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
        /// <summary>
        /// 当前登陆的员工id
        /// </summary>
        protected string CurrentEmployeeId => employeeSession.CurrentEmployeeId;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }
        protected EmployeeAppServiceBase(IEmployeeSession employeeSession)
        {
            LocalizationSourceName = CoreConsts.LocalizationSourceName;
            this.employeeSession = employeeSession;
        }
    }
}
