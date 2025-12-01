using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Notifications
{
    /// <summary>
    /// 带标题的消息通知数据
    /// </summary>
    public class MessageNotificationWithTitleData : MessageNotificationData
    {
        public string? Title { 
            get=>Properties .ContainsKey("Title")? Properties["Title"] .ToString():string.Empty;
            set=>Properties["Title"] = value;
        }
        public MessageNotificationWithTitleData(string message,string? title=default) : base(message)
        {
            Title = title;
        }
    }
}
