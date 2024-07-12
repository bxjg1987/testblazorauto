using Abp.Dependency;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Threading;
namespace BXJG.Utils.Web.Events
{
    /// <summary>
    /// 前端事件触发器
    /// 后端通过此触发器触发前端事件
    /// </summary>
    public class UIEventTrigger : ITransientDependency
    {
        private readonly IHubContext<AbpCommonHub> hubContext;
        private readonly IOnlineClientManager onlineClientManager;
        private readonly IAbpSession abpSession;
        private readonly ICancellationTokenProvider cancellationTokenProvider;

        public UIEventTrigger(IHubContext<AbpCommonHub> hubContext, IOnlineClientManager onlineClientManager, IAbpSession session, ICancellationTokenProvider cancellationTokenProvider)
        {
            this.hubContext = hubContext;
            this.onlineClientManager = onlineClientManager;
            this.abpSession = session;
            this.cancellationTokenProvider = cancellationTokenProvider;
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
            var connectionIds = connections.Select(x => x.ConnectionId);
            //无论是abp的事件总线，还是我们的Zhongjie都最多支持一个事件参数，所以这里也只考虑1个事件参数的情况
            //但前端貌似没有必要再用zongjie触发一次，因为hubconnection已经是一个事件总线了，此时传多个参数是有必要的。
            //后端调用前端 大部分情况只是想给前端一个信号，传少量用于前端角色的数据过去是可以的，做复杂处理前端可以再发请求到服务端
            //所以目前的设计够用了，复杂参数要么自定义对象一次性传递，或直接用hubcontext原生api即可。
            if (param != default)
                await hubContext.Clients.Clients(connectionIds).SendAsync(eventName, param, cancellationTokenProvider.Token);
            else
                await hubContext.Clients.Clients(connectionIds).SendAsync(eventName, cancellationTokenProvider.Token);
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