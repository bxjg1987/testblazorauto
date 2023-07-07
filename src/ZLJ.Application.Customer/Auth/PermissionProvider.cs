using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLJ.App.Customer.Auth
{
    public class PermissionProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var cust = context.CreatePermission(PermissionNames.Customer,
     PermissionNames.Customer.GetLocalizableString(),
     multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //       var ou = cust.CreateChildPermission(PermissionNames.OU,
            //PermissionNames.OU.GetLocalizableString(),
            //multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //       ou.CreateChildPermission(PermissionNames.OUCreate,
            //           "新增".UtilsLI(),
            //           multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            //       ou.CreateChildPermission(PermissionNames.OUUpdate,
            //           "修改".UtilsLI(),
            //           multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            //       ou.CreateChildPermission(PermissionNames.OUDelete,
            //           "删除".UtilsLI(),
            //           multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });

            //        var statff = cust.CreateChildPermission(PermissionNames.Staff,
            //PermissionNames.Staff.GetLocalizableString(),
            //multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //        statff.CreateChildPermission(PermissionNames.StaffCreate,
            //            "新增".UtilsLI(),
            //            multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            //        statff.CreateChildPermission(PermissionNames.StaffUpdate,
            //            "修改".UtilsLI(),
            //            multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            //        statff.CreateChildPermission(PermissionNames.StaffDelete,
            //            "删除".UtilsLI(),
            //            multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });

        }
    }
}
