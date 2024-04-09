using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Abp.Extensions;
using Abp.Reflection.Extensions;

namespace ZLJ.Core.Configuration
{
    public static class AppConfigurations
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache;

        static AppConfigurations()
        {
            _configurationCache = new ConcurrentDictionary<string, IConfigurationRoot>();
        }

        public static IConfigurationRoot Get(string path, string environmentName = null, bool addUserSecrets = false)
        {
            var cacheKey = path + "#" + environmentName + "#" + addUserSecrets;
            return _configurationCache.GetOrAdd(
                cacheKey,
                _ => BuildConfiguration(path, environmentName, addUserSecrets)
            );
        }

        private static IConfigurationRoot BuildConfiguration(string path, string environmentName = null, bool addUserSecrets = false)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            if (!environmentName.IsNullOrWhiteSpace())
            {
                builder = builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            if (addUserSecrets)
            {
                builder.AddUserSecrets(typeof(AppConfigurations).GetAssembly());
            }

            //builder.AddJsonFile(builder.Build()[]);


            return builder.Build();
        }

        ///// <summary>
        ///// 将配置拆分到多个配置文件中，以便共享配置，此方法注册这些共享配置
        ///// </summary>
        ///// <param name="configurationBuilder"></param>
        ///// <returns></returns>
        //public static IConfigurationBuilder AddCommon(this IConfigurationBuilder configurationBuilder)
        //{
        //    var cm = new ConfigurationManager();
        //    cm.AddJsonFile(System.IO.Path.Combine(AppContext.BaseDirectory, "webcore.json"));
        //    //cm.AddJsonFile(cm["IotXuanxiangLujing"]);
        //    configurationBuilder.AddConfiguration(cm);
        //    return configurationBuilder;
        //}
    }
}
