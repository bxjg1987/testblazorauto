using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using Abp.Linq.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace BXJG.WorkOrder.Session
{
    public class EmployeeSession : IEmployeeSession
    {
        private readonly IAbpSession abpSession;

        public EmployeeSession(IAbpSession abpSession)
        {
            this.abpSession = abpSession;
        }

        public string BusinessUserId => abpSession.UserId?.ToString();

    }
}

