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
            //商城模块自己的数据字典
            var spdtzdTree = shop.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionary, BXJGShopPermissions.BXJGShopDictionary.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryCreate, BXJGShopPermissions.BXJGShopDictionaryCreate.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryUpdate, BXJGShopPermissions.BXJGShopDictionaryUpdate.L(), multiTenancySides: MultiTenancySides.Tenant);
            spdtzdTree.CreateChildPermission(BXJGShopPermissions.BXJGShopDictionaryDelete, BXJGShopPermissions.BXJGShopDictionaryDelete.L(), multiTenancySides: MultiTenancySides.Tenant);
        }
    }
}
