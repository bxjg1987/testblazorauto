using Abp.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.Common.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Settings
{
    //反正内部要使用ef，可以考虑使用ef的事件来通知。
    //经过测试，貌似它是单例的

    /// <summary>
    /// 通过ef查询abp的setting表中的应用程序配置作为.net配置系统的数据源
    /// </summary>
    public class AbpSettingsConfigurationProvider : ConfigurationProvider, IDisposable
    {
        IDisposable d;  //注册全局事件，ISettingManager会来调用这里
        Func<DbContext> dbContextFactory;
        YanchiChuli yanchiChuli;//延时覆盖执行器，因为批量修改多个设置项，短时间会触发多次事件
        ILogger logger = SimpleLogger.Instance;
        public AbpSettingsConfigurationProvider(Func<DbContext> dbContextFactory, ILoggerFactory? loggerFactory = default)
        {
            this.dbContextFactory = dbContextFactory;

            if (loggerFactory != default)
                logger = loggerFactory.CreateLogger<AbpSettingsConfigurationProvider>();

            yanchiChuli = new YanchiChuli(ct =>
            {
                Load();
                OnReload();//通知iconfiguration，当前配置提供器已刷新
                return Task.CompletedTask;
            },loggerFactory:loggerFactory);


            //用我们自己的事件总线更轻量
            //Abp.Events.Bus.EventBus.Default.Register()

            //由于仅关注全局的应用程序级别的配置，所以这里使用全局事件总线，跟ISettingManager保持一致即可
            d = Zhongjie.Instance.Zhuce(() =>
            {
                yanchiChuli.Request();
                return ValueTask.CompletedTask;
            }, Share.BXJGUtilsConsts.OnAbpApplicationSettingsChanged);
        }

        public void Dispose()
        {
            logger.LogInformation($"AbpSetting配置提供器{GetHashCode()}释放了");
            try
            {
                d?.Dispose();
            }
            catch { }
            try
            {
                yanchiChuli?.Dispose();
            }
            catch { }
        }

        public override void Load()
        {
            //logger.LogDebug($"AbpSetting配置提供器{GetHashCode()}开始加载abp settings 数据");
            using var db = dbContextFactory.Invoke();
            var list = db.Set<Abp.Configuration.Setting>().Where(x => !x.TenantId.HasValue && !x.UserId.HasValue).ToList();
            Data = list.ToDictionary(x => x.Name, x => x.Value, StringComparer.OrdinalIgnoreCase);
            //这个频率不会太高，记录日志是可以的
            logger.LogInformation($"AbpSetting配置提供器{GetHashCode()}加载载abp settings 数据：{JsonSerializer.Serialize(Data)}");
            //直接从数据库加载应用程序配置,
            //反正提供器好像不容易使用依赖注入，所以这里直接new dbcontext即可，记得释放
            //或者这里使用ef的事件实现
        }
    }
}