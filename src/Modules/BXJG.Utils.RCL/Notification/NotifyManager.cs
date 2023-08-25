using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Notifications;
using BXJG.Common.Dto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BXJG.Utils.Notification
{
    /// <summary>
    /// 通知管理组件抽象类
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TAppService"></typeparam>
    public class NotifyManager<TUser, TAppService> : AbpBaseComponent
        where TUser : AbpUser<TUser>
        where TAppService : PersonNotificationAppService<TUser>
    {
        /// <summary>
        /// 当前选择的通知定义
        /// </summary>
        protected NotifyDefineDto? currDefine => defines.SingleOrDefault(d => d.Selected);
        /// <summary>
        /// 通知定义列表
        /// </summary>
        protected List<NotifyDefineDto> defines = new List<NotifyDefineDto>();

        /// <summary>
        /// 当前页码
        /// </summary>
        protected int pageIndex => condition.SkipCount / condition.MaxResultCount + 1;
        ///// <summary>
        ///// 页大小
        ///// </summary>
        //protected int pageSize =>condition.MaxResultCount;
        /// <summary>
        /// 当前消息列表的总行数
        /// </summary>
        protected int total = 0;
        /// <summary>
        /// 过滤条件
        /// </summary>
        protected GetAllInput condition = new GetAllInput();
        /// <summary>
        /// 消息列表
        /// </summary>
        protected List<MessageDto> messages = new List<MessageDto>();

        /// <summary>
        /// 通知应用服务
        /// </summary>
        public TAppService AppService { get; set; }
        protected override void OnInitialized2()
        {
            base.OnInitialized2();
            AppService = base.ScopedServices.GetRequiredService<TAppService>();
        }
        protected override async Task OnInitialized2Async()
        {
            await LoadDefinesAsync();
        }
        /// <summary>
        /// 加载头部列表及其未读数量
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadDefinesAsync()
        {
            var r = AppService.GetDefines();//可订阅的列表
            var r1 = AppService.GetSubscriptedNotifyDefines();//已订阅的列表
            var sl = AppService.GetUnReadTotalGroupBySubscript(defines.Select(c => c.Name).ToArray());//按通知类型分组的未读数量
            await Task.WhenAll(r, r1, sl);

            defines = r.Result.Where(c => r1.Result.Any(d => d.NotificationName == c.Name)).Select(c => new NotifyDefineDto
            {
                Attributes = c.Attributes,
                Description = c.Description,
                DisplayName = c.DisplayName,
                EntityType = c.EntityType,
                Name = c.Name,
                UnReadCount = sl.Result.Where(d => d.Key.NotifyName == c.Name).Sum(c => c.Value),
                Selected = false
            }).ToList();

            //foreach (var item in defines)
            //{
            //    base.Logger.Debug(System.Text.Json.JsonSerializer.Serialize(item));
            //}
            if (defines.Any())
            {
                await HeadChanged(defines.First().Name);
            }
        }
        /// <summary>
        /// 当选择的通知类型变化时执行
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual async Task HeadChanged(string name)
        {
            if (currDefine?.Name == name)
                return;

            await base.SafeExecute(async () =>
            {
                if (currDefine != default)
                    currDefine.Selected = false;

                defines.Single(c => c.Name == name).Selected = true;
                condition.NotificationNames = new[] { name };
                await ConditionChanged();
            });
        }
        /// <summary>
        /// 根据当前条件加载消息列表
        /// </summary>
        /// <returns></returns>
        protected virtual async Task LoadMessagesAsync()
        {
            await SafeExecute(async () =>
            {
                var r = await AppService.GetAllAsync(condition);
                total = r.TotalCount;
                messages = r.Items.ToList();
            });
        }
        /// <summary>
        /// 条件-开始时间变化时调用
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        protected virtual async Task StartTimeChanged(DateTime? startTime)
        {
            condition.StartTime = startTime;
            await ConditionChanged();
        }
        //结束时间变化时调用
        protected virtual async Task EndTimeChanged(DateTime? endTime)
        {
            condition.EndTime = endTime;
            await ConditionChanged();
        }
        /// <summary>
        /// 条件-紧急程度变化时调用
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual async Task SeverityChanged(NotificationSeverity val)
        {
            condition.NotificationSeverities = new[] { val };
            await ConditionChanged();
        }

        /// <summary>
        /// 读取状态变化时调用
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected virtual async Task ReadStateChanged(UserNotificationState? val)
        {
            condition.UserNotificationState = val;
            await ConditionChanged();
        }

        /// <summary>
        /// 条件-关键字变化时调用
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        protected virtual async Task KeywordsChanged(string keywords)
        {
            condition.Keywords = keywords;
            await ConditionChanged();
        }
        /// <summary>
        /// 条件有任何变化时调用
        /// </summary>
        /// <returns></returns>
        protected virtual async Task ConditionChanged()
        {
            condition.SkipCount = 0;
            await LoadMessagesAsync();
        }
        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        protected virtual async Task SetReadAsync(params Guid[] ids)
        {
            await SafeExecute(async () =>
            {
                await AppService.SetReadedAsync(new BatchOperationInput<Guid> { Ids = ids });
            });
        }

        /// <summary>
        /// 标记为已读
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual async Task SetReadAllAsync(string? name = default)
        {
            await SafeExecute(async () =>
            {
                await AppService.SetReadedAllAsync(name);
            });
        }

        /// <summary>
        /// tab头
        /// 通知定义，扩展应用层的通知定义dto
        /// </summary>
        protected class NotifyDefineDto : BXJG.Utils.Notification.NotifyDefineDto
        {
            /// <summary>
            /// 未读数量
            /// </summary>
            public int UnReadCount { get; set; }
            /// <summary>
            /// 已选择
            /// </summary>
            public bool Selected { get; set; }
        }
    }
}