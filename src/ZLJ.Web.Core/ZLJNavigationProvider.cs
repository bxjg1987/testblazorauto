using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using BXJG.CMS;
using BXJG.GeneralTree;
using BXJG.Shop;
using ZLJ.Authorization;

namespace ZLJ.Navigation
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class ZLJNavigationProvider : NavigationProvider
    {
        GeneralTreeModuleConfig cfg;
        public ZLJNavigationProvider(GeneralTreeModuleConfig cfg)
        {
            this.cfg = cfg;
        }
        public override void SetNavigation(INavigationProviderContext context)
        {

            //{codegenerator}
            //var zcgl = new MenuItemDefinition("Asset",
            //                                     L("Asset management"),
            //                                     icon: "zican",
            //                                     permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorAsset));
            //context.Manager.MainMenu.AddItem(zcgl);

            //zcgl.AddItem(new MenuItemDefinition("EquipmentInfo",
            //                                    L("Equipment info"),
            //                                    icon: "shebei",
            //                                    url: "/asset/equipmentInfo/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorAssetEquipmentInfo)));
            BXJGShopNavigationProvider.Init(context.Manager.MainMenu);
            BXJGCMSNavigationProvider.Init(context.Manager.MainMenu);
            var jczl = new MenuItemDefinition("BaseInfo",
                                                L("BaseInfo"),
                                                icon: "dangan",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfo));
            context.Manager.MainMenu.AddItem(jczl);
            jczl.AddItem(new MenuItemDefinition("OrganizationUnit",
                                                L("OrganizationUnit"),
                                                icon: "zuzhi",
                                                url: "/baseinfo/OrganizationUnit/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoOrganizationUnit)));
            //jczl.AddItem(new MenuItemDefinition("Job",
            //                                    L("Job"),
            //                                    icon: "zheng",
            //                                    url: "/baseinfo/job/index.html",
            //                                    permissionDependency: PermissionNames.AdministratorBaseInfoJob));
            jczl.AddItem(new MenuItemDefinition("Employee",
                                                L("Employee"),
                                                icon: "user",
                                                url: "/baseinfo/Employee/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoEmployee)));
            jczl.AddItem(new MenuItemDefinition("Administrative",
                                                L("Administrative"),
                                                icon: "qizi",
                                                url: "/baseinfo/Administrative/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoAdministrative)));
            //jczl.AddItem(new MenuItemDefinition("DataDictionary",
            //                                    L("DataDictionary"),
            //                                    icon: "shuju",
            //                                    url: "/baseinfo/DataDictionary/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoDataDictionary)));
            var sjzd = cfg.InitNav(jczl);
            sjzd.Icon = "shuju";
            sjzd.Url = "/baseinfo/generalTree/index.html";


            jczl.AddItem(new MenuItemDefinition("Btype",
                                                L("Btype"),
                                                url: "/views/baseinfo/btype/index.html",
                                                icon: "groupbai",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorBaseInfoBtype)));

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
            //var demo = new MenuItemDefinition("Demo",
            //                                    L("Demo"),
            //                                    icon: "caidanbai",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorDemo));
            //context.Manager.MainMenu.AddItem(demo);
            //demo.AddItem(new MenuItemDefinition("DemoUpload",
            //                                    L("Upload"),
            //                                    icon: "wenjian",
            //                                    url: "/demo/upload.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorDemoUpload)));
            //var weChat = new MenuItemDefinition("WeChat",
            //                                    L("WeChat"),
            //                                    icon: "caidanbai",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorWeChat));
            //context.Manager.MainMenu.AddItem(weChat);
            //weChat.AddItem(new MenuItemDefinition("WeChatIndex",
            //                                    L("WeChatIndex"),
            //                                    icon: "wenjian",
            //                                    url: "/wechat/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.AdministratorWeChatIndex)));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ZLJConsts.LocalizationSourceName);
        }
    }
}
