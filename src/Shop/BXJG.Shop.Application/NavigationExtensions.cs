using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Extensions;
using BXJG.Shop.Authorization;
using BXJG.Shop.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop
{
    public static class NavigationExtensions
    {
        //public static MenuDefinition Init(MenuDefinition menu)
        //{
        //    var jczl = new MenuItemDefinition(BXJGShopPermissions.BXJGShop,
        //                             BXJGShopPermissions.BXJGShop.BXJGShopL(),
        //                             icon: BXJGShopPermissions.BXJGShop,
        //                             permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShop))
        //        //商城不提供独立的字典，而时由模块调用方提供
        //        //.AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopDictionary,
        //        //                                BXJGShopPermissions.BXJGShopDictionary.BXJGShopL(),
        //        //                                icon: BXJGShopPermissions.BXJGShopDictionary,
        //        //                                url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopDictionary}/index.html",
        //        //                                permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopDictionary)))
        //        .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopItemCategory,
        //                                        BXJGShopPermissions.BXJGShopItemCategory.BXJGShopL(),
        //                                        icon: BXJGShopPermissions.BXJGShopItemCategory,
        //                                        url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopItemCategory}/index.html",
        //                                        permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopItemCategory)))
        //        .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopItem,
        //                                        BXJGShopPermissions.BXJGShopItem.BXJGShopL(),
        //                                        icon: BXJGShopPermissions.BXJGShopItem,
        //                                        url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopItem}/index.html",
        //                                        permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopItem)))
        //        .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopOrder,
        //                                        BXJGShopPermissions.BXJGShopOrder.BXJGShopL(),
        //                                        icon: BXJGShopPermissions.BXJGShopOrder,
        //                                        url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopOrder}/index.html",
        //                                        permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopOrder)))
        //        .AddItem(new MenuItemDefinition(BXJGShopPermissions.BXJGShopCustomer,
        //                                        BXJGShopPermissions.BXJGShopCustomer.BXJGShopL(),
        //                                        icon: BXJGShopPermissions.BXJGShopCustomer,
        //                                        url: $"/{BXJGShopPermissions.BXJGShop}/{BXJGShopPermissions.BXJGShopCustomer}/index.html",
        //                                        permissionDependency: new SimplePermissionDependency(BXJGShopPermissions.BXJGShopCustomer))); 

        //    menu.AddItem(jczl);
        //    return menu;
        //}

        static MenuItemDefinition Create()
        {
            var jczl = new MenuItemDefinition(PermissionNames.BXJGShop,
                                     PermissionNames.BXJGShop.BXJGShopL(),
                                     icon: PermissionNames.BXJGShop,
                                     permissionDependency: new SimplePermissionDependency(PermissionNames.BXJGShop));

            //代码生成器的占位符，它将在这里插入更多菜单
            //{codegenerator}
          
            jczl.AddItem(new MenuItemDefinition(PermissionNames.ProductCategory,
                                                PermissionNames.ProductCategory.BXJGShopL(),
                                                icon: PermissionNames.ProductCategory,
                                                url: $"/{PermissionNames.BXJGShop}/{PermissionNames.ProductCategory.RemovePreFix(PermissionNames.BXJGShop)}/index.html",
                                                permissionDependency: new SimplePermissionDependency(PermissionNames.ProductCategory)))
            .AddItem(new MenuItemDefinition(PermissionNames.Product,
                                            PermissionNames.Product.BXJGShopL(),
                                            icon: PermissionNames.Product,
                                            url: $"/{PermissionNames.BXJGShop}/{PermissionNames.Product.RemovePreFix(PermissionNames.BXJGShop)}/index.html",
                                            permissionDependency: new SimplePermissionDependency(PermissionNames.Product)))
            .AddItem(new MenuItemDefinition(PermissionNames.Order,
                                            PermissionNames.Order.BXJGShopL(),
                                            icon: PermissionNames.Order,
                                            url: $"/{PermissionNames.BXJGShop}/{PermissionNames.Order.RemovePreFix(PermissionNames.BXJGShop)}/index.html",
                                            permissionDependency: new SimplePermissionDependency(PermissionNames.Order)))
            .AddItem(new MenuItemDefinition(PermissionNames.Customer,
                                            PermissionNames.Customer.BXJGShopL(),
                                            icon: PermissionNames.Customer,
                                            url: $"/{PermissionNames.BXJGShop}/{PermissionNames.Customer.RemovePreFix(PermissionNames.BXJGShop)}/index.html",
                                            permissionDependency: new SimplePermissionDependency(PermissionNames.Customer)));
            return jczl;
        }
        /// <summary>
        /// 注册商城模块种的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGShopNavigation(this MenuDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
        /// <summary>
        /// 注册商城模块种的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGShopNavigation(this MenuItemDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
    }
}
