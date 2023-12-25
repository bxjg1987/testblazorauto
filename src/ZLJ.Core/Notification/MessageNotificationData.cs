using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Notification
{
    /// <summary>
    /// 简易的，通用的消息通知模型
    /// </summary>
    public class MessageNotificationData : Abp.Notifications.MessageNotificationData
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        //[Obsolete("直接使用Message属性")]
        //public string Content => base.Message;
        public MessageNotificationData(string title, string content) : base(content)
        {
            Title = title;
            base.Message = content;
        }
    }
}
