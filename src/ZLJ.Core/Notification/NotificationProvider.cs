using Abp.Localization;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Notification
{
    // 各上层app可以覆盖重写这里定义的通知
    public class MyAppNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            //context.Manager.Add(
            //    new NotificationDefinition("WorkOrder.Created",
            //                               displayName: new LocalizableString("WorkOrderCreatedNotificationDefinition", ZLJConsts.LocalizationSourceName))
            //);
        }
    }
}
