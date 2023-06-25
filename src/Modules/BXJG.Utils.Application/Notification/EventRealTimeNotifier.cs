using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Net.Mail;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
{

    public class NotificationEventData : Abp.Events.Bus.EventData
    {
        public UserNotification[] UserNotifications { get; set; }
    }

    /// <summary>
    /// 基于事件的实时通知器
    /// 它将通知发布为事件，以便界面的事件处理器发布基于界面的实时通知。
    /// 通常用于blazor项目
    /// </summary>
    public class EventRealTimeNotifier : IRealTimeNotifier, ISingletonDependency
    {
        /// <summary>
        /// If true, this real time notifier will be used for sending real time notifications when it is requested. Otherwise it will not be used.
        /// <para>
        /// If false, this realtime notifier will notify any notifications.
        /// </para>
        /// </summary>
        public bool UseOnlyIfRequestedAsTarget => false;

        public IEventBus EventBus { get; set; } = NullEventBus.Instance;

        public async Task SendNotificationsAsync(UserNotification[] userNotifications)
        {
            await this.EventBus.TriggerAsync<NotificationEventData>(new NotificationEventData { EventSource = this, EventTime = DateTime.Now, UserNotifications = userNotifications });
        }
    }
}