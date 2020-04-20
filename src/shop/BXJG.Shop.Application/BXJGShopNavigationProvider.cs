using Abp.Application.Navigation;
using Abp.Authorization;
using BXJG.Shop.Authorization;
using BXJG.Shop.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public static class BXJGShopNavigationProvider
    {
        public static MenuDefinition Init(MenuDefinition menu)
        {
            var jczl = new MenuItemDefinition(BXJGShopPermissions.BXJGShop,
                                     BXJGShopPermissions.BXJGShop.BXJGShopL(),
                                     icon: BXJGShopPermissions.BXJGShop,
                                     permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShop))
                .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopDictionary,
                                                BXJGShopPermissions.BXJGShopDictionary.BXJGShopL(),
                                                icon: BXJGShopPermissions.BXJGShopDictionary,
                                                url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopDictionary}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopDictionary)))
                .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopItem,
                                                BXJGShopPermissions.BXJGShopItem.BXJGShopL(),
                                                icon: BXJGShopPermissions.BXJGShopItem,
                                                url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopItem}/index.html",
                                                permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopItem)));

            menu.AddItem(jczl);
            return menu;
        }
    }
}
