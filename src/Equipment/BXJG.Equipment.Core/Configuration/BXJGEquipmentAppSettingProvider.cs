using Abp.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.Configuration
{
    public class BXJGEquipmentAppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return null;
            //var sys = new SettingDefinitionGroup(BXJGEquipmentConst.DataDictionayMigrationValueSettingGroupKey, "数据字典下来值".BXJGEquipmentL());

            //var orderGroup = new SettingDefinitionGroup(BXJGEquipmentConst.OrderSettingGroupKey, "订单设置".BXJGEquipmentL());
            //return new[]
            //{
            //    new SettingDefinition(
            //        BXJGEquipmentConst.OrderTaxRateSettingKey,
            //        BXJGEquipmentConst.OrderTaxRateSettingDefaultValue.ToString("0.00"),
            //        "订单税率".BXJGEquipmentL(),
            //        orderGroup,
            //        scopes: SettingScopes.Application | SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGEquipmentConst.DataDictionayMigrationValuepinpai,
            //        "0",
            //        "品牌数据字典Id".BXJGEquipmentL(),
            //        sys,
            //        scopes:  SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGEquipmentConst.DataDictionayMigrationValuedanwei,
            //        "0",
            //        "单位数据字典Id".BXJGEquipmentL(),
            //        sys,
            //        scopes:  SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGEquipmentConst.DataDictionayMigrationValuezhifufangshi,
            //        "0",
            //        "支付方式数据字典Id".BXJGEquipmentL(),
            //        sys,
            //        scopes: SettingScopes.Tenant,
            //        isVisibleToClients:true),

            //     new SettingDefinition(
            //        BXJGEquipmentConst.DataDictionayMigrationValuepeisongfangshi,
            //        "0",
            //        "配送方式数据字典Id".BXJGEquipmentL(),
            //        sys,
            //        scopes: SettingScopes.Application | SettingScopes.Tenant,
            //        isVisibleToClients:true),
            //    new SettingDefinition("bxjgtestshp", "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true)
            //};
        }
    }
}
