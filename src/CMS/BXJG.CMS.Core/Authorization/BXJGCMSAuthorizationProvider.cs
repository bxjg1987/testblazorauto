using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using BXJG.CMS.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;

namespace BXJG.CMS.Authorization
{
    public static class BXJGCMSAuthorizationProvider// : AuthorizationProvider
    {
        public static Permission SetPermissions( Permission context)
        {
            //商城管理
            var shop = context.CreateChildPermission(BXJGCMSPermissions.BXJGCMS, BXJGCMSPermissions.BXJGCMS.BXJGCMSL());
            ////商城模块自己的数据字典
            //var spdtzdTree = shop.CreateChildPermission(BXJGCMSPermissions.BXJGCMSDictionary, BXJGCMSPermissions.BXJGCMSDictionary.BXJGCMSL(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGCMSPermissions.BXJGCMSDictionaryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGCMSPermissions.BXJGCMSDictionaryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //spdtzdTree.CreateChildPermission(BXJGCMSPermissions.BXJGCMSDictionaryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            ////商品
            //var item = shop.CreateChildPermission(BXJGCMSPermissions.BXJGCMSItem, BXJGCMSPermissions.BXJGCMSItem.BXJGCMSL(), multiTenancySides: MultiTenancySides.Tenant);
            //item.CreateChildPermission(BXJGCMSPermissions.BXJGCMSItemCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //item.CreateChildPermission(BXJGCMSPermissions.BXJGCMSItemUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //item.CreateChildPermission(BXJGCMSPermissions.BXJGCMSItemDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);


            return context;
        }
    }
}
