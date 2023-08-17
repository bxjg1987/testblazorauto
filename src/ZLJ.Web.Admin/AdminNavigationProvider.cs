using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Authorization;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using MudBlazor;
using ZLJ.App.Admin.Authorization.Permissions;
using BXJG.Utils;
using BXJG.Utils.Localization;

namespace ZLJ.Web.Admin
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class AdminNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = new MenuDefinition("adminBlazor", PermissionNames.Administrator.GetLocalizableString());
            context.Manager.Menus.Add("adminBlazor", menu);

            //{codegenerator}

            menu.AddItem(new MenuItemDefinition("adminBlazor_home",
                                                "后台管理首页".GetAdminLocalizableString(),
                                                url: "/admin",
                                                icon: Icons.Material.Outlined.Dashboard,
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Administrator)));


            #region 通知
            menu.AddItem(new MenuItemDefinition("通知中心",
                                                "通知中心".GetAdminLocalizableString(),
                                                icon: Icons.Material.Outlined.NotificationsNone,
                                                url: "/admin/tongzhi"));
            #endregion

            #region 基础资料
            //基础数据
            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                PermissionNames.AdministratorBaseInfo.GetAdminLocalizableString(),
                icon: Icons.Material.Outlined.AutoAwesome,
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            menu.AddItem(menuBaseInfo);

            ////组织机构
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
            //  displayName: PermissionNames.AdministratorBaseInfoOrganizationUnit.GetLocalizableString(),
            //  icon: "zuzhi",
            //  url: $"/bxjgbaseinfo/organizationUnit/index.html",
            //  requiresAuthentication: true,
            //  permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                PermissionNames.AdministratorBaseInfoPost.GetAdminLocalizableString(),
                icon: "groupbai",
                url: "/bxjgbaseinfo/post/index.html",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetAdminLocalizableString(),
                icon: Icons.Material.Outlined.Person,
                url: $"/admin/employee",
                //requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            ////来往单位
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
            //    displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetLocalizableString(),
            //    icon: "group",
            //    url: $"/bxjgbaseinfo/associatedCompany/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));



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
                                                BXJGUtilsConsts.GeneralTreeMenuName.GetAdminLocalizableString(),
                                                icon: Icons.Material.Outlined.Dataset,
                                                url: "/admin/data-dictionary",
                                                permissionDependency: new SimplePermissionDependency(BXJGUtilsConsts.GeneralTreeMenuName)));

            ////行政区域
            //menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
            //    displayName: PermissionNames.BXJGBaseInfoAdministrative.GetLocalizableString(),
            //    icon: "qizi",
            //    url: $"/bxjgbaseinfo/Administrative/index.html",
            //    requiresAuthentication: true,
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAdministrative)));

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

            //menuBaseInfo.AddItem(new MenuItemDefinition("AdminTenant",
            //    L("Tenant"),
            //    icon: "filter",
            //    url: "/system/tenant/index.html",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemTenant)));

            //menu.AddItem(new MenuItemDefinition("AdminRole",
            //   "Role".GetAdminLocalizableString(),
            //    icon: Icons.Material.Outlined.Group,
            //    url: "/admin/role",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemRole)));
            //menuBaseInfo.AddItem(new MenuItemDefinition("AdminUser",
            //    L("User"),
            //    icon: "user",
            //    url: "/system/user/index.html",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemUser)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemLog",
                "Log".GetAdminLocalizableString(),
                icon: Icons.Material.Outlined.History,
                url: "/admin/auditing",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));
            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                "Settings".GetAdminLocalizableString(),
               icon: Icons.Material.Outlined.Settings,
                url: "/admin/settings",
                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));

            #endregion
        }
    }
}