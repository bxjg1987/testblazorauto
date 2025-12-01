using Abp.Configuration;
using Abp.Zero.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.PSI
{
    public class PSISettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return Enumerable.Empty<SettingDefinition>();
        }
    }
}