using Abp.Configuration.Startup;
using BXJG.PSI;
using BXJG.PSI.Module;
using BXJG.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Localization
{
    public static class BXJGPSIModulExt
    {
        public static BXJGPSIModuleConfig BXJGPSI(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<BXJGPSIModuleConfig>();
        }
    }
}
