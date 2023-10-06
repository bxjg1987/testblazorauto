using Abp.Notifications;
using Abp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 通知订阅组件抽象类
    /// 注：目前为实现实体通知的订阅
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    public class NotifySubscript : AbpBaseComponent
    {
        public INotificationDefinitionManager NotificationDefinitionManager { get;  set; }

        public INotificationSubscriptionManager NotificationSubscriptionManager { get;  set; }

        protected UserIdentifier idf;
        protected IReadOnlyList<NotificationDefinition> notificationDefinitions = new List<NotificationDefinition>();
        protected HashSet<string> seleted = new HashSet<string>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            NotificationDefinitionManager= base.ScopedServices.GetRequiredService<INotificationDefinitionManager>();
            NotificationSubscriptionManager = base.ScopedServices.GetRequiredService<INotificationSubscriptionManager>();
        }

        protected override async Task OnInitializedAsync()
        {
            if (idf != default)
                return;

            idf = new Abp.UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value);
            //getall可能是获取所有，而GetAllAvailableAsync有权访问的通知定义
            notificationDefinitions = await NotificationDefinitionManager.GetAllAvailableAsync(idf);

            var notificationSubscriptions = await NotificationSubscriptionManager.GetSubscribedNotificationsAsync(idf);

            seleted = notificationDefinitions.Where(x => notificationSubscriptions.Any(y => y.NotificationName == x.Name)).Select(c => c.Name).ToHashSet();
        }

        protected virtual async Task SelectChanged(string name, bool xuanze)
        {
          
                if (xuanze)
                {
                    await NotificationSubscriptionManager.SubscribeAsync(idf, name);
                    seleted.AddIfNotContains(name);
                }
                else
                {
                    await NotificationSubscriptionManager.UnsubscribeAsync(idf, name);
                    seleted.Remove(name);
                }
                //base.ShowSuccess("");
        
        }
    }
}
