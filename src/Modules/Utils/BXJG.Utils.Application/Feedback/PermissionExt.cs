using Abp.Authorization;
using Abp.MultiTenancy;
using BXJG.Utils.Application.Share.Auth;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Authorization
{
    public static class PermissionExt
    {
        public static Permission AddFreedback(this Permission permissionBaseInfo) {
            var permission = permissionBaseInfo.CreateChildPermission(PermissionNames.FeedbackAdmin,
       PermissionNames.FeedbackAdmin.UtilsLI(),
        multiTenancySides: MultiTenancySides.Tenant| MultiTenancySides.Host);

            permission.CreateChildPermission(PermissionNames.FeedbackGetPermissionName,
                "查询".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant | MultiTenancySides.Host);

            permission.CreateChildPermission(PermissionNames.FeedbackCreatePermissionName,
                "新增".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant | MultiTenancySides.Host,
                properties: new Dictionary<string, object> { { "btn", true } });
            permission.CreateChildPermission(PermissionNames.FeedbackUpdatePermissionName,
                "修改".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant | MultiTenancySides.Host,
                properties: new Dictionary<string, object> { { "btn", true } });
            permission.CreateChildPermission(PermissionNames.FeedbackDeletePermissionName,
                "删除".UtilsLI(),
                multiTenancySides: MultiTenancySides.Tenant | MultiTenancySides.Host,
                properties: new Dictionary<string, object> { { "btn", true } });

            return permission;
        }
    }
}
