using Abp.Configuration;
using BXJG.Shop.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var sys = new SettingDefinitionGroup(CoreConsts.DataDictionayMigrationValueSettingGroupKey, "数据字典下来值".BXJGShopL());

            var orderGroup = new SettingDefinitionGroup(CoreConsts.OrderSettingGroupKey, "订单设置".BXJGShopL());
            return new[]
            {
                new SettingDefinition(
                    CoreConsts.OrderTaxRateSettingKey,
                    CoreConsts.OrderTaxRateSettingDefaultValue.ToString("0.00"),
                    "订单税率".BXJGShopL(),
                    orderGroup,
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    CoreConsts.DataDictionayMigrationValuepinpai,
                    "0",
                    "品牌数据字典Id".BXJGShopL(),
                    sys,
                    scopes:  SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    CoreConsts.DataDictionayMigrationValuedanwei,
                    "0",
                    "单位数据字典Id".BXJGShopL(),
                    sys,
                    scopes:  SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    CoreConsts.DataDictionayMigrationValuezhifufangshi,
                    "0",
                    "支付方式数据字典Id".BXJGShopL(),
                    sys,
                    scopes: SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    CoreConsts.DataDictionayMigrationValuepeisongfangshi,
                    "0",
                    "配送方式数据字典Id".BXJGShopL(),
                    sys,
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients:true),
                new SettingDefinition("bxjgtestshp", "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            };
        }
    }
}
