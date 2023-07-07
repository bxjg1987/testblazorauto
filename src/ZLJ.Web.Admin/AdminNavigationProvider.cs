using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Authorization;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using MudBlazor;

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

          
           
            
            //@Icons.Material.Filled.NotificationsNone
            menu.AddItem(new MenuItemDefinition("通知中心",
                                               "通知中心".LICommon(),
                                               url: "/admin/tongzhi",
                                               icon: Icons.Material.Outlined.Notifications));



            //menu.AddItem(new MenuItemDefinition("yuangong",
            //                                    "Employee".GetLocalizableString(),
            //                                    url: "/cust/emp",
            //                                    icon: Icons.Outlined.EmojiPeople,
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));
            //menu.AddItem(new MenuItemDefinition("bumen",
            //                                    "Department".GetLocalizableString(),
            //                                    url: "/cust/dept",
            //                                    icon: Icons.Outlined.Group,
            //                                    permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));

        }
    }
}