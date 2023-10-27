using Abp.Notifications;
using BootstrapBlazor.Components;
using BXJG.Common;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.BootstrapServer.Shared
{
    public partial class App
    {
        [Inject]
        protected MessageService MessageService { get; private set; }
        [Inject]
        protected Castle.Core.Logging.ILoggerFactory LoggerFactory { get; private set; }
        /// <summary>
        /// 这里的错误仅仅是兜底，错误后当前页面的控件状态很可能无法恢复，我们通过肉夹馍的aop实现了统一异常处理
        /// 参考文档中的详细描述，或者 https://www.cnblogs.com/jionsoft/p/17783675.html
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private async Task OnErrorHandleAsync(ILogger logger, Exception ex)
        {

            var l = LoggerFactory.Create("App");
            if (ex is UserFriendlyException uex)
            {
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = uex.Message,
                    ShowBorder = true,
                    ShowShadow = true
                });
            }
            else
            {
                l.Error("未处理异常！", ex);
                await MessageService.Show(new MessageOption
                {
                    Color = Color.Danger,
                    Content = "发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。",
                    ShowBorder = true,
                    ShowShadow = true
                });
            }
        }
        IDisposable xxtz;//消息通知释放对象
        protected override void OnInitialized()
        {
            base.OnInitialized();
            xxtz = base.Zhongjie.Zhuce<UserNotification>(ShowUsernotification);
        }
        protected override void Dispose(bool disposing)
        {
            xxtz?.Dispose();
            base.Dispose(disposing);
        }
        private async ValueTask ShowUsernotification(UserNotification userNotification)
        {
            string icon = string.Empty;
            Color color = Color.None;
            switch (userNotification.Notification.Severity)
            {
                case NotificationSeverity.Info:
                    color = Color.Info;
                    icon = "fas fa-info";
                    break;
                case NotificationSeverity.Success:
                    color = Color.Success;
                    icon = "fas fa-check";
                    break;
                case NotificationSeverity.Warn:
                    color = Color.Warning;
                    icon = "fas fa-triangle-exclamation";
                    break;
                case NotificationSeverity.Error:
                    color = Color.Danger;
                    icon = "fas fa-xmark";
                    break;
                case NotificationSeverity.Fatal:
                    color = Color.Danger;
                    icon = "fas fa-xmark";
                    break;
            }

            string msg = string.Empty;
            if (userNotification.Notification.Data is MessageNotificationData xx)
                msg = xx.Message;
            else
                msg = "未知的通知内容！";
            await MessageService.Show(new MessageOption
            {
                Color = color,
                Content = msg,
                ShowBar = true,
                ShowBorder = true,
                ShowShadow = true,
                Icon = icon
            });
        }
    }
}
