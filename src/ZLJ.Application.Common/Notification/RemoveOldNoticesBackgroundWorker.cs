using Abp.Threading.Timers;
using BXJG.Utils.Application.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Users;
using ZLJ.Core.MultiTenancy;

namespace ZLJ.Application.Common.Notification
{
    public class RemoveOldNoticesBackgroundWorker : RemoveOldNotification<User, TenantManager,Tenant>
    {
        public RemoveOldNoticesBackgroundWorker(AbpAsyncTimer timer) : base(timer)
        {
        }
    }
}
