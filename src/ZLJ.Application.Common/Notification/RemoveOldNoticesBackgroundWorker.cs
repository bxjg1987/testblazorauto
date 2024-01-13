using Abp.Threading.Timers;
using BXJG.Utils.Application.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;

namespace ZLJ.App.Common.Notification
{
    public class RemoveOldNoticesBackgroundWorker : RemoveOldNotification<User>
    {
        public RemoveOldNoticesBackgroundWorker(AbpTimer timer) : base(timer)
        {
        }
    }
}
