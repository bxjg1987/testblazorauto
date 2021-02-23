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
            #region 加盟商
            //var Franchisee = new MenuItemDefinition("Franchisee",
            //                                   L("Franchisee"),
            //                                   icon: "groupbai",
            //                                   permissionDependency: new SimplePermissionDependency(PermissionNames.Franchisee));
            //context.Manager.MainMenu.AddItem(Franchisee);
            //Franchisee.AddItem(new MenuItemDefinition("FranchiseeInfo",
            //                                    L("FranchiseeInfo"),
            //                                    icon: "wenjian",
            //                                    url: "/Franchisee/FranchiseeInfo/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeInfo)));
            //Franchisee.AddItem(new MenuItemDefinition("FranchiseeEquipment",
            //                                   L("FranchiseeEquipment"),
            //                                   icon: "wenjian",
            //                                   url: "/Franchisee/FranchiseeEquipment/index.html",
            //                                   permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeEquipment)));



            ////var FranchiseeBack = new MenuItemDefinition("FranchiseeBack",
            ////                                   L("FranchiseeBack"),
            ////                                   icon: "FranchiseeBack",
            ////                                   permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBack));
            ////context.Manager.MainMenu.AddItem(FranchiseeBack);

            //var m1 = new MenuItemDefinition("FranchiseeBackStatistical",
            //                                   L("FranchiseeBackStatistical"),
            //                                   icon: "wenjian",
            //                                   permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackStatistical));

            //context.Manager.MainMenu.AddItem(m1);
            //m1.AddItem(new MenuItemDefinition("FranchiseeBackStatisticalIncome",
            //                                      L("FranchiseeBackStatisticalIncome"),
            //                                      icon: "wenjian",
            //                                      url: "/Franchisee/FranchiseeBackStatisticalIncome/index.html",
            //                                      permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackStatisticalIncome)));


            //m1.AddItem(new MenuItemDefinition("FranchiseeBackStatisticalUser",
            //                                                L("FranchiseeBackStatisticalUser"),
            //                                                icon: "wenjian",
            //                                                url: "/Franchisee/FranchiseeBackStatisticalUser/index.html",
            //                                                permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackStatisticalUser)));
            //m1.AddItem(new MenuItemDefinition("FranchiseeBackStatisticalOrder",
            //                                      L("FranchiseeBackStatisticalOrder"),
            //                                      icon: "wenjian",
            //                                      url: "/Franchisee/FranchiseeBackStatisticalOrder/index.html",
            //                                      permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackStatisticalOrder)));
            //m1.AddItem(new MenuItemDefinition("FranchiseeBackStatisticalSale",
            //                                      L("FranchiseeBackStatisticalSale"),
            //                                      icon: "wenjian",
            //                                      url: "/Franchisee/FranchiseeBackStatisticalSale/index.html",
            //                                      permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackStatisticalSale)));


            //var m2 = new MenuItemDefinition("FranchiseeBackEquipment",
            //                                  L("FranchiseeBackEquipment"),
            //                                  icon: "wenjian",
            //                                  permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackEquipment));

            //context.Manager.MainMenu.AddItem(m2);
            //m2.AddItem(new MenuItemDefinition("FranchiseeBackEquipmentOrderStatus",
            //                                      L("FranchiseeBackEquipmentOrderStatus"),
            //                                      icon: "wenjian",
            //                                      url: "/Franchisee/FranchiseeBackEquipmentOrderStatus/index.html",
            //                                      permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackEquipmentOrderStatus)));
            //m2.AddItem(new MenuItemDefinition("FranchiseeBackEquipmentStatus",
            //                          L("FranchiseeBackEquipmentStatus"),
            //                          icon: "wenjian",
            //                          url: "/Franchisee/FranchiseeBackEquipmentStatus/index.html",
            //                          permissionDependency: new SimplePermissionDependency(PermissionNames.FranchiseeBackEquipmentStatus)));

            #endregion
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
