using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
//using BXJG.WorkOrder;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using BXJG.Utils;
using ZLJ.App.Admin.Authorization.Permissions;

namespace ZLJ.App.Admin
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public partial class ZLJNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            //{codegenerator}

            

            #region 基础资料

            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                PermissionNames.AdministratorBaseInfo.GetLocalizableString(),
                icon: "shuju",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            context.Manager.MainMenu.AddItem(menuBaseInfo);

            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
              displayName: PermissionNames.AdministratorBaseInfoOrganizationUnit.GetLocalizableString(),
              icon: "zuzhi",
              url: $"/bxjgbaseinfo/organizationUnit/index.html",
              requiresAuthentication: true,
              permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                PermissionNames.AdministratorBaseInfoPost.GetLocalizableString(),
                icon: "groupbai",
                url: "/bxjgbaseinfo/post/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetLocalizableString(),
                icon: "user",
                url: $"/bxjgbaseinfo/staffInfo/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            //来往单位
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
                displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetLocalizableString(),
                icon: "group",
                url: $"/bxjgbaseinfo/associatedCompany/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));



            //设备故障
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoEquipmentFaultDefinition,
            //    displayName: PermissionNames.BXJGBaseInfoEquipmentFaultDefinition.GetLocalizableString(),
            //    icon: "zuzhi",
            //    url: $"/bxjgbaseinfo/equipmentFaultDefinition/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames
            //        .BXJGBaseInfoEquipmentFaultDefinition)));

            //var sjzd = menuBaseInfo.AddGeneralTreeNavigation();
            //sjzd.Icon = "shuju";
            //sjzd.Url = "/bxjgbaseinfo/generalTree/index.html";

            menuBaseInfo.AddItem(new MenuItemDefinition(BXJGUtilsConsts.GeneralTreeMenuName,
                                                        BXJGUtilsConsts.GeneralTreeMenuName.UtilsLI(),
                                                        icon: "generalTree",
                                                        permissionDependency: new SimplePermissionDependency(BXJGUtilsConsts.GeneralTreeMenuName)));



            //行政区域
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
                displayName: PermissionNames.BXJGBaseInfoAdministrative.GetLocalizableString(),
                icon: "qizi",
                url: $"/bxjgbaseinfo/Administrative/index.html",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAdministrative)));

            //直接使用用户
            ////员工档案
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
            //    displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetLocalizableString(),
            //    icon: "user",
            //    url: $"/bxjgbaseinfo/staffInfo/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));


            //--codegenerator.BaseInfo==


            //var xtsz = new MenuItemDefinition("System",
            //    L("System"),
            //    icon: "config",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystem));
            //context.Manager.MainMenu.AddItem(xtsz);

            menuBaseInfo.AddItem(new MenuItemDefinition("AdminTenant",
                L("Tenant"),
                icon: "filter",
                url: "/system/tenant/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemTenant)));

            menuBaseInfo.AddItem(new MenuItemDefinition("AdminRole",
                L("Role"),
                icon: "groupbai",
                url: "/system/role/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemRole)));
            menuBaseInfo.AddItem(new MenuItemDefinition("AdminUser",
                L("User"),
                icon: "user",
                url: "/system/user/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemUser)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemLog",
                L("Log"),
                icon: "lishi",
                url: "/system/Auditing.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));
            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                L("Settings"),
                icon: "config",
                url: "/system/settings.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));
            #endregion
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}