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
            var jczl = new MenuItemDefinition(BXJGEquipmentPermissionNames.BXJGEquipment,
                                              BXJGEquipmentPermissionNames.BXJGEquipment.BXJGEquipmentL(),
                                              icon: "shebei",
                                              permissionDependency: new SimplePermissionDependency(BXJGEquipmentPermissionNames.BXJGEquipment));

            //代码生成器的占位符，它将在这里插入更多菜单
            //{codegenerator}

            //设备档案
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
