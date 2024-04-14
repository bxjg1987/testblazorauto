using Abp.Threading.Timers;
using BXJG.Utils.Application.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Core.Authorization.Users;

namespace ZLJ.Application.Common.Notification
{
    public class RemoveOldNoticesBackgroundWorker : RemoveOldNotification<User>
    {
        public RemoveOldNoticesBackgroundWorker(AbpAsyncTimer timer) : base(timer)
        {
        }
    }
}
