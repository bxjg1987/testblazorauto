using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using ZLJ.Authorization;
using ZLJ.Localization;
using BXJG.Utils.GeneralTree;
using ZLJ.App.Customer;
using MudBlazor;

namespace ZLJ.Web.Customer
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class CustNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = new MenuDefinition(PermissionNames.Customer, PermissionNames.Customer.GetCustLocalizableString());
            context.Manager.Menus.Add(PermissionNames.Customer, menu);

            //{codegenerator}

            menu.AddItem(new MenuItemDefinition("tongji",
                                                "Report".GetLocalizableString(),
                                                url: "/cust",
                                                icon: Icons.Outlined.BarChart,
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));
            menu.AddItem(new MenuItemDefinition("yuangong",
                                                "Employee".GetLocalizableString(),
                                                url: "/cust/emp",
                                                icon: Icons.Outlined.EmojiPeople,
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));
            menu.AddItem(new MenuItemDefinition("bumen",
                                                "Department".GetLocalizableString(),
                                                url: "/cust/dept",
                                                icon: Icons.Outlined.Group,
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));

        }
    }
}