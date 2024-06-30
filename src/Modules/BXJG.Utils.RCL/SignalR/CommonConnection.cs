using Abp.Notifications;
using BXJG.Common.Events;
using BXJG.Common.Http;
using Force.DeepCloner;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.RCL.SignalR
{
    //public static class CommonHubExt
    //{
    //    //public const string CommonSignalR = "CommonSignalR";
    //    /// <summary>
    //    /// 注册abp通知处理器
    //    /// 当通知抵达时，通过zhongjie触发事件
    //    /// </summary>
    //    /// <param name="Connection"></param>
    //    /// <param name="zhongjie"></param>
    //    /// <param name="logger"></param>
    //    /// <returns></returns>
    //    public static HubConnection RegisterUserNotification(this HubConnection Connection, Zhongjie zhongjie, ILogger? logger = null)
    //    {
    //        if (logger == default)
    //            logger = NullLogger.Instance;
    //        Connection.On<Abp.Notifications.UserNotification>("getNotification", async msg =>
    //        {
    //            logger.LogDebug($"通知连接接受到消息{System.Text.Json.JsonSerializer.Serialize(msg)}，准备触发事件。");
    //            //var str = System.Text.Json.JsonSerializer.Serialize(msg.Notification.Data.Properties);
    //            string eventName = default;
    //            if (msg.Notification.Data.Properties.Count == 1 && msg.Notification.Data.Properties.TryGetValue("Message", out var jg))
    //            {
    //                msg.Notification.Data = new MessageNotificationData(jg.ToString());
    //                eventName = nameof(MessageNotificationData);

    //            }
    //            else if (msg.Notification.Data.Properties.Count == 2 &&
    //                     msg.Notification.Data.Properties.TryGetValue("SourceName", out var jg1) &&
    //                     msg.Notification.Data.Properties.TryGetValue("Name", out var jg2))
    //            {
    //                msg.Notification.Data = new LocalizableMessageNotificationData(new Abp.Localization.LocalizableString(jg1.ToString(), jg2.ToString()));
    //                eventName = nameof(LocalizableMessageNotificationData);
    //            }
    //            await zhongjie.Chufa(msg.Notification, eventName);
    //        });
    //        return Connection;
    //    }

    //    //public static HttpConnectionOptions SetDefault(this HttpConnectionOptions opt, IAccessTokenProvider accessTokenProvider)
    //    //{
    //    //    opt.AccessTokenProvider = () => Task.FromResult(accessTokenProvider.GetEncryptedAccessToken());
    //    //    return opt;
    //    //}
    //}
    /*
     * abp中通常有个全局的signalR
     * 通过这里的类去连接它
     * 
     * 在server模式时scope
     * wasm时，单例
     * 可以统一搞成scope
     */

    public class CommonConnection : IAsyncDisposable
    {
        public HubConnection Connection { get; private set; }
        IAccessTokenProvider accessTokenProvider;
        Zhongjie zhongjie;
        ILogger logger;

        public CommonConnection(IAccessTokenProvider accessTokenProvider, Zhongjie zhongjie, ILogger<CommonConnection> logger)
        {
            this.accessTokenProvider = accessTokenProvider;
            this.zhongjie = zhongjie;
            this.logger = logger;
        }

        public async ValueTask DisposeAsync()
        {
            if (Connection != default)
                await Connection.DisposeAsync();
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken = default)
        {
            Connection = new HubConnectionBuilder().WithUrl("http://localhost:21021/signalr", x =>
                                                   {
                                                       x.AccessTokenProvider = () => Task.FromResult(accessTokenProvider.GetEncryptedAccessToken());
                                                   })
                                                   .WithAutomaticReconnect()
                                                   .Build();
            Connection.On<Abp.Notifications.UserNotification>("getNotification", async msg =>
            {
                logger.LogDebug($"通知连接接受到消息{System.Text.Json.JsonSerializer.Serialize(msg)}，准备触发事件。");
                //var str = System.Text.Json.JsonSerializer.Serialize(msg.Notification.Data.Properties);
                string eventName = default;
                if (msg.Notification.Data.Properties.Count == 1 && msg.Notification.Data.Properties.TryGetValue("Message", out var jg))
                {
                    msg.Notification.Data = new MessageNotificationData(jg.ToString());
                    eventName = nameof(MessageNotificationData);

                }
                else if (msg.Notification.Data.Properties.Count == 2 &&
                         msg.Notification.Data.Properties.TryGetValue("SourceName", out var jg1) &&
                         msg.Notification.Data.Properties.TryGetValue("Name", out var jg2))
                {
                    msg.Notification.Data = new LocalizableMessageNotificationData(new Abp.Localization.LocalizableString(jg1.ToString(), jg2.ToString()));
                    eventName = nameof(LocalizableMessageNotificationData);
                }
                await zhongjie.Chufa(msg.Notification, eventName);
            });
            await Connection.StartAsync(stoppingToken);
        }

        // public class sdfsdf
    }
}
