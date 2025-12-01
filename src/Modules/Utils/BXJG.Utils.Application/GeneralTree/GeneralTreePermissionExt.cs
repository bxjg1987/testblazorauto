using Abp.Authorization;
using BXJG.Utils.Application.Share.Auth;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Abp.Authorization
{
    public static class GeneralTreePermissionExt
    {
        /// <summary>
        /// 将基础信息模块中 权限树 的顶级节点 添加到 此节点的子节点中。返回此节点以便实现链式编程
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Permission AddGeneralTreePermission(this Permission parent)
        {
            var root = parent.CreateChildPermission(PermissionNames.GeneralTreeMenuName,
                                                    PermissionNames.GeneralTreeMenuName.UtilsLI(),
                                                    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            root.CreateChildPermission(PermissionNames.GeneralTreeCreatePermissionName,
                                       "新增".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            root.CreateChildPermission(PermissionNames.GeneralTreeUpdatePermissionName,
                                       "修改".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            root.CreateChildPermission(PermissionNames.GeneralTreeDeletePermissionName,
                                       "删除".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant, properties: new Dictionary<string, object> { { "btn", true } });
            return root;
        }
    }
}
