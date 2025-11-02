using Abp.Authorization;
using BXJG.Utils.Application.Share.Settings;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.Authorization
{
    public static class PermissionSettingExt
    {
        //public override void SetPermissions(IPermissionDefinitionContext context)
        //{
        //    context.CreatePermission(Changliang.PermissionNameGet, "查看".UtilsLI());
        //    context.CreatePermission(Changliang.PermissionNameUpdate, "修改".UtilsLI());
        //}

        public static Permission AddSettingPermissions(this Permission permission) {
            permission.CreateChildPermission(BXJG.Utils.Application.Share.Settings.Changliang.PermissionNameGet, "查看".UtilsLI());
            permission.CreateChildPermission(BXJG.Utils.Application.Share.Settings.Changliang.PermissionNameUpdate, "修改".UtilsLI());
            return permission;
        }
    }
}
