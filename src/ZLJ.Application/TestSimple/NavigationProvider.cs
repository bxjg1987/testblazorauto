using ZLJ.Application.Share.TestSimple;
using Abp.Application.Navigation;

namespace ZLJ.Application
{
    public partial class AdminNavigationProvider
    {
        public void CodeGeneratorTestSimple(INavigationProviderContext context)
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

            var menuItem = new MenuItemDefinition(TestSimpleApplicationShareConsts.PermissionNameGet,
                                                  TestSimpleApplicationShareConsts.PermissionNameGet.GetAdminLocalizableString(),
                                                  icon: "test",
                                                  url: "/TestSimple",
                                                  permissionDependency: new SimplePermissionDependency(TestSimpleApplicationShareConsts.PermissionNameGet));

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