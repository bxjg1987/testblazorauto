using Abp.Configuration;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Zero.EntityFrameworkCore;
using BXJG.Common;
using BXJG.Common.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BXJG.Utils.EFCore.Settings
{
    /*
     * 考虑到分布式场景，结合abp源码实现，改造下，直接从缓存获取即可
     */

    ///// <summary>
    ///// 直接获取abp settings 应用级别的配置
    ///// </summary>
    //public class AbpSettingsConfigurationProvider : IConfigurationProvider//, IDisposable
    //{
    //    private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

    //    //Func<DbContext> dbContextFactory;
    //    //ILoggerFactory sdf;
    //    ILogger logger;
    //    public AbpSettingsConfigurationProvider(Func<DbContext> dbContextFactory, ILoggerFactory sdf = default)
    //    {

    //        this.logger = sdf?.CreateLogger(GetType()) ?? SimpleLogger.Instance;

    //        using var db = dbContextFactory.Invoke();
    //        var list = db.Set<Abp.Configuration.Setting>().Where(x => !x.TenantId.HasValue && !x.UserId.HasValue).ToList();
    //        ls = list.ToDictionary(x => x.Name, x => string.Empty, StringComparer.OrdinalIgnoreCase);
    //        logger.LogDebug($"AbpSettingsConfigurationProvider构造函数执行了");
    //        // this.sdf = sdf;
    //        //Console.WriteLine($"AbpSettingsConfigurationProvider构造函数执行了");
    //    }

    //    IDictionary<string, string?> ls;//= new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
    //    Lock nlock = new Lock();
    //    const int jgsj = 20;
    //    DateTime lastT = DateTime.Now;
    //    /// <summary>
    //    /// Gets or sets the configuration key-value pairs for this provider.
    //    /// </summary>
    //    protected IDictionary<string, string?> Data
    //    {
    //        get
    //        {

    //            if ((DateTime.Now - lastT).TotalSeconds < jgsj)
    //                return ls;

    //            if (IocManager.Instance == default)
    //                return ls;

    //            using (nlock.EnterScope())
    //            {
    //                if ((DateTime.Now - lastT).TotalSeconds < jgsj)
    //                    return ls;

    //                if (IocManager.Instance == default)
    //                    return ls;

    //                ls.Clear();
    //                IocManager.Instance.UsingScope(scope =>
    //                {
    //                    //efcorerep
    //                    var sm = scope.Resolve<ISettingManager>();
    //                    // sm.getall
    //                    var ls1 = sm.GetAllSettingValues();//.ToDictionary(x => x.Name, x => x.Value,);
    //                                                       //  SettingManager
    //                    foreach (var s in ls1)
    //                    {
    //                        _ = ls.TryAdd(s.Name, string.Empty);
    //                    }
    //                });
    //                lastT = DateTime.Now;
    //            }

    //            logger.LogDebug($"AbpSettingsConfigurationProvider.Data被访问，数据量：{ls.Count} 对象：{this.GetHashCode()}");
    //            return ls;
    //        }
    //    }

    //    /// <summary>
    //    /// Attempts to find a value with the given key.
    //    /// </summary>
    //    /// <param name="key">The key to lookup.</param>
    //    /// <param name="value">When this method returns, contains the value if one is found.</param>
    //    /// <returns><see langword="true" /> if <paramref name="key" /> has a value; otherwise <see langword="false" />.</returns>
    //    public virtual bool TryGet(string key, out string? value)
    //    {
    //        var lsz = string.Empty;
    //        var cg = false;
    //        try
    //        {

    //            IocManager.Instance.UsingScope(scope =>
    //            {
    //                var sm = scope.Resolve<ISettingManager>();

    //                lsz = sm.GetSettingValue(key);

    //                cg = true;


    //            });
    //            //logger.LogDebug($"AbpSettingsConfigurationProvider.TryGet成功，key：{key} value：{lsz} 对象：{this.GetHashCode()}");
    //        }
    //        catch
    //        {
    //            //logger.LogDebug($"AbpSettingsConfigurationProvider.TryGet失败，key：{key} value：{lsz} 对象：{this.GetHashCode()}");
    //            //if(data)
    //            //cg = Data.TryGetValue(key, out lsz);
    //        }
    //        value = lsz;

    //        return cg;
    //    }

    //    /// <summary>
    //    /// Sets a value for a given key.
    //    /// </summary>
    //    /// <param name="key">The configuration key to set.</param>
    //    /// <param name="value">The value to set.</param>
    //    public virtual void Set(string key, string? value)
    //        => ls[key] = value;



    //    /// <summary>
    //    /// Returns the list of keys that this provider has.
    //    /// </summary>
    //    /// <param name="earlierKeys">The earlier keys that other providers contain.</param>
    //    /// <param name="parentPath">The path for the parent IConfiguration.</param>
    //    /// <returns>The list of keys for this provider.</returns>
    //    public virtual IEnumerable<string> GetChildKeys(
    //        IEnumerable<string> earlierKeys,
    //        string? parentPath)
    //    {
    //        var results = new List<string>();

    //        if (parentPath is null)
    //        {
    //            foreach (KeyValuePair<string, string?> kv in Data)
    //            {
    //                results.Add(Segment(kv.Key, 0));
    //            }
    //        }
    //        else
    //        {
    //            Debug.Assert(ConfigurationPath.KeyDelimiter == ":");

    //            foreach (KeyValuePair<string, string?> kv in Data)
    //            {
    //                if (kv.Key.Length > parentPath.Length &&
    //                    kv.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) &&
    //                    kv.Key[parentPath.Length] == ':')
    //                {
    //                    results.Add(Segment(kv.Key, parentPath.Length + 1));
    //                }
    //            }
    //        }

    //        results.AddRange(earlierKeys);

    //        results.Sort(ConfigurationKeyComparer.Instance.Compare);
    //        logger.LogDebug($"AbpSettingsConfigurationProvider.GetChildKeys，earlierKeys：{earlierKeys.Count()} parentPath：{parentPath} 对象：{this.GetHashCode()}");
    //        return results;
    //    }

    //    private static string Segment(string key, int prefixLength)
    //    {
    //        Debug.Assert(ConfigurationPath.KeyDelimiter == ":");
    //        int indexOf = key.IndexOf(':', prefixLength);
    //        return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
    //    }

    //    /// <summary>
    //    /// Returns a <see cref="IChangeToken"/> that can be used to listen when this provider is reloaded.
    //    /// </summary>
    //    /// <returns>The <see cref="IChangeToken"/>.</returns>
    //    public IChangeToken GetReloadToken()
    //    {
    //        return _reloadToken;
    //    }

    //    /// <summary>
    //    /// Triggers the reload change token and creates a new one.
    //    /// </summary>
    //    protected void OnReload()
    //    {
    //        ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
    //        previousToken.OnReload();
    //    }

    //    /// <summary>
    //    /// Generates a string representing this provider name and relevant details.
    //    /// </summary>
    //    /// <returns>The configuration name.</returns>
    //    public override string ToString() => GetType().Name;

    //    public void Load()
    //    {
    //        logger.LogDebug($"AbpSettingsConfigurationProvider.Load被执行");
    //        return;
    //        ////GetReloadToken().
    //        ////base.OnReload
    //        //IocManager.Instance.UsingScope(scope =>
    //        //{
    //        //    var sm = scope.Resolve<ISettingManager>();
    //        //    Data = sm.GetAllSettingValuesForApplication().ToDictionary(x => x.Name, x => x.Value);
    //        //});
    //    }

    //}


    /// <summary>
    /// abp的setting是基于数据库和缓存的
    /// 它的配置提供了应用层程序级别、租户和用户级别
    /// .net core本身有配置系统
    /// 有时候开发的外部库（不仅给本项目用，所以不依赖abp），或引用的三方库，往往使用.net core的配置系统
    /// 此时配置就比较割裂。
    /// 
    /// 这里自定义.net的配置提供程序，从abp setting获取配置
    /// 仅关注abp应用程序级别的setting，租户、用户的配置太多，不好、也不合适、也不方便失效 作为.net的配置。
    /// 
    /// 可以考虑增加租户隔离，实现思路是在业务方法开始时设置AsyncLocal租户id，在AbpSettingsConfigurationProvider使用
    /// 
    /// 在分布式环境中，A服务器更新了缓存，B服务器无法知晓，所以这里粗暴地使用轮询方式
    /// </summary>
    public class AbpSettingsConfigurationProvider : ConfigurationProvider
    {
        ILogger logger;
        Func<DbContext> dbContextFactory;
        System.Threading.Timer reloadTimer;//
        int interval = 1000 * Random.Shared.Next(30, 120) * 5;//setting默认使用缓存，群集共享redis时，可以错开
        public AbpSettingsConfigurationProvider(Func<DbContext> dbContextFactory, ILoggerFactory sdf = default)
        {
            reloadTimer = new Timer(jc, null, interval, interval);
            this.logger = sdf?.CreateLogger(GetType()) ?? SimpleLogger.Instance;
            this.dbContextFactory = dbContextFactory;
        }
        private void jc(object? state)
        {
            //logger.LogDebug($"AbpSettingsConfigurationProvider监控任务开始执行，当前配置提供器：{GetHashCode()}");
            try
            {
                //没法提前检测，因为反正要全量比对，那还不如直接重新加载
                Load();
                OnReload();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "abp settings 配置监控异常");
                //throw;
            }
        }

        public override void Load()
        {
            // 开始的时候iocmanager还没初始化好，所以try下
            // ISettingManager拿setting有缓存，而直接查数据库木有
            try
            {
                IocManager.Instance.UsingScope(scope =>
                {
                    Data = scope.Resolve<ISettingManager>().GetAllSettingValuesForApplication().ToDictionary(x => x.Name, x => x.Value, StringComparer.OrdinalIgnoreCase);
                });
                logger.LogDebug($"AbpSettingsConfigurationProvider从ISettingManager获取配置，数据{string.Join(',', Data.Keys)}");
            }
            catch
            {
                List<Setting> settings;
                using (var db = dbContextFactory.Invoke())
                {
                    settings = db.Set<Abp.Configuration.Setting>().AsNoTracking().Where(x => !x.TenantId.HasValue && !x.UserId.HasValue).ToList();
                }
                Data = settings.ToDictionary(
                     x => x.Name,
                     x =>
                     {
                         // 尝试解密加密的设置值
                         try
                         {
                             return SimpleStringCipher.Instance.Decrypt(x.Value);
                         }
                         catch
                         {
                             // 如果解密失败，返回原始值（可能是未加密的值）
                             return x.Value;
                         }
                     },
                     StringComparer.OrdinalIgnoreCase
                );


                logger.LogDebug($"AbpSettingsConfigurationProvider从dbcontext获取配置，数量{Data.Count}");
            }
        }
    }
}