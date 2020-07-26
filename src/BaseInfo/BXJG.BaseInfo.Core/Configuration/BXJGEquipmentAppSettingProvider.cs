using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.BaseInfo.Configuration
{
    public class BXJGBaseInfoAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return null;
            //var sys = new SettingDefinitionGroup(BXJGBaseInfoConst.DataDictionayMigrationValueSettingGroupKey, "数据字典下来值".BXJGBaseInfoL());

            //var orderGroup = new SettingDefinitionGroup(BXJGBaseInfoConst.OrderSettingGroupKey, "订单设置".BXJGBaseInfoL());
            //return new[]
            //{
            //    new SettingDefinition(
            //        BXJGBaseInfoConst.OrderTaxRateSettingKey,
            //        BXJGBaseInfoConst.OrderTaxRateSettingDefaultValue.ToString("0.00"),
            //        "订单税率".BXJGBaseInfoL(),
            //        orderGroup,
            //        scopes: SettingScopes.Application | SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGBaseInfoConst.DataDictionayMigrationValuepinpai,
            //        "0",
            //        "品牌数据字典Id".BXJGBaseInfoL(),
            //        sys,
            //        scopes:  SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGBaseInfoConst.DataDictionayMigrationValuedanwei,
            //        "0",
            //        "单位数据字典Id".BXJGBaseInfoL(),
            //        sys,
            //        scopes:  SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGBaseInfoConst.DataDictionayMigrationValuezhifufangshi,
            //        "0",
            //        "支付方式数据字典Id".BXJGBaseInfoL(),
            //        sys,
            //        scopes: SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGBaseInfoConst.DataDictionayMigrationValuepeisongfangshi,
            //        "0",
            //        "配送方式数据字典Id".BXJGBaseInfoL(),
            //        sys,
            //        scopes: SettingScopes.Application | SettingScopes.Tenant,
            //        isVisibleToClients:true),
            //    new SettingDefinition("bxjgtestshp", "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            //};
        }
    }
}
