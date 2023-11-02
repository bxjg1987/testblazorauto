using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Authorization;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.App.Admin.Authorization.Permissions;
using BXJG.Utils;
using BXJG.Utils.Localization;

namespace ZLJ.Web.Admin.Startup
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
                                                icon: "fas fa-solar-panel",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Administrator)));



            #region 基础资料
            //基础数据
            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                                                      PermissionNames.AdministratorBaseInfo.GetAdminLocalizableString(),
                                                      icon: "fas fa-table",
                                                      permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            menu.AddItem(menuBaseInfo);

            menuBaseInfo.AddItem(new MenuItemDefinition("通知中心",
                                                        "通知中心".GetAdminLocalizableString(),
                                                        icon: "fas fa-bullhorn",
                                                        url: "/admin/tongzhi"));
            //数据字典
            menuBaseInfo.AddItem(new MenuItemDefinition(BXJGUtilsConsts.GeneralTreeMenuName,
                                                        BXJGUtilsConsts.GeneralTreeMenuName.UtilsLI(),
                                                        icon: "fas fa-clipboard-list",
                                                        url: "/admin/data-dictionary",
                                                        permissionDependency: new SimplePermissionDependency(BXJGUtilsConsts.GeneralTreeMenuName)));

            //组织机构
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
              displayName: PermissionNames.AdministratorBaseInfoOrganizationUnit.GetAdminLocalizableString(),
              // @Icons.Material.Outlined.AccountTree
              icon: "fas fa-city",
              url: $"/admin/organizationUnit",
              requiresAuthentication: true,
              permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                                                        PermissionNames.AdministratorBaseInfoPost.GetAdminLocalizableString(),
                                                        icon: "fas fa-user-group",
                                                        url: "/admin/post",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                                                        displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetAdminLocalizableString(),
                                                        icon: "fas fa-user",
                                                        url: $"/admin/employee",
                                                        //requiresAuthentication: true,
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            //来往单位
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
                displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetAdminLocalizableString(),
                icon: "fas fa-handshake",
                url: $"/admin/related-company",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));



            //行政区域
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
                displayName: PermissionNames.BXJGBaseInfoAdministrative.GetAdminLocalizableString(),
                icon: "fas fa-location-dot",
                url: $"/admin/Administrative",
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
                                                        icon: "fas fa-clock-rotate-left",
                                                        url: "/admin/auditing",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                                                        "Settings".GetAdminLocalizableString(),
                                                        icon: "fas fa-gear",
                                                        url: "/admin/settings",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));

            #endregion
        }
    }
}