using Abp.Dependency;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Notifications;
namespace BXJG.Utils.Events
{
    /*
     * 前后端分离场景中，前端会缓存一些数据，abp默认地会缓存如：本地化、设置、菜单、权限等到前端
     * 后端变动时，前端需要刷新页面才会获取最新数据。
     * 最好有一种机制，通知下前端，提示用户刷新，或静默刷新
     * 
     * 此场景有些特点，只需要通知当前在线的用户
     * 仅需要通知部分在线的用户，比如修改某个租户的相关配置，仅需要通知这部分租户中所有在线的用户
     * 不需要持久化
     * 
     * abp通知系统参考官方文档，INotificationPublisher的默认实现会持久化消息，以确保离线用户在下次登录时能获取通知。
     * 而IRealTimeNotifier只是发送实时通知，它更适合我们的场景
     * 它仅仅是发送实时通知。
     * 
     * 但调用时不够方便，这里做个封装，以适应上面所说的场景。
     * 
     * 通知即事件，事件即通知
     * 
     */

    //请关注：https://github.com/aspnetboilerplate/aspnetboilerplate/issues/6985

    /// <summary>
    /// 前端事件触发器
    /// 后端通过此触发器触发前端事件
    /// </summary>
    public class UIEventTrigger : ITransientDependency
    {
        private readonly IOnlineClientManager onlineClientManager;
        private readonly IAbpSession abpSession;
        //private readonly ICancellationTokenProvider cancellationTokenProvider;
        private readonly IRealTimeNotifier realTimeNotifier;
        public UIEventTrigger(IOnlineClientManager onlineClientManager, IAbpSession session,/* ICancellationTokenProvider cancellationTokenProvider,*/ IRealTimeNotifier realTimeNotifier)
        {
            this.onlineClientManager = onlineClientManager;
            this.abpSession = session;
            //this.cancellationTokenProvider = cancellationTokenProvider;
            this.realTimeNotifier = realTimeNotifier;
            //Abp.Notifications.UserNotificationInfoWithNotificationInfoExtensions
        }

        /// <summary>
        /// 触发一个前端事件
        /// </summary>
        /// <param name="scope">范围，0应用程序 1租户 2用户</param>
        /// <param name="scopeId">scope为0时忽略，为1时表示租户id，为2时表示用户id，不为0时，若为空则自动去当前租户或用户id</param>
        /// <param name="eventName">事件名称</param>
        /// <param name="param">事件参数，可空</param>
        /// <returns></returns>
        public async Task Trigger(string eventName, object param = default, int scope = 0, long? scopeId = default)
        {
            IEnumerable<IOnlineClient> connections = await onlineClientManager.GetAllClientsAsync();
            switch (scope)
            {
                // case 0:
                // connections = clients;
                //     break;
                case 1:
                    if (!scopeId.HasValue)
                        scopeId = abpSession.TenantId;
                    connections = connections.Where(x => x.TenantId == scopeId);
                    break;
                case 2:
                    if (!scopeId.HasValue)
                        scopeId = abpSession.UserId;
                    connections = connections.Where(x => x.UserId == scopeId);
                    break;
            }
            var uns = connections.Select(x => new UserNotification
            {
                TenantId = x.TenantId, //TargetNotifiers=
                UserId = x.UserId.Value,
                Id = Guid.NewGuid(),
                State = UserNotificationState.Unread,
                Notification = new()
                {
                    CreationTime = DateTime.Now,
                    Data = new()
                    {
                        Properties = new Dictionary<string, object>() { { "param", param } }
                    },
                    NotificationName = eventName,
                    Severity = NotificationSeverity.Info, 
                    TenantId = x.TenantId,
                    Id = Guid.NewGuid(),
                }
            }).ToArray();
            await realTimeNotifier.SendNotificationsAsync(uns);

            //var connectionIds = connections.Select(x => x.ConnectionId);
            ////无论是abp的事件总线，还是我们的Zhongjie都最多支持一个事件参数，所以这里也只考虑1个事件参数的情况
            ////但前端貌似没有必要再用zongjie触发一次，因为hubconnection已经是一个事件总线了，此时传多个参数是有必要的。
            ////后端调用前端 大部分情况只是想给前端一个信号，传少量用于前端角色的数据过去是可以的，做复杂处理前端可以再发请求到服务端
            ////所以目前的设计够用了，复杂参数要么自定义对象一次性传递，或直接用hubcontext原生api即可。
            //if (param != default)
            //    await hubContext.Clients.Clients(connectionIds).SendAsync(eventName, param, cancellationTokenProvider.Token);
            //else
            //    await hubContext.Clients.Clients(connectionIds).SendAsync(eventName, cancellationTokenProvider.Token);
        }
        /// <summary>
        /// 触发一个前端事件
        /// </summary>
        /// <typeparam name="T">参数类型，其类型的FullName作为事件名称</typeparam>
        /// <param name="scope">范围，0应用程序 1租户 2用户</param>
        /// <param name="scopeId">scope为0时忽略，为1时表示租户id，为2时表示用户id，不为0时，若为空则自动去当前租户或用户id</param>
        /// <param name="param">事件参数，可空</param>
        /// <returns></returns>
        public Task Trigger<T>(object param = default, int scope = 0, long? scopeId = default)
        {
            return Trigger(typeof(T).FullName, param, scope, scopeId);
        }
        /// <summary>
        /// 触发一个应用程序级别的前端事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public Task TriggerApplication(string eventName, object param = default)
        {
            return Trigger(eventName, param);
        }
        /// <summary>
        /// 触发一个应用程序级别的前端事件
        /// </summary>
        /// <typeparam name="T">参数类型，其类型的FullName作为事件名称</typeparam>
        /// <param name="param">参数对象</param>
        /// <returns></returns>
        public Task TriggerApplication<T>(object param = default)
        {
            return TriggerApplication(typeof(T).FullName, param);
        }
        /// <summary>
        /// 触发一个租户级别的前端事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="param"></param>
        /// <param name="tenantId">若为空则取当前租户</param>
        /// <returns></returns>
        public Task TriggerTenant(string eventName, object param = default, int? tenantId = default)
        {
            return Trigger(eventName, param, 1, tenantId);
        }
        /// <summary>
        /// 触发一个租户级别的前端事件
        /// </summary>
        /// <typeparam name="T">参数类型，其类型的FullName作为事件名称</typeparam>
        /// <param name="param"></param>
        /// <param name="tenantId">若为空则取当前租户</param>
        /// <returns></returns>
        public Task TriggerTenant<T>(object param = default, int? tenantId = default)
        {
            return TriggerTenant(typeof(T).FullName, param, tenantId);
        }
        /// <summary>
        /// 触发一个用户级别的前端事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="param"></param>
        /// <param name="tenantId">若为空则取当前用户</param>
        /// <returns></returns>
        public Task TriggerUser(string eventName, object param = default, long? userId = default)
        {
            return Trigger(eventName, param, 2, userId);
        }
        /// <summary>
        /// 触发一个用户级别的前端事件
        /// </summary>
        /// <typeparam name="T">参数类型，其类型的FullName作为事件名称</typeparam>
        /// <param name="param"></param>
        /// <param name="tenantId">若为空则取当前用户</param>
        /// <returns></returns>
        public Task TriggerUser<T>(object param = default, long? userId = default)
        {
            return TriggerUser(typeof(T).FullName, param, userId);
        }
    }
}