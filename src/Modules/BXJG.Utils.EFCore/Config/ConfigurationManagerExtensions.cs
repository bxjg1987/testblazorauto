using BXJG.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationManagerExtensions
    {
        public static ConfigurationManager AddEntityConfiguration( this ConfigurationManager manager)
        {
            var connectionString = manager.GetConnectionString("WidgetConnectionString");

            IConfigurationBuilder configBuilder = manager;
            configBuilder.Add(new AbpSettingsConfigurationSource());

            return manager;
        }
    }
}
