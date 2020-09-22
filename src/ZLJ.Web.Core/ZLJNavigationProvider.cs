using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using BXJG.CMS;
using BXJG.GeneralTree;
using BXJG.Shop;
using ZLJ.Authorization;
using BXJG.Equipment;
using BXJG.BaseInfo;
namespace ZLJ.Navigation
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ZLJNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            //{codegenerator}
        
            //注册设备管理模块的菜单
            context.Manager.MainMenu.AddBXJGEquipmentNavigation();

            context.Manager.MainMenu.AddBXJGShopNavigation();
            BXJGCMSNavigationProvider.Init(context.Manager.MainMenu);

            //注册基础信息模块的菜单
            context.Manager.MainMenu.AddBXJGBaseInfoNavigation();
           
            var xtsz = new MenuItemDefinition("System",
                                                L("System"),
                                                icon: "config",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystem));
            context.Manager.MainMenu.AddItem(xtsz);

            xtsz.AddItem(new MenuItemDefinition("AdminTenant",
                                                L("Tenant"),
                                                icon: "filter",
                                                url: "/system/tenant/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemTenant)));
            xtsz.AddItem(new MenuItemDefinition("AdminRole",
                                                L("Role"),
                                                icon: "groupbai",
                                                url: "/system/role/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemRole)));
            xtsz.AddItem(new MenuItemDefinition("AdminUser",
                                                L("User"),
                                                icon: "user",
                                                url: "/system/user/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemUser)));
            xtsz.AddItem(new MenuItemDefinition("SystemLog",
                                                L("Log"),
                                                icon: "lishi",
                                                url: "/system/Auditing.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemLog)));
            xtsz.AddItem(new MenuItemDefinition("SystemConfig",
                                                L("Settings"),
                                                icon: "config",
                                                url: "/system/settings.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorSystemConfig)));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}
