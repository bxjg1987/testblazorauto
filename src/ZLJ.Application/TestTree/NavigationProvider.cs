using ZLJ.Application.Share.TestTree;
using Abp.Application.Navigation;

namespace ZLJ.Application
{
    public partial class AdminNavigationProvider
    {
        public void CodeGeneratorTestTree(INavigationProviderContext context)
        {
            object parentMenu = default;
            const string currMenuName = "";
            foreach (var item in context.Manager.Menus)
            {
                if (item.Key.Equals(currMenuName, StringComparison.OrdinalIgnoreCase))
                    parentMenu = item.Value;
                else
                    parentMenu = item.Value.RecursionFindDown(currMenuName);
            }
            if (parentMenu == default)
                parentMenu = context.Manager.MainMenu;

            var menuItem = new MenuItemDefinition(TestTreeApplicationShareConsts.PermissionNameGet,
                                                  TestTreeApplicationShareConsts.PermissionNameGet.GetAdminLocalizableString(),
                                                  icon: "testtree",
                                                  url: "/TestTree",
                                                  permissionDependency: new SimplePermissionDependency(TestTreeApplicationShareConsts.PermissionNameGet));

            if (parentMenu is MenuDefinition m0)
            {
                m0.AddItem(menuItem);
            }
            else
            {
                (parentMenu as MenuItemDefinition).AddItem(menuItem);
            }
        }
    }
}