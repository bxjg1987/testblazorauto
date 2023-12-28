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
using Abp.Auditing;
using BXJG.Utils.Application.Share.Notification;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 通用个人消息管理服务
    /// 由于通知是与个人相关的数据，所以不需要严格的权限控制
    /// 此功能是所有类型的用户通用的
    /// </summary>
    [AbpAuthorize]
    public abstract class PersonNotificationAppService<TUser> : ApplicationService where TUser : AbpUserBase
    {
        public Lazy<IRepository<TUser, long>> userRepository { get; set; }
        protected IRepository<TUser, long> UserRepository => userRepository.Value;

        public Lazy<IUserNotificationManager> userNotificationManager { get; set; }
        protected IUserNotificationManager UserNotificationManager => userNotificationManager.Value;

        public Lazy<IRepository<UserNotificationInfo, Guid>> userNotificationRepository { get; set; }
        protected IRepository<UserNotificationInfo, Guid> UserNotificationRepository => userNotificationRepository.Value;

        public Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository { get; set; }
        protected IRepository<TenantNotificationInfo, Guid> TenantNotificationRepository => tenantNotificationRepository.Value;

        public Lazy<INotificationDefinitionManager> notificationDefinitionManager { get; set; }
        protected INotificationDefinitionManager NotificationDefinitionManager => notificationDefinitionManager.Value;

        public Lazy<INotificationSubscriptionManager> notificationSubscriptionManager { get; set; }
        protected INotificationSubscriptionManager NotificationSubscriptionManager => notificationSubscriptionManager.Value;

        public PersonNotificationAppService()
        {
            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 获取当前用户可以订阅的通知定义列表
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        [DisableAuditing]
        public virtual async Task<List<NotifyDefineDto>> GetDefines()
        {
            var list = await NotificationDefinitionManager.GetAllAvailableAsync(base.AbpSession.ToUserIdentifier());
            return base.ObjectMapper.Map<List<NotifyDefineDto>>(list);
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

            foreach (var subscript in subscripts)
            {
                EntityIdentifier? entityIdentifier = default;
                if (subscript.EntityTypeName.IsNotNullOrWhiteSpaceBXJG())
                {
                    entityIdentifier = new EntityIdentifier(Type.GetType(subscript.EntityTypeName), subscript.EntityId);
                }
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await NotificationSubscriptionManager.SubscribeAsync(base.AbpSession.ToUserIdentifier(), subscript.NotifyName, entityIdentifier);
                    await uow.CompleteAsync();
                    r.Ids.Add(subscript);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(subscript, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(subscript.Message500());
                    Logger.Warn($"部分订阅失败！{subscript}", ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 批量取消订阅
        /// </summary>
        /// <param name="subscripts"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutput<SubscriptNotifyItem>> UnSubscript(List<SubscriptNotifyItem> subscripts)
        {
            var r = new BatchOperationOutput<SubscriptNotifyItem>();
            foreach (var subscript in subscripts)
            {
                EntityIdentifier? entityIdentifier = default;
                if (subscript.EntityTypeName.IsNotNullOrWhiteSpaceBXJG())
                {
                    entityIdentifier = new EntityIdentifier(Type.GetType(subscript.EntityTypeName), subscript.EntityId);
                }
                try
                {
                    using var uow = UnitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew);
                    await NotificationSubscriptionManager.UnsubscribeAsync(base.AbpSession.ToUserIdentifier(), subscript.NotifyName, entityIdentifier);
                    await uow.CompleteAsync();
                    r.Ids.Add(subscript);
                }
                catch (UserFriendlyException ex)
                {
                    r.ErrorMessage.Add(new BatchOperationErrorMessage(subscript, ex.Message));
                }
                catch (Exception ex)
                {
                    r.ErrorMessage.Add(subscript.Message500());
                    Logger.Warn($"部分取消订阅失败！{subscript}", ex);
                }
            }
            return r;
        }
        /// <summary>
        /// 获取已订阅的通知定义
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        [DisableAuditing]
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
        [DisableAuditing]
        public virtual async Task<int> GetTotalAsync(GetTotalInput input)
        {
            var query = GetQuery(input);
            return await query.CountAsync();
        }

        /// <summary>
        /// 获取消息数量
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        [DisableAuditing]
        public virtual async Task<Dictionary<SubscriptNotifyItem, int>> GetUnReadTotalGroupBySubscript(params string[] names)
        {
            var query = GetQuery(new GetTotalInput { NotificationNames = names, UserNotificationState = UserNotificationState.Unread }).GroupBy(c => new SubscriptNotifyItem { NotifyName = c.TenantNotificationInfo.NotificationName, EntityTypeName = c.TenantNotificationInfo.EntityTypeName, EntityId = c.TenantNotificationInfo.EntityId }).Select(c => new { c.Key, ct = c.Count() });
            return await query.ToDictionaryAsync(c => c.Key, c => c.ct);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        [DisableAuditing]
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

        protected virtual IQueryable<temp> GetQuery(GetTotalInput input)
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
                         .WhereIf(input.EntityTypeName.IsNotNullOrWhiteSpaceBXJG(), c => c.TenantNotificationInfo.EntityTypeName == input.EntityTypeName)
                         .WhereIf(input.EntityId.IsNotNullOrWhiteSpaceBXJG(), c => c.TenantNotificationInfo.EntityId == input.EntityId)
                         .WhereIf(input.Keywords.IsNotNullOrWhiteSpaceBXJG(), c => c.TenantNotificationInfo.Data.Contains(input.Keywords))
                         .WhereIf(input.NotificationSeverities != null && input.NotificationSeverities.Any(), c => input.NotificationSeverities.Any(d => d == c.TenantNotificationInfo.Severity));
            //var sql = query.ToQueryString();
            return query;
        }

        /// <summary>
        /// 将指定消息设置为已读
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public virtual async Task<BatchOperationOutput<Guid>> SetReadedAsync(BatchOperationInput<Guid> input)
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
        /// <summary>
        /// 设置所有消息为已读
        /// </summary>
        /// <param name="name">通知类型名</param>
        /// <returns></returns>
        public virtual async Task SetReadedAllAsync(string name = default)
        {
            if (name.IsNullOrWhiteSpaceBXJG())
                await UserNotificationManager.UpdateAllUserNotificationStatesAsync(AbpSession.ToUserIdentifier(), UserNotificationState.Read);
            else
            {
                var msgs = await UserNotificationManager.GetUserNotificationsAsync(AbpSession.ToUserIdentifier(), UserNotificationState.Unread);
                await SetReadedAsync(new BatchOperationInput<Guid> { Ids = msgs.Where(c => c.Notification.NotificationName == name).Select(c => c.Id).ToArray() });
            }
        }
    }
}