using Abp.Notifications;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Notification
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
        public string Name { get; set; }
        /// <summary>
        /// 通知数据
        /// </summary>
        public string Data { get; set; }
        
        /// <summary>
        /// 消息状态
        /// </summary>
        public UserNotificationState State { get; set; }
        /// <summary>
        /// 消息级别
        /// </summary>
        public NotificationSeverity Severity { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 发布显示时间
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                //这里的逻辑应结合abp本地化，放在BXJG.Utils包中，做为扩展方法

                var sss = Clock.Now - this.CreationTime;

                if (sss.TotalSeconds <= 60)
                    return "刚刚";
                else if (sss.TotalMinutes <= 60)
                    return $"{sss.TotalMinutes}分钟前";
                else if (sss.TotalHours <= 24)
                    return $"{sss.TotalHours}小时前";
                else if (sss.TotalDays <= 7)
                    return $"{sss.TotalDays}天前";
                else if (sss.TotalDays <= 30)
                    return $"{Convert.ToInt32(sss.TotalDays / 7)}周前";
                else if (sss.TotalDays <= 365)
                    return $"{Convert.ToInt32(sss.TotalDays / 30)}个月前";
                else
                    return $"{Convert.ToInt32(sss.TotalDays / 365)}年前";
                // return Clock.Now.Subtract(this.CreationTime).Duration().Days + "天前";
            }
        }
        

    }
}
