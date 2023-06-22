using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Abp.Application.Services;
using Abp.Json;
using Newtonsoft.Json;
using Abp.UI;
using Abp.Domain.Entities;
using Abp.Authorization.Users;
using Abp.Localization;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 通用个人消息管理服务
    /// 由于通知是与个人相关的数据，所以不需要严格的权限控制
    /// 此功能是所有类型的用户通用的
    /// </summary>
    [AbpAuthorize]
    public class PersonNotificationAppService<TUser> : ApplicationService where TUser : AbpUserBase
    {
        private readonly Lazy<IRepository<TUser, long>> userRepository;
        protected IRepository<TUser, long> UserRepository => userRepository.Value;


        private readonly Lazy<IUserNotificationManager> userNotificationManager;
        protected IUserNotificationManager UserNotificationManager => userNotificationManager.Value;

        private readonly Lazy<IRepository<UserNotificationInfo, Guid>> userNotificationRepository;
        protected IRepository<UserNotificationInfo, Guid> UserNotificationRepository => userNotificationRepository.Value;

        private readonly Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository;
        protected IRepository<TenantNotificationInfo, Guid> TenantNotificationRepository => tenantNotificationRepository.Value;

        private readonly Lazy<INotificationDefinitionManager> notificationDefinitionManager;
        protected INotificationDefinitionManager NotificationDefinitionManager => notificationDefinitionManager.Value;

        private readonly Lazy<INotificationSubscriptionManager> notificationSubscriptionManager;
        protected INotificationSubscriptionManager NotificationSubscriptionManager => notificationSubscriptionManager.Value;

        public PersonNotificationAppService(Lazy<IUserNotificationManager> userNotificationManager,
                                            Lazy<IRepository<UserNotificationInfo, Guid>> repository,
                                            Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository,
                                            Lazy<IRepository<TUser, long>> userRepository,
                                            Lazy<INotificationDefinitionManager> notificationDefinitionManager,
                                            Lazy<INotificationSubscriptionManager> notificationSubscriptionManager)
        {
            this.userNotificationManager = userNotificationManager;
            this.userNotificationRepository = repository;
            this.tenantNotificationRepository = tenantNotificationRepository;
            this.userRepository = userRepository;
            this.notificationDefinitionManager = notificationDefinitionManager;

            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;

            this.notificationSubscriptionManager = notificationSubscriptionManager;
        }

        /// <summary>
        /// 获取当前用户可以订阅的通知定义列表
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<NotifyDefineDto>> GetDefines()
        {
            var list = await NotificationDefinitionManager.GetAllAvailableAsync(base.AbpSession.ToUserIdentifier());
            return base.ObjectMapper.Map<List<NotifyDefineDto>>(list);
            //var r = new List<NotifyDefineDto>();
            //foreach (var item in list)
            //{
            //    var temp = new NotifyDefineDto
            //    {
            //        Name = item.Name,
            //        DisplayName = item.DisplayName.Localize(LocalizationManager),
            //        EntityType = item.EntityType.FullName,
            //        Description = item.Description.Localize(LocalizationManager),
            //        Attributes = item.Attributes
            //    };
            //    r.Add(temp);
            //}
            //return r;
        }
        /// <summary>
        /// 批量订阅通知
        /// </summary>
        /// <param name="subscripts"></param>
        /// <returns></returns>

        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutput<SubscriptNotifyItem>> Subscript(List<SubscriptNotifyItem> subscripts)
        {
            var r = new BatchOperationOutput<SubscriptNotifyItem>();
            foreach (var name in subscripts)
            {
                EntityIdentifier? entityIdentifier = default;
                if (name.EntityTypeName.IsNotNullOrWhiteSpaceBXJG())
                {
                    entityIdentifier = new EntityIdentifier(Type.GetType(name.EntityTypeName), name.EntityId);
                }
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await NotificationSubscriptionManager.SubscribeAsync(base.AbpSession.ToUserIdentifier(), name.NotifyName, entityIdentifier);
                    await uow.CompleteAsync();
                    r.Ids.Add(name);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(name, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(name.Message500());
                    Logger.Warn($"部分订阅失败！{name}", ex);
                }
            }
            return r;
        }

        /// <summary>
        /// 获取已订阅的通知定义
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<NotificationSubscription>> GetSubscriptedNotifyDefines()
        {
            return await NotificationSubscriptionManager.GetSubscribedNotificationsAsync(base.AbpSession.ToUserIdentifier());
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<int> GetTotalAsync(GetTotalInput input)
        {
            var query = GetQuery(input);
            return await query.CountAsync();
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<Dictionary<SubscriptNotifyItem, int>> GetTotalGroupBySubscript(GetTotalInput input)
        {
            var query = GetQuery(input).GroupBy(c =>new SubscriptNotifyItem { NotifyName=  c.TenantNotificationInfo.NotificationName , EntityTypeName = c.TenantNotificationInfo.EntityTypeName, EntityId= c.TenantNotificationInfo.EntityId}).Select(c => new { c.Key, ct = c.Count() });
            var tm = await query.ToArrayAsync();
            return tm.ToDictionary(c => c.Key, c => c.ct);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<PagedResultDto<MessageDto>> GetAllAsync(GetAllInput input)
        {
            //var u = new Abp.UserIdentifier(base.AbpSession.TenantId, AbpSession.UserId.Value);
            //r.TotalCount = await userNotificationManager.GetUserNotificationCountAsync(u, input.UserNotificationState, input.StartTime, input.EndTime);
            //r.Items = await userNotificationManager.GetUserNotificationsAsync(u, input.UserNotificationState, input.SkipCount, input.MaxResultCount, input.StartTime, input.EndTime);
            //UserNotificationInfo
            var r = new PagedResultDto<MessageDto>();
            var query = GetQuery(input);
            r.TotalCount = await query.CountAsync();
            query = query.OrderBy(input.Sorting).PageBy(input);

            var items = await query.ToListAsync();
            r.Items = ObjectMapper.Map<List<MessageDto>>(items);
            return r;
        }

        private IQueryable<temp> GetQuery(GetTotalInput input)
        {
            var query = from c in UserNotificationRepository.GetAll().AsNoTrackingWithIdentityResolution()
                        join d in TenantNotificationRepository.GetAll().AsNoTrackingWithIdentityResolution() on c.TenantNotificationId equals d.Id into temp
                        from e in temp.DefaultIfEmpty()
                        join f in UserRepository.GetAll().AsNoTrackingWithIdentityResolution() on e.CreatorUserId equals f.Id into ut
                        from g in ut.DefaultIfEmpty()
                        where c.UserId == AbpSession.UserId
                        select new temp { TenantNotificationInfo = e, UserNotificationInfo = c, CreateUserName = g.FullName };

            if (input.NotificationNames != null && input.NotificationNames.Any())
            {
                if (input.NotificationNameContains)
                    query = query.Where(c => input.NotificationNames.Contains(c.TenantNotificationInfo.NotificationName));
                else
                    query = query.Where(c => !input.NotificationNames.Contains(c.TenantNotificationInfo.NotificationName));
            }

            query = query.WhereIf(input.UserNotificationState.HasValue, c => c.UserNotificationInfo.State == input.UserNotificationState.Value)
                         .WhereIf(input.StartTime.HasValue, c => c.TenantNotificationInfo.CreationTime >= input.StartTime)
                         .WhereIf(input.EndTime.HasValue, c => c.TenantNotificationInfo.CreationTime < input.EndTime)
                         .WhereIf(input.EntityTypeName.IsNotNullOrWhiteSpaceBXJG(),c=>c.TenantNotificationInfo.EntityTypeName== input.EntityTypeName)
                         .WhereIf(input.EntityId.IsNotNullOrWhiteSpaceBXJG(),c=>c.TenantNotificationInfo.EntityId== input.EntityId)
                         .WhereIf(input.NotificationSeverities != null && input.NotificationSeverities.Any(), c => input.NotificationSeverities.Any(d => d == c.TenantNotificationInfo.Severity));
            //var sql = query.ToQueryString();
            return query;
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
                    using var sw = base.UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await userNotificationManager.Value.UpdateUserNotificationStateAsync(AbpSession.TenantId, item, UserNotificationState.Read);
                    await sw.CompleteAsync();
                    r.Ids.Add(item);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(item, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(item.Message500());
                    Logger.Warn($"部分通知设置读取状态失败！{item}", ex);
                }
            }
            return r;
        }

        //获取当前用户可以订阅的通知列表

        //订阅通知

        //取消订阅通知
    }
}
