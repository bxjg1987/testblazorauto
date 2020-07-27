using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using BXJG.Equipment.Authorization;
using BXJG.Equipment.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Equipment
{
    public static class BXJGEquipmentNavigationExt
    {
        static MenuItemDefinition Create()
        {
            var jczl = new MenuItemDefinition("BXJGEquipment",
                                              "BXJGEquipment".BXJGEquipmentL(),
                                               icon: "shebei",
                                               permissionDependency: new SimplePermissionDependency(BXJGEquipmentPermissionNames.BXJGEquipment));

            //初始化更多菜单或子菜单
            //{codegenerator}

            //添加设备信息的菜单定义
            jczl.AddItem(new MenuItemDefinition(name: BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfo,
                                                displayName: BXJGEquipmentPermissionNames.BXJGEquipmentEquipmentInfo.BXJGEquipmentL(),
                                                icon: "dangan",
                                                url: $"/{BXJGEquipmentPermissionNames.BXJGEquipment}/equipmentinfo/index.html",
                                                requiresAuthentication: true,
                                                permissionDependency: new SimplePermissionDependency()));
            return jczl;
        }

        public static MenuItemDefinition AddBXJGEquipmentNavigation(this MenuDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
        public static MenuItemDefinition AddBXJGEquipmentNavigation(this MenuItemDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
    }
}
