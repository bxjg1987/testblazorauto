using Abp.Configuration;
using BXJG.Shop.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Configuration
{
    public class BXJGShopAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var sys = new SettingDefinitionGroup(BXJGShopConsts.DataDictionayMigrationValueSettingGroupKey, "数据字典下来值".BXJGShopL());

            var orderGroup = new SettingDefinitionGroup(BXJGShopConsts.OrderSettingGroupKey, "订单设置".BXJGShopL());
            return new[]
            {
                new SettingDefinition(
                    BXJGShopConsts.OrderTaxRateSettingKey,
                    BXJGShopConsts.OrderTaxRateSettingDefaultValue.ToString("0.00"),
                    "订单税率".BXJGShopL(),
                    orderGroup,
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    BXJGShopConsts.DataDictionayMigrationValuepinpai,
                    "0",
                    "品牌数据字典Id".BXJGShopL(),
                    sys,
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    BXJGShopConsts.DataDictionayMigrationValuezhifufangshi,
                    "0",
                    "支付方式数据字典Id".BXJGShopL(),
                    sys,
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients:true),

                 new SettingDefinition(
                    BXJGShopConsts.DataDictionayMigrationValuepeisongfangshi,
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
