using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Abp.Web.Security.AntiForgery;
using ZLJ.Controllers;
using Abp.RealTime;
using Microsoft.AspNetCore.Authorization;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ZLJ.Web.Host.Controllers
{
    public class HomeController : ZLJControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        INotificationConfiguration sdf;
        private readonly IOnlineClientManager _onlineClientManager;

        public HomeController(INotificationPublisher notificationPublisher, INotificationConfiguration sdf, IOnlineClientManager onlineClientManager)
        {
            _notificationPublisher = notificationPublisher;
            this.sdf = sdf;
            _onlineClientManager = onlineClientManager;
        }
        //https://github.com/dotnet/aspnetcore/blob/a450cb69b5e4549f5515cdb057a68771f56cefd7/src/Identity/Core/src/IdentityConstants.cs
        // [AbpMvcAuthorize]



        /*
         * 这里统一分发到不同应用的视图或请求
         * 为什么不在中间件中分发请求？
         * 因为比较是单体应用，中间件是所有请求都要执行的，而我们决定使用blazor，这种应用只需要导航一次
         * webapi不需要导航
         * mvc也只需要导航一次
         * webapi mvc razorpage在单体中，所以注意路由全局不能冲突，而blazor server木有这个现在，因为它是单页面应用，有自己的路由
         * 相比用razorPages来导航，mvc可以指向到rcl中的视图
         */

        public IActionResult Index()
        {
            //return Redirect("/swagger");

                return View($"_Host");
        }
        ////public IActionResult Blazor()
        ////{
        ////    return View("_host");
        ////}
        ///// <summary>
        ///// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        ///// Don't use this code in production !!!
        ///// </summary>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public async Task<ActionResult> TestNotification(string message = "")
        //{
        //    var onlineClients = _onlineClientManager.GetAllByUserId(new UserIdentifier(1, 2));
        //    var sf = sdf.Notifiers;
        //    if (message.IsNullOrEmpty())
        //    {
        //        message = "This is a test notification, created at " + Clock.Now;
        //    }

        //    var defaultTenantAdmin = new UserIdentifier(1, 2);
        //    var hostAdmin = new UserIdentifier(null, 1);

        //    await _notificationPublisher.PublishAsync(
        //        "App.SimpleMessage",
        //        new MessageNotificationData(message),
        //        severity: NotificationSeverity.Info,
        //        userIds: new[] { defaultTenantAdmin, hostAdmin }
        //    );

        //    return Content("Sent notification: " + message);
        //}

       
    }
}
