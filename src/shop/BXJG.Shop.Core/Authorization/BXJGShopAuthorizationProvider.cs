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
            var shop = context.CreateChildPermission(BXJGShopPermissions.BXJGShop, BXJGShopPermissions.BXJGShop.L());
            //商城模块自己的数据字典
            var spdtzdTree = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionary, BXJGShopPermissions.BXJGShopDictionary.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //商品
            var item = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopItem, BXJGShopPermissions.BXJGShopItem.L(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(BXJGShopPermissions.BXJGShopItemDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);


            return context;
        }
    }
}
