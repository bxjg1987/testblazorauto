using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.Shop.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;

namespace BXJG.Shop.Authorization
{
    public static class PermissionExtensions
    {
        /// <summary>
        /// 商城相关功能权限定义
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGShopPermission(this Permission context)
        {
            //商城管理
            var shop = context.CreateChildPermission(PermissionNames.BXJGShop, PermissionNames.BXJGShop.BXJGShopL());
            ////商城模块自己的数据字典
            //var spdtzdTree = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionary, BXJGShopPermissions.BXJGShopDictionary.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //商品分类
            var itemCategory = shop.CreateChildPermission(PermissionNames.ProductCategory, PermissionNames.ProductCategory.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(PermissionNames.ProductCategoryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(PermissionNames.ProductCategoryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(PermissionNames.ProductCategoryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //商品
            var item = shop.CreateChildPermission(PermissionNames.Product, PermissionNames.Product.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(PermissionNames.ProductCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(PermissionNames.ProductUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(PermissionNames.ProductDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //订单
            var order = shop.CreateChildPermission(PermissionNames.Order, PermissionNames.Order.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(PermissionNames.OrderShip, "发货".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(PermissionNames.OrderRefund, "退款".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(PermissionNames.OrderCancel, "取消".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);

            //顾客
            var Customer = shop.CreateChildPermission(PermissionNames.Customer, PermissionNames.Customer.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(PermissionNames.CustomerCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(PermissionNames.CustomerUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(PermissionNames.CustomerDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            return context;
        }
    }
}
