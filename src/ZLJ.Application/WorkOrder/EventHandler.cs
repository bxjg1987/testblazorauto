using Abp;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Notifications;
using Abp.RealTime;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.WorkOrder.RentOrderItemWorkOrder;

namespace ZLJ.WorkOrder.EventHandlers
{
    /// <summary>
    /// 统一的工单事件处理程序
    /// </summary>
    public class EventHandler : IAsyncEventHandler<EntityCreatingEventData<RentOrderItemWorkOrderEntity>>,
                                         ITransientDependency
    {
        private readonly INotificationPublisher notificationPublisher;
        private readonly IAbpSession abpSession;
        private readonly IRepository<User, long> userRepository;

        private readonly UserManager userManager;
        private readonly RoleManager roleManager;
     
        public EventHandler(INotificationPublisher notificationPublisher, IAbpSession abpSession, IRepository<User, long> userRepository, UserManager userManager, RoleManager roleManager)
        {
            this.notificationPublisher = notificationPublisher;
            this.abpSession = abpSession;
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task HandleEventAsync(EntityCreatingEventData<RentOrderItemWorkOrderEntity> eventData)
        {
            //这个不通知到顾客，只通知到后台管理和维修师傅
            //也可以考虑使用订阅的方式
            var customerRoleId = 0;// this.roleManager.FindByNameAsync(ZLJConsts.CustomerRoleName);
            var users = await userRepository.GetAll().Where(c => !c.Roles.Any(d => d.RoleId == customerRoleId))
                                                     .Select(c => new { c.TenantId, c.Id })
                                                     .ToListAsync();
            var userIdentifiers = users.Select(c => new UserIdentifier(c.TenantId, c.Id)).ToArray();

            await notificationPublisher.PublishAsync(WorkOrderConsts.WorkOrderCreated,
                                                     new MessageNotificationData("有新的工单，请及时处理！"),
                                                     new EntityIdentifier(typeof(RentOrderItemWorkOrderEntity), eventData.Entity.Id),
                                                     NotificationSeverity.Info,
                                                     userIdentifiers);
        }
    }
}
