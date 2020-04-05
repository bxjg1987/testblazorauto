using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Configuration
{
    public class BXJGShopAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition("bxjgtestshp", "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
    }
}
