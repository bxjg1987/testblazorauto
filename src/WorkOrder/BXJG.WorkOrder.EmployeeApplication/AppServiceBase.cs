using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.Localization.Sources;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Abp.Threading;
using BXJG.WorkOrder.EmployeeApplication.Session;

namespace BXJG.WorkOrder.EmployeeApplication
{
    /// <summary>
    /// 工单处理人应用服务基类
    /// </summary>
    public abstract class AppServiceBase : BXJG.WorkOrder.AppServiceBase
    {
        /// <summary>
        /// 工单处理人session
        /// </summary>
        protected readonly IEmployeeSession employeeSession;
        /// <summary>
        /// 当前登陆的处理人id
        /// </summary>
        protected string CurrentEmployeeId => employeeSession.BusinessUserId;

        public AppServiceBase(IEmployeeSession employeeSession)
        {
            //抽象模块中不设置，留给子类用
            //LocalizationSourceName = CoreConsts.LocalizationSourceName;
            this.employeeSession = employeeSession;
        }
    }
}
