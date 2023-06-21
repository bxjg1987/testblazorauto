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
    /// </summary>
    [AbpAuthorize]
    public class PersonNotificationAppService<TUser> : ApplicationService where TUser : AbpUserBase
    {
        protected readonly IRepository<TUser,long> userRepository;
        protected readonly Lazy<IUserNotificationManager> userNotificationManager;
        //private readonly INotificationDefinitionManager 
        protected readonly Lazy<IRepository<UserNotificationInfo, Guid>> userNotificationRepository;
        protected readonly Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository;

        protected readonly Lazy<INotificationDefinitionManager> notificationDefinitionManager;

        public PersonNotificationAppService(Lazy<IUserNotificationManager> userNotificationManager,
                                            Lazy<IRepository<UserNotificationInfo, Guid>> repository,
                                            Lazy<IRepository<TenantNotificationInfo, Guid>> tenantNotificationRepository,
                                            IRepository<TUser, long> userRepository,
                                            Lazy<INotificationDefinitionManager> notificationDefinitionManager)
        {
            this.userNotificationManager = userNotificationManager;
            this.userNotificationRepository = repository;
            this.tenantNotificationRepository = tenantNotificationRepository;
            this.userRepository = userRepository;
            this.notificationDefinitionManager = notificationDefinitionManager;

            base.LocalizationSourceName = BXJGUtilsConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 获取当前用户可访问的消息定义
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<NotifyDefineDto>> GetDefines()
        {
            var list = await notificationDefinitionManager.Value.GetAllAvailableAsync(base.AbpSession.ToUserIdentifier());
            var r = new List<NotifyDefineDto>();
            foreach (var item in list)
            {
                var temp = new NotifyDefineDto
                {
                    Name = item.Name,
                    DisplayName = item.DisplayName.Localize(LocalizationManager),
                    EntityType = item.EntityType.FullName,
                    Description = item.Description.Localize(LocalizationManager),
                    Attributes = item.Attributes
                };
                r.Add(temp);
            }
            return r;
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
        /// 获取消息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(false)]
        public virtual async Task<List<MessageDto>> GetAllAsync(GetAllInput input)
        {
            //var u = new Abp.UserIdentifier(base.AbpSession.TenantId, AbpSession.UserId.Value);
            //r.TotalCount = await userNotificationManager.GetUserNotificationCountAsync(u, input.UserNotificationState, input.StartTime, input.EndTime);
            //r.Items = await userNotificationManager.GetUserNotificationsAsync(u, input.UserNotificationState, input.SkipCount, input.MaxResultCount, input.StartTime, input.EndTime);
            //UserNotificationInfo

            var query = GetQuery(input);
            query = query.OrderBy(input.Sorting).PageBy(input);

            var items = await query.ToListAsync();
            var tempList = new List<MessageDto>();
            foreach (var item in items)
            {
                //var tn = ObjectMapper.Map<TenantNotification>(item.e);
                //var un = ObjectMapper.Map<UserNotification>(item.c);
                //un.Notification = tn;
                //  MessageNotificationData
                //Abp.Notifications.MessageNotificationData


                //ZLJ.Notification.MessageNotificationData data = JsonConvert.DeserializeObject<ZLJ.Notification.MessageNotificationData>(item.TenantNotificationInfo.Data);
                var temp = new MessageDto
                {
                    UserNotificationInfoId = item.UserNotificationInfo.Id,
                    //Title = data.Title,
                    //Content = data.Message, 
                    State = item.UserNotificationInfo.State,
                    Data = item.TenantNotificationInfo.Data,
                    Name = item.TenantNotificationInfo.NotificationName,
                    Severity = item.TenantNotificationInfo.Severity,
                    CreationTime = item.UserNotificationInfo.CreationTime,
                    CreateUserName = item.CreateUserName,
                };
                //var temp = new UserNotification
                //{
                //    Id = item.UserNotificationInfo.Id,
                //    State = item.UserNotificationInfo.State,
                //    TenantId = item.UserNotificationInfo.TenantId,
                //    UserId = item.UserNotificationInfo.UserId,
                //    Notification = new TenantNotification
                //    {
                //        CreationTime = item.UserNotificationInfo.CreationTime,
                //        Data = System.Text.Json.JsonSerializer.Deserialize<NotificationData>(item.TenantNotificationInfo.Data),
                //        EntityId = item.TenantNotificationInfo.EntityId,
                //        EntityTypeName = item.TenantNotificationInfo.EntityTypeName,
                //        Id = item.TenantNotificationInfo.Id,
                //        NotificationName = item.TenantNotificationInfo.NotificationName,
                //        Severity = item.TenantNotificationInfo.Severity,
                //        TenantId = item.TenantNotificationInfo.TenantId
                //    }
                //};
                tempList.Add(temp);
            }
            return tempList;
        }

        private IQueryable<temp> GetQuery(GetTotalInput input)
        {
            var query = from c in userNotificationRepository.Value.GetAll().AsNoTrackingWithIdentityResolution()
                        join d in tenantNotificationRepository.Value.GetAll().AsNoTrackingWithIdentityResolution() on c.TenantNotificationId equals d.Id into temp
                        from e in temp.DefaultIfEmpty()
                        join f in userRepository.GetAll().AsNoTrackingWithIdentityResolution() on e.CreatorUserId equals f.Id into ut
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
                         .WhereIf(input.NotificationSeverities != null && input.NotificationSeverities.Any(), c => input.NotificationSeverities.Any(d => d == c.TenantNotificationInfo.Severity));
            //var sql = query.ToQueryString();
            return query;
        }

        //元祖可以用，但不支持动态排序
        class temp
        {
            public TenantNotificationInfo TenantNotificationInfo { get; set; }
            public UserNotificationInfo UserNotificationInfo { get; set; }
            public string CreateUserName { get; set; }
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
                    r.Ids.Add(item);
                    await sw.CompleteAsync();
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
