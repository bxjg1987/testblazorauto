using BXJG.Utils.EFCore.Settings;
using BXJG.Utils.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationManagerExtensions
    {
        public static IConfigurationBuilder AddAbpSettingsConfiguration(this IConfigurationBuilder manager, Func<DbContext> dbContextFactory,ILoggerFactory logger =default)
        {
         return   manager.Add(new AbpSettingsConfigurationSource(dbContextFactory, logger));
        }
    }
}
