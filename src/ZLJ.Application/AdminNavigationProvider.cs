using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Core.Authorization;
using ZLJ.Core.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.Application.Authorization.Permissions;
using BXJG.Utils;
using BXJG.Utils.Localization;
using DocumentFormat.OpenXml.Drawing;
using ZLJ.Application.Share.Authorization.Permissions;
using BXJG.Utils.Helpers;

namespace ZLJ.Application
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public partial class AdminNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.MainMenu; //new MenuDefinition("adminBlazor", PermissionNames.Administrator.GetLocalizableString());
            //context.Manager.Menus.Add("adminBlazor", menu);

            //--codegenerator==

            menu.AddItem(new MenuItemDefinition("adminBlazor_home",
                                                "后台管理首页".GetAdminLocalizableString(),
                                                url: "/",
                                                icon: "dashboard",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Administrator)));

            #region 多租户
            var multiTenancy = new MenuItemDefinition(PermissionNames.AdminMultiTenancy,
                                                      PermissionNames.AdminMultiTenancy.GetAdminLocalizableString(),
                                                      icon: "appstore",
                                                      permissionDependency: new SimplePermissionDependency(PermissionNames.AdminMultiTenancy),order:1);
            menu.AddItem(multiTenancy);

            multiTenancy.AddItem(new MenuItemDefinition(PermissionNames.AdminTenant,
                                                        PermissionNames.AdminTenant.GetAdminLocalizableString(),
                                                        icon: "block",
                                                        url: "/tenant",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdminTenant)));
            //特征 版本...后续添加

            #endregion

            #region 基础资料
            //基础数据
            var menuBaseInfo = new MenuItemDefinition(PermissionNames.AdministratorBaseInfo,
                                                     "BaseInfo".UtilsLI(),
                                                      icon: "appstore",
                                                      permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo),order:int.MaxValue);
            menu.AddItem(menuBaseInfo);

            menuBaseInfo.AddItem(new MenuItemDefinition("通知中心",
                                                        "通知中心".GetAdminLocalizableString(),
                                                        icon: "notification",
                                                        url: "/notification"));
            //数据字典
            menuBaseInfo.AddItem(new MenuItemDefinition(BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeMenuName,
                                                        "数据字典".UtilsLI(),
                                                        icon: "table",
                                                        url: "/data-dictionary",
                                                        permissionDependency: new SimplePermissionDependency(BXJG.Utils.Application.Share.Auth.PermissionNames.GeneralTreeMenuName)));

            //组织机构
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.AdministratorBaseInfoOrganizationUnit,
              displayName: "OrganizationUnit".UtilsLI(),
              // @Icons.Material.Outlined.AccountTree
              icon: "compass",
              url: $"/organization-unit",
              requiresAuthentication: true,
              permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));

            //岗位
            menuBaseInfo.AddItem(new MenuItemDefinition(PermissionNames.AdministratorBaseInfoPost,
                                                        "Job".UtilsLI(),
                                                        icon: "team",
                                                        url: "/post",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoPost)));

            //员工档案
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoStaffInfo,
                                                        displayName: "Employee".UtilsLI(),
                                                        icon: "user",
                                                        url: $"/employee",
                                                        //requiresAuthentication: true,
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoStaffInfo)));

            //来往单位
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAssociatedCompany,
                displayName: PermissionNames.BXJGBaseInfoAssociatedCompany.GetAdminLocalizableString(),
                icon: "bank",
                url: $"/related-company",
                requiresAuthentication: true,
                permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGBaseInfoAssociatedCompany)));



            //行政区域
            menuBaseInfo.AddItem(new MenuItemDefinition(name: PermissionNames.BXJGBaseInfoAdministrative,
                displayName: "Administrative".UtilsLI(),
                icon: "flag",
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
                                                        "Log".UtilsLI(),
                                                        icon: "history",
                                                        url: "/auditing",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));

            menu.AddItem(new MenuItemDefinition("Job",
                                                        "作业".UtilsLI(),
                                                        icon: "field-time",
                                                        url: "/hangfire-page",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.HangFireDashboard)));

            menuBaseInfo.AddItem(new MenuItemDefinition("SystemConfig",
                                                        "Settings".UtilsLI(),
                                                        icon: "setting",
                                                        url: "/settings",
                                                        permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfigGet)));

            #endregion

            CodeGeneratorHelper.CodeGenerator(this, context);
        }
    }
}