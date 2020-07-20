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

            return jczl;
        }

        public static MenuDefinition AddBXJGEquipmentNavigation(this MenuDefinition parent)
        {
            parent.AddItem(Create());
            return parent;
        }
        public static MenuItemDefinition AddBXJGEquipmentNavigation(this MenuItemDefinition parent)
        {
            parent.AddItem(Create());
            return parent;
            //jczl.AddItem(new MenuItemDefinition("OrganizationUnit",
            //                                    L("OrganizationUnit"),
            //                                    icon: "shebei",
            //                                    url: "/baseinfo/OrganizationUnit/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));
            //jczl.AddItem(new MenuItemDefinition("Job",
            //                                    L("Job"),
            //                                    icon: "zheng",
            //                                    url: "/baseinfo/job/index.html",
            //                                    permissionDependency: PermissionNames.AdministratorBaseInfoJob));
            //jczl.AddItem(new MenuItemDefinition("Employee",
            //                                    L("Employee"),
            //                                    icon: "user",
            //                                    url: "/baseinfo/Employee/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoEmployee)));
            //jczl.AddItem(new MenuItemDefinition("Administrative",
            //                                    L("Administrative"),
            //                                    icon: "qizi",
            //                                    url: "/baseinfo/Administrative/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoAdministrative)));
            //jczl.AddItem(new MenuItemDefinition("DataDictionary",
            //                                    L("DataDictionary"),
            //                                    icon: "shuju",
            //                                    url: "/baseinfo/DataDictionary/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoDataDictionary)));
        }
    }
}
