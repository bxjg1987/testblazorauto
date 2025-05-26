using Abp.Configuration;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
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

    /// <summary>
    /// 直接获取abp settings 应用级别的配置
    /// </summary>
    public class AbpSettingsConfigurationProvider : IConfigurationProvider//, IDisposable
    {
        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        //Func<DbContext> dbContextFactory;
        //ILoggerFactory sdf;
        ILogger logger;
        public AbpSettingsConfigurationProvider(Func<DbContext> dbContextFactory, ILoggerFactory sdf=default)
        {

            this.logger = sdf?.CreateLogger(GetType())?? SimpleLogger.Instance;

            using var db = dbContextFactory.Invoke();
            var list = db.Set<Abp.Configuration.Setting>().Where(x => !x.TenantId.HasValue && !x.UserId.HasValue).ToList();
            ls = list.ToDictionary(x => x.Name, x => x.Value, StringComparer.OrdinalIgnoreCase);
            logger.LogDebug($"AbpSettingsConfigurationProvider构造函数执行了");
           // this.sdf = sdf;
            //Console.WriteLine($"AbpSettingsConfigurationProvider构造函数执行了");
        }

        IDictionary<string, string?> ls;//= new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        DateTime lastT = DateTime.Now;
        /// <summary>
        /// Gets or sets the configuration key-value pairs for this provider.
        /// </summary>
        protected IDictionary<string, string?> Data
        {
            get
            {
                if ((DateTime.Now - lastT).TotalSeconds < 10)
                    return ls;

                if (IocManager.Instance == default)
                    return ls;

                ls.Clear();
                IocManager.Instance.UsingScope(scope =>
                {
                    //efcorerep
                    var sm = scope.Resolve<ISettingManager>();
                    // sm.getall
                    var ls1 = sm.GetAllSettingValues();//.ToDictionary(x => x.Name, x => x.Value,);

                    foreach (var s in ls1)
                    {
                        ls.Add(s.Name, s.Value);
                    }
                });
                logger.LogDebug($"AbpSettingsConfigurationProvider.Data被访问，数据量：{ls.Count}");
                return ls;
            }
        }

        /// <summary>
        /// Attempts to find a value with the given key.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">When this method returns, contains the value if one is found.</param>
        /// <returns><see langword="true" /> if <paramref name="key" /> has a value; otherwise <see langword="false" />.</returns>
        public virtual bool TryGet(string key, out string? value)
        {
            var lsz = string.Empty;
            var cg = false;
            try
            {
                IocManager.Instance.UsingScope(scope =>
                {
                    var sm = scope.Resolve<ISettingManager>();

                    lsz = sm.GetSettingValue(key);
                    cg = true;


                });
            }
            catch
            {
                //if(data)
                cg = Data.TryGetValue(key, out lsz);
            }
            value = lsz;
            logger.LogDebug($"AbpSettingsConfigurationProvider.TryGet，key：{key} value：{lsz}");
            return cg;
        }

        /// <summary>
        /// Sets a value for a given key.
        /// </summary>
        /// <param name="key">The configuration key to set.</param>
        /// <param name="value">The value to set.</param>
        public virtual void Set(string key, string? value)
            => ls[key] = value;



        /// <summary>
        /// Returns the list of keys that this provider has.
        /// </summary>
        /// <param name="earlierKeys">The earlier keys that other providers contain.</param>
        /// <param name="parentPath">The path for the parent IConfiguration.</param>
        /// <returns>The list of keys for this provider.</returns>
        public virtual IEnumerable<string> GetChildKeys(
            IEnumerable<string> earlierKeys,
            string? parentPath)
        {
            var results = new List<string>();

            if (parentPath is null)
            {
                foreach (KeyValuePair<string, string?> kv in Data)
                {
                    results.Add(Segment(kv.Key, 0));
                }
            }
            else
            {
                Debug.Assert(ConfigurationPath.KeyDelimiter == ":");

                foreach (KeyValuePair<string, string?> kv in Data)
                {
                    if (kv.Key.Length > parentPath.Length &&
                        kv.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) &&
                        kv.Key[parentPath.Length] == ':')
                    {
                        results.Add(Segment(kv.Key, parentPath.Length + 1));
                    }
                }
            }

            results.AddRange(earlierKeys);

            results.Sort(ConfigurationKeyComparer.Instance.Compare);

            return results;
        }

        private static string Segment(string key, int prefixLength)
        {
            Debug.Assert(ConfigurationPath.KeyDelimiter == ":");
            int indexOf = key.IndexOf(':', prefixLength);
            return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        }

        /// <summary>
        /// Returns a <see cref="IChangeToken"/> that can be used to listen when this provider is reloaded.
        /// </summary>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        /// <summary>
        /// Triggers the reload change token and creates a new one.
        /// </summary>
        protected void OnReload()
        {
            ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        /// <summary>
        /// Generates a string representing this provider name and relevant details.
        /// </summary>
        /// <returns>The configuration name.</returns>
        public override string ToString() => GetType().Name;

        public void Load()
        {
            logger.LogDebug($"AbpSettingsConfigurationProvider.Load被执行");
            return;
            ////GetReloadToken().
            ////base.OnReload
            //IocManager.Instance.UsingScope(scope =>
            //{
            //    var sm = scope.Resolve<ISettingManager>();
            //    Data = sm.GetAllSettingValuesForApplication().ToDictionary(x => x.Name, x => x.Value);
            //});
        }

    }
}