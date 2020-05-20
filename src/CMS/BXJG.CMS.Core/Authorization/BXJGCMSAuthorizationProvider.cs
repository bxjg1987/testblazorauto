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
            //cms
            var cms = context.CreateChildPermission(BXJGCMSPermissions.BXJGCMS, BXJGCMSPermissions.BXJGCMS.BXJGCMSL());

            #region 广告
            //广告
            var ad = cms.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAd, BXJGCMSPermissions.BXJGCMSAd.BXJGCMSL(), multiTenancySides: MultiTenancySides.Tenant);
            ad.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            ad.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            ad.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //广告位
            var adPosition = cms.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdPosition, BXJGCMSPermissions.BXJGCMSAdPosition.BXJGCMSL(), multiTenancySides: MultiTenancySides.Tenant);
            adPosition.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdPositionCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            adPosition.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdPositionUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            adPosition.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdPositionDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            //广告控件
            var adControl = cms.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdControl, BXJGCMSPermissions.BXJGCMSAdControl.BXJGCMSL(), multiTenancySides: MultiTenancySides.Tenant);
            adControl.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdControlCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            adControl.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdControlUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            adControl.CreateChildPermission(BXJGCMSPermissions.BXJGCMSAdControlDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            #endregion


            return context;
        }
    }
}
