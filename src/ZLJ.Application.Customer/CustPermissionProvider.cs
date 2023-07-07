using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using BXJG.Utils.GeneralTree;
//using BXJG.WorkOrder;
using BXJG.Utils.Localization;
using ZLJ.Localization;
using System.Collections.Generic;
using ZLJ.App.Customer;

namespace ZLJ.App.Customer
{
    public  class CustPermissionProvider : AuthorizationProvider
    {
        //GeneralTreeModuleConfig cfg;

        //public ZLJAuthorizationProvider(GeneralTreeModuleConfig cfg)
        //{
        //    this.cfg = cfg;
        //}

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            #region 被依赖的权限
            //context.CreatePermission(PermissionNames.CommonGetRentOrderDetail);
            #endregion

            var cust = context.CreatePermission(PermissionNames.Customer,
                                                PermissionNames.Customer.GetCustLocalizableString(),
                                                multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
        }
    }
}