using Abp.Notifications;
using BXJG.Common;
using BXJG.Common.RCL;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.Web.Admin.Shared
{
    public partial class App
    {
        [Inject]
        protected INotificationService MessageService { get; private set; }
        /// <summary>
        /// 它是范围注册的，可以直接注入
        /// </summary>
        [Inject]
        protected CircuitStateHandler CircuitStateHandler { get; private set; }
        [Inject]
        protected CircuitStateContainer CircuitStateContainer { get; private set; }


        BlazorServerContext context;
        
      //  ILogger

        ///// <summary>
        ///// 这里的错误仅仅是兜底，错误后当前页面的控件状态很可能无法恢复，我们通过肉夹馍的aop实现了统一异常处理
        ///// 参考文档中的详细描述，或者 https://www.cnblogs.com/jionsoft/p/17783675.html
        ///// </summary>
        ///// <param name="logger"></param>
        ///// <param name="ex"></param>
        ///// <returns></returns>
        //private async Task OnErrorHandleAsync(ILogger logger, Exception ex)
        //{

       
        //    if (ex is UserFriendlyException uex)
        //    {
        //        await MessageService.Show(new MessageOption
        //        {
        //            Color = Color.Danger,
        //            Content = uex.Message,
        //            ShowBorder = true,
        //            ShowShadow = true
        //        });
        //    }
        //    else
        //    {
        //        logger.LogError(ex,"未处理异常！");
        //        await MessageService.Show(new MessageOption
        //        {
        //            Color = Color.Danger,
        //            Content = "发生未处理异常！请稍后重试，若多次失败，请联系系统管理员。",
        //            ShowBorder = true,
        //            ShowShadow = true
        //        });
        //    }
        //}
        IDisposable xxtz;//消息通知释放对象
        protected override void OnInitialized()
        {
            base.OnInitialized();

           // var container = ScopedServices.GetRequiredService<CircuitStateContainer>();//不晓得为啥，必须用注入方式，这样获取不到

            //var cir = ScopedServices.GetRequiredService<CircuitStateHandler>();//这样获取的，Current属性为空

            context = CircuitStateContainer[CircuitStateHandler.Current];

            xxtz = context.Zhongjie.Zhuce<UserNotification>(ShowUsernotification);
        }
        public  void Dispose()
        {
            xxtz?.Dispose();
        }
        private async ValueTask ShowUsernotification(UserNotification userNotification)
        {
           // string icon = string.Empty;
            NotificationType color = NotificationType.None;
            switch (userNotification.Notification.Severity)
            {
                case NotificationSeverity.Info:
                    color = NotificationType.Info;
                    break;
                case NotificationSeverity.Success:
                    color = NotificationType.Success;
                    break;
                case NotificationSeverity.Warn:
                    color = NotificationType.Warning;
                    break;
                case NotificationSeverity.Error:
                    color = NotificationType.Error;
                    break;
                case NotificationSeverity.Fatal:
                    color = NotificationType.Error;
                    break;
            }

            string msg = string.Empty;
            if (userNotification.Notification.Data is MessageNotificationData xx)
                msg = xx.Message;
            else
                msg = "未知的通知内容！";

            await InvokeAsync(async () =>
            {
                await MessageService.Open(new NotificationConfig
                {
                    Message = userNotification.Notification.NotificationName ,
                    Description = msg,//"This is the content of the notification. This is the content of the notification. This is the content of the notification.",
                    NotificationType = color
                });
            });

        }
    }
}
