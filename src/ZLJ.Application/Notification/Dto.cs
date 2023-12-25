using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Notification
{
    /// <summary>
    /// 获取消息数量的输入模型
    /// </summary>
    public class GetTotalInput
    {
        /// <summary>
        /// 通知类型，参考NotificationNameContains属性，配合使用
        /// 不提供此参数或为空则不限制此条件
        /// </summary>
        public IEnumerable<string> NotificationNames { get; set; }
        /// <summary>
        /// true表示获取NotificationNames中指定的类型的通知
        /// false表示获取除NotificationNames中指定的类型之外的其它类型的通知
        /// NotificationNames为空时，此参数无意义。
        /// </summary>
        public bool NotificationNameContains { get; set; } = true;
        /// <summary>
        /// 读取状态 0未读 1已读 不提供此参数则不限制此条件
        /// </summary>
        public UserNotificationState? UserNotificationState { get; set; }
        /// <summary>
        /// 发布时间范围的开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 发布时间范围的开始结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 只查询指定级别的通知，空则不限制
        /// </summary>
        public IEnumerable<NotificationSeverity> NotificationSeverities { get; set; }
    }
    /// <summary>
    /// 获取通知列表的输入模型
    /// </summary>
    public class GetAllInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public GetTotalInput GetTotalInput { get; set; } = new GetTotalInput();
        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "UserNotificationInfo.CreationTime desc, TenantNotificationInfo.Severity asc";
        }
    }
}
