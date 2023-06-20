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

namespace BXJG.Utils.Notification
{
    public class NotifySubscript<TUser, TUserManager, TRole> : AbpComponentBase<TUser, TUserManager, TRole>
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TUserManager : AbpUserManager<TRole, TUser>
    {
        [Inject]
        public INotificationDefinitionManager NotificationDefinitionManager { get; set; }
        [Inject]
        public INotificationSubscriptionManager NotificationSubscriptionManager { get; set; }

        protected UserIdentifier idf;
        protected IReadOnlyList<NotificationDefinition> notificationDefinitions = new List<NotificationDefinition>();
        protected HashSet<string> seleted = new HashSet<string>();


        protected override async Task OnInitialized2Async()
        {
            idf = new Abp.UserIdentifier(base.AbpSession.TenantId, base.AbpSession.UserId.Value);
            //getall可能是获取所有，而GetAllAvailableAsync有权访问的通知定义
            notificationDefinitions = await NotificationDefinitionManager.GetAllAvailableAsync(idf);

            var notificationSubscriptions = await NotificationSubscriptionManager.GetSubscribedNotificationsAsync(idf);

            seleted = notificationDefinitions.Where(x => notificationSubscriptions.Any(y => y.NotificationName == x.Name)).Select(c => c.Name).ToHashSet();
        }

        protected virtual async Task SelectChanged(string name, bool xuanze)
        {
            await base.SafeExecuteAsync(async () =>
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
            });
        }
    }
}
