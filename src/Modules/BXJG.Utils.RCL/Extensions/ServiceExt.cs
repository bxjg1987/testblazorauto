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
