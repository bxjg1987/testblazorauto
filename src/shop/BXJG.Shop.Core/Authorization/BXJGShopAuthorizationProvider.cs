using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.Shop.Localization;
using Abp.MultiTenancy;

namespace BXJG.Shop.Authorization
{
    public class BXJGShopAuthorizationProvider : AuthorizationProvider
    {
        /// <summary>
        /// 商城相关功能权限定义
        /// </summary>
        /// <param name="context"></param>
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //商城管理
            var shop = context.CreatePermission(BXJGShopPermissions.BXJGShop, BXJGShopPermissions.BXJGShop.L());
            //商品动态字段数
            var spdtzdTree = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopCatalogueItemDynamicField, BXJGShopPermissions.BXJGShopCatalogueItemDynamicField.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldCreate, BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldCreate.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldUpdate, BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldUpdate.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldDelete, BXJGShopPermissions.BXJGShopCatalogueItemDynamicFieldDelete.L(), multiTenancySides: MultiTenancySides.Tenant);
        }
    }
}
