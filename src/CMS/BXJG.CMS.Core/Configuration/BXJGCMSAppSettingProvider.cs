using Abp.Configuration;
using BXJG.CMS.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Configuration
{
    public class BXJGCMSAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //var orderGroup = new SettingDefinitionGroup(BXJGCMSConsts.OrderSettingGroupKey, "订单设置".BXJGCMSL());
            //return new[]
            //{
            //    new SettingDefinition(
            //        BXJGCMSConsts.OrderTaxRateSettingKey,
            //        BXJGCMSConsts.OrderTaxRateSettingDefaultValue.ToString("0.00"),
            //        "订单税率".BXJGCMSL(),
            //        orderGroup,
            //        scopes: SettingScopes.Application | SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //    new SettingDefinition("bxjgtestshp", "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            //};
        }
    }
}
