using Abp.Notifications;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Notification
{
    /// <summary>
    /// 通用的消息列表模型
    /// </summary>
    public class MessageDto
    {
        public Guid UserNotificationInfoId { get; set; }

        //public Guid TenantNotificationInfoId { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string TenantNotificationInfoNotificationName { get; set; }
        /// <summary>
        /// 通知数据
        /// </summary>
        public string TenantNotificationInfoData { get; set; }
        /// <summary>
        /// 通知数据类型名称
        /// </summary>
        public string TenantNotificationInfoDataTypeName { get; set; }
        /// <summary>
        /// 消息状态
        /// </summary>
        public UserNotificationState UserNotificationInfoState { get; set; }
        /// <summary>
        /// 消息级别
        /// </summary>
        public NotificationSeverity TenantNotificationInfoSeverity { get; set; }
        ///// <summary>
        ///// 发布人
        ///// </summary>
        //public string UserNotificationInfoCreationTime { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime UserNotificationInfoCreationTime { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 发布显示时间
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                //这里的逻辑应结合abp本地化，放在BXJG.Utils包中，做为扩展方法

                var sss = Clock.Now - this.UserNotificationInfoCreationTime;

                if (sss.TotalSeconds <= 60)
                    return "刚刚";
                if (sss.TotalMinutes <= 60)
                    return $"{Convert.ToInt32(sss.TotalMinutes)}分钟前";
                if (sss.TotalHours <= 24)
                    return $"{Convert.ToInt32(sss.TotalHours)}小时前";
                if (sss.TotalDays <= 7)
                    return $"{Convert.ToInt32(sss.TotalDays)}天前";
                if (sss.TotalDays <= 30)
                    return $"{Convert.ToInt32(sss.TotalDays / 7)}周前";
                if (sss.TotalDays <= 365)
                    return $"{Convert.ToInt32(sss.TotalDays / 30)}个月前";

                return $"{Convert.ToInt32(sss.TotalDays / 365)}年前";
                // return Clock.Now.Subtract(this.CreationTime).Duration().Days + "天前";
            }
        }

    }
}
