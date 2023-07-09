using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.App.Customer.Authorization.Permissions;

namespace ZLJ.App.Common.Authorization
{
    public class CustAppAuthorizationProvider : AuthorizationProvider
    {
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
