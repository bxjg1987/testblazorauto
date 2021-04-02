using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using BXJG.WorkOrder.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using BXJG.WorkOrder.Session;

namespace BXJG.WorkOrder.Session
{
    public class EmployeeSession : IEmployeeSession
    {
        private readonly IAbpSession abpSession;

        public EmployeeSession(IAbpSession abpSession)
        {
            this.abpSession = abpSession;
        }

        public string CurrentEmployeeId => abpSession.UserId?.ToString();
    }
}

