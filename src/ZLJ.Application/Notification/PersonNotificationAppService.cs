using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.StaffInfo;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using BXJG.Common.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Domain.Uow;
using System.Linq.Dynamic.Core;
using System.Collections.ObjectModel;

namespace ZLJ.Notification
{
    /// <summary>
    /// 通用个人消息管理服务
    /// </summary>
    [AbpAuthorize]
    public class PersonNotificationAppService : ZLJAppServiceBase
    {
        private readonly Lazy<IUserNotificationManager> userNotificationManager;
        //private readonly INotificationDefinitionManager 
        private readonly Lazy<IRepository<UserNotificationInfo, Guid>> userNotificationRepository;
        private readonly Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository;

        public PersonNotificationAppService(Lazy<IUserNotificationManager> userNotificationManager,
                                            Lazy<IRepository<UserNotificationInfo, Guid>> repository,
                                            Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository)
        {
            this.userNotificationManager = userNotificationManager;
            this.userNotificationRepository = repository;
            this.tenantNotificationRepository = tenantNotificationRepository;
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public async Task<int> GetTotalAsync(GetTotalInput input)
        {
            var query = GetQuery(input);
            return await query.CountAsync();
        }
        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public async Task<PagedResultDto<UserNotification>> GetAllAsync(GetAllInput input)
        {
            var r = new PagedResultDto<UserNotification>();
            //var u = new Abp.UserIdentifier(base.AbpSession.TenantId, AbpSession.UserId.Value);
            //r.TotalCount = await userNotificationManager.GetUserNotificationCountAsync(u, input.UserNotificationState, input.StartTime, input.EndTime);
            //r.Items = await userNotificationManager.GetUserNotificationsAsync(u, input.UserNotificationState, input.SkipCount, input.MaxResultCount, input.StartTime, input.EndTime);
            //UserNotificationInfo

            var query = GetQuery(input.GetTotalInput);
            r.TotalCount = await query.CountAsync();
            query = query.OrderBy(input.Sorting).PageBy(input);

            var items = await query.ToListAsync();
            var tempList = new List<UserNotification>();
            foreach (var item in items)
            {
                //var tn = ObjectMapper.Map<TenantNotification>(item.e);
                //var un = ObjectMapper.Map<UserNotification>(item.c);
                //un.Notification = tn;

                var temp = new UserNotification
                {
                    Id = item.UserNotificationInfo.Id,
                    State = item.UserNotificationInfo.State,
                    TenantId = item.UserNotificationInfo.TenantId,
                    UserId = item.UserNotificationInfo.UserId,
                    Notification = new TenantNotification
                    {
                        CreationTime = item.UserNotificationInfo.CreationTime,
                        Data = System.Text.Json.JsonSerializer.Deserialize<NotificationData>(item.TenantNotificationInfo.Data),
                        EntityId = item.TenantNotificationInfo.EntityId,
                        EntityTypeName = item.TenantNotificationInfo.EntityTypeName,
                        Id = item.TenantNotificationInfo.Id,
                        NotificationName = item.TenantNotificationInfo.NotificationName,
                        Severity = item.TenantNotificationInfo.Severity,
                        TenantId = item.TenantNotificationInfo.TenantId
                    }
                };
                tempList.Add(temp);
            }
            r.Items = new ReadOnlyCollection<UserNotification>(tempList);
            return r;
        }

        private IQueryable<temp> GetQuery(GetTotalInput input)
        {
            var query = from c in userNotificationRepository.Value.GetAll().AsNoTrackingWithIdentityResolution()
                        join d in tenantNotificationRepository.Value.GetAll() on c.TenantNotificationId equals d.Id into temp
                        from e in temp.DefaultIfEmpty()
                        where c.UserId == AbpSession.UserId
                        select new temp { TenantNotificationInfo = e, UserNotificationInfo = c };

            if (input.NotificationNames != null && input.NotificationNames.Any())
            {
                if (input.NotificationNameContains)
                    query = query.Where(c => input.NotificationNames.Contains(c.TenantNotificationInfo.NotificationName));
                else
                    query = query.Where(c => !input.NotificationNames.Contains(c.TenantNotificationInfo.NotificationName));
            }

            query = query.WhereIf(input.UserNotificationState.HasValue, c => c.UserNotificationInfo.State == input.UserNotificationState)
                         .WhereIf(input.StartTime.HasValue, c => c.TenantNotificationInfo.CreationTime >= input.StartTime)
                         .WhereIf(input.EndTime.HasValue, c => c.TenantNotificationInfo.CreationTime < input.EndTime)
                         .WhereIf(input.NotificationSeverities != null && input.NotificationSeverities.Any(), c => input.NotificationSeverities.Contains(c.TenantNotificationInfo.Severity));
            var sql = query.ToQueryString();
            return query;
        }

        //元祖可以用，但不支持动态排序
        class temp
        {
            public TenantNotificationInfo TenantNotificationInfo { get; set; }
            public UserNotificationInfo UserNotificationInfo { get; set; }
        }

        /// <summary>
        /// 将指定消息设置为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BatchOperationOutput<Guid>> SetReadedAsync(BatchOperationInput<Guid> input)
        {
            var r = new BatchOperationOutput<Guid>();
            foreach (var item in input.Ids)
            {
                try
                {
                    await userNotificationManager.Value.UpdateUserNotificationStateAsync(AbpSession.TenantId, item, UserNotificationState.Read);
                    r.Ids.Add(item);
                }
                catch (Exception ex)
                {
                    Logger.Warn($"部分通知设置读取状态失败！{item}", ex);
                }
            }
            return r;
        }
    }
}
