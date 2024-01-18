using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Core.Authorization;
using ZLJ.Core.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.Application.Admin.Authorization.Permissions;
using BXJG.Utils;
using BXJG.Utils.Localization;
using DocumentFormat.OpenXml.Drawing;
using ZLJ.Application.Share.Authorization.Permissions;

namespace ZLJ.Application.Admin
{
    /*
     * 这个需要在host和blazorhost间共享，属于ui部分的东东，放这里其实不太合适
     * 后期如果发现host和blazorhost之间需要共享更多东东时再单独建个类
     * 
     * 放blazor客户端更不合适，因为它一来依赖abp，况且客户端是调用webapi拿有权访问的菜单的
     */

    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class AdminNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.MainMenu; //new MenuDefinition("adminBlazor", PermissionNames.Administrator.GetLocalizableString());
            //context.Manager.Menus.Add("adminBlazor", menu);

            //{codegenerator}
            menu.AddItem(new MenuItemDefinition("adminBlazor_home",
                                                "后台管理首页".GetAdminLocalizableString(),
                                                url: "/",
                                                icon: "dashboard",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Administrator)));



            #region 基础资料
            //基础数据
            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                                                      PermissionNames.AdministratorBaseInfo.GetAdminLocalizableString(),
                                                      icon: ".Outline.Appstore",
                                                      permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            menu.AddItem(menuBaseInfo);

            menuBaseInfo.AddItem(new MenuItemDefinition("通知中心",
                                                        "通知中心".GetAdminLocalizableString(),
                                                        icon: ".Outline.Notification",
                                                        url: "/notification"));
            //数据字典
            menuBaseInfo.AddItem(new MenuItemDefinition(BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeMenuName,
                                                        BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeMenuName.UtilsLI(),
                                                        icon: ".Outline.Table",
                                                        url: "/data-dictionary",
                                                        permissionDependency: new SimplePermissionDependency(BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeMenuName)));

            //组织机构
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
              displayName: PermissionNames.AdministratorBaseInfoOrganizationUnit.GetAdminLocalizableString(),
              // @Icons.Material.Outlined.AccountTree
              icon: ".Outline.Compass",
              url: $"/organization-unit",
              requiresAuthentication: true,
              permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                                                        PermissionNames.AdministratorBaseInfoPost.GetAdminLocalizableString(),
                                                        icon: ".Outline.UsergroupAdd",
                                                        url: "/post",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                                                        displayName: PermissionNames.BXJGBaseInfoStaffInfo.GetAdminLocalizableString(),
                                                        icon: ".Outline.User",
                                                        url: $"/employee",
                                                        //requiresAuthentication: true,
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            //来往单位
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
                displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetAdminLocalizableString(),
                icon: ".Outline.CustomerService",
                url: $"/related-company",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));



            //行政区域
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
                displayName: PermissionNames.BXJGBaseInfoAdministrative.GetAdminLocalizableString(),
                icon: ".Outline.BoxPlot",
                url: $"/Administrative",
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
            //    url: "/role",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemRole)));
            //menuBaseInfo.AddItem(new MenuItemDefinition("AdminUser",
            //    L("User"),
            //    icon: "user",
            //    url: "/system/user/index.html",
            //    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemUser)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemLog",
                                                        "Log".GetAdminLocalizableString(),
                                                        icon: ".Outline.History",
                                                        url: "/auditing",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                                                        "Settings".GetAdminLocalizableString(),
                                                        icon: ".Outline.Setting",
                                                        url: "/settings",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));

            #endregion
        }
    }
}