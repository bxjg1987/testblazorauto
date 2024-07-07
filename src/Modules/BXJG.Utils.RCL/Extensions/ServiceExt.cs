using Abp.Application.Features;
using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.ObjectMapping;
using Abp.Runtime.Session;
using Abp.Web.Models.AbpUserConfiguration;
using BXJG.Utils.Application.Share;
using BXJG.Utils.Application.Share.Session;
using BXJG.Utils.RCL;
using BXJG.Utils.RCL.Helpers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NUglify.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using BXJG.Common.Http;
using Abp.Notifications;
using BXJG.Common.Events;
using Microsoft.AspNetCore.Http.Connections.Client;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExt
    {
        //public static Microsoft.AspNetCore.SignalR.Client.IHubConnectionBuilder WithUrl1(this Microsoft.AspNetCore.SignalR.Client.IHubConnectionBuilder hcb,string url,IAccessTokenProvider accessTokenProvider)
        //{
        //    return hcb.WithUrl(url, x =>
        //      {
        //          x.AccessTokenProvider = () => Task.FromResult(accessTokenProvider.GetEncryptedAccessToken());
        //      });
        //}

        /// <summary>
        /// blazor server和wasm都要注册的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cfg">通用signalR配置，通常只需要withurl配置地址即可</param>
        /// <returns></returns>
        public static IServiceCollection UseBXJGUtilsRCL(this IServiceCollection services, Action<IServiceProvider,HubConnectionBuilder> cfg = default)
        {
            //   BXJGUtilsRCLConfiguration peizhi = new BXJGUtilsRCLConfiguration(services);
            // peizhi.HubConnectionBuilder.WithAutomaticReconnect();
            //   Action<HttpConnectionOptions> act = x => x.AccessTokenProvider;
            //  peizhi.Services.Configure()

            //if (cfg != default)
            //    cfg.Invoke(peizhi);

            #region 通用signalR连接配置
            if (cfg != default)
            {
                services.TryAddKeyedScoped(Consts.TongyongLianjie, (s, o) =>
                {
                    // peizhi.HubConnectionBuilder.WithUrl("")

                    var hcb = new HubConnectionBuilder();
                    //观察withurl源码中发现token配置是通过选项来的， 或者这里用postconfiguration
                    hcb.Services.Configure<HttpConnectionOptions>(x=>x.AccessTokenProvider=()=> Task.FromResult( s.GetRequiredService<IAccessTokenProvider>().GetEncryptedAccessToken()) );
                    hcb.WithAutomaticReconnect();
                    cfg.Invoke(s,hcb);//后调用，便于外部覆盖

                    var conn = hcb.Build();
                    var logger = s.GetRequiredService<ILoggerFactory>().CreateLogger(Consts.TongyongLianjie);
                    var zhongjie = s.GetRequiredService<Zhongjie>();
                    conn.On<Abp.Notifications.UserNotification>("getNotification", async msg =>
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

                    //某些推送仅仅是向前端发个信号，前端收到信号后，主动去找服务器做些事
                    //比如：全局状态，收到信号后，集合 延迟覆盖 重新去服务端拿数据后，更新本地缓存的状态
                    //当然这种推送不仅仅是应用在这个场景，这里定义一个通用的方式，及仅仅是获取信号，没有参数，内部直接触发同名事件

                    //每个事件的处理都不一样，所以这东西是死的，不能是动态的，

                    //这感觉是打通了后端api中abp事件 与前端事件。
                    //比如 后端 实体变更事件，可以对应前端 一个实体状态变更事件。太复杂了，还是特事特办

                    //状态变更事件是应用程序级别的，不需要用户订阅
                    //通知会持久化，且用户少是直接推送，多了是任务推送
                    //全局状态变更是不需要持久化，仅推送给所有在线用户

                    //后端推送应该用延迟覆盖，因为短时间内多个线程，可能同时在改变多个状态，都做推送太浪费。
                    //conn.On("", async () =>
                    //{

                    //});

                    return conn;
                });
            }
            
            #endregion

            services.AddCommonRCL(async s =>
            {
                var fw = s.GetRequiredService<Task<AbpUserConfigurationDto>>();

                var r = (await fw)?.Auth?.GrantedPermissions?.Keys;
                if (r == null)
                    return [];

                return r;

            });
            //.AddTransient<FileHelper>()
            //.AddZLJBlazorClient()
            //.AddScoped(AppContainer.App);
            //services.AddScoped<AppContainer>();

            // services.TryAddKeyedScoped<Task<GetCurrentLoginInformationsOutput>>(Consts.GetCurrentLoginInformationsOutput);




            //services.TryAddSingleton<IObjectMapper, AutoMapperObjectMapper>();
            //services.TryAddScoped<CommonConnection>();
            services.AddAutoMapper(typeof(BXJG.Utils.RCL._Imports));

            services.TryAddTransient<FileHelper>();




            //不好实现，所以不要使用多语言
            //services.TryAddSingleton<ILocalizationManager, NullLocalizationManager>();
            return services;
        }
    }

    //木有必要，注册服务时，后续参数都是可选的，即使改服务注册的方法参数也无所谓，没必要定义这么多层对象
    ///// <summary>
    ///// 方便对当前库进行服务注册时的配置
    ///// </summary>
    //public class BXJGUtilsRCLConfiguration
    //{
    //    public IServiceCollection Services { get; private set; }

    //    public BXJGUtilsRCLConfiguration(IServiceCollection services)
    //    {
    //        Services = services;
    //    }

    //    public Action<HubConnectionBuilder> HubConnectionBuilder { get; set; } = x => { };

    //    //可以进一步添加对通知配置
    //}
}
