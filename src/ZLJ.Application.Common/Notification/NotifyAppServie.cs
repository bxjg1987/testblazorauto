using Abp.Notifications;
using BXJG.Utils;
using BXJG.Utils.Application.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Users;

namespace ZLJ.App.Common.Notification
{
    public class NotifyAppServie : PersonNotificationAppService<User>
    {
        public NotifyAppServie()
        {
            base.LocalizationSourceName = Consts.Common;
        }
    }
}
