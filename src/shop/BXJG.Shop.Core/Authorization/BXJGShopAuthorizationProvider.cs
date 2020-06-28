using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.Shop.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;

namespace BXJG.Shop.Authorization
{
    //由于模块中的权限应该加入到主程序中，因此此逻辑移植到静态方法中
    public static class BXJGShopAuthorizationProvider// : AuthorizationProvider
    {
        /// <summary>
        /// 商城相关功能权限定义
        /// </summary>
        /// <param name="context"></param>
        public static Permission SetPermissions( Permission context)
        {
            //商城管理
            var shop = context.CreateChildPermission(BXJGShopPermissions.BXJGShop, BXJGShopPermissions.BXJGShop.BXJGShopL());
            //商城模块自己的数据字典
            var spdtzdTree = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionary, BXJGShopPermissions.BXJGShopDictionary.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //商品分类
            var itemCategory = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCategory, BXJGShopPermissions.BXJGShopItemCategory.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCategoryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCategoryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            itemCategory.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCategoryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //商品
            var item = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopItem, BXJGShopPermissions.BXJGShopItem.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            //订单
            var order = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopOrder, BXJGShopPermissions.BXJGShopOrder.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(BXJGShopPermissions.BXJGShopOrderShip, "发货".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(BXJGShopPermissions.BXJGShopOrderRefund, "退款".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            order.CreateChildPermission(BXJGShopPermissions.BXJGShopOrderCancel, "取消".BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);

            //顾客
            var Customer = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopCustomer, BXJGShopPermissions.BXJGShopCustomer.BXJGShopL(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(BXJGShopPermissions.BXJGShopCustomerCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(BXJGShopPermissions.BXJGShopCustomerUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            Customer.CreateChildPermission(BXJGShopPermissions.BXJGShopCustomerDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            return context;
        }
    }
}
