using Abp.Authorization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BXJG.Utils.GeneralTree
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
            var root = parent.CreateChildPermission(BXJGUtilsConsts.GeneralTreeGetPermissionName,
                                                    BXJGUtilsConsts.GeneralTreeGetPermissionName.UtilsLI(),
                                                    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            root.CreateChildPermission(BXJGUtilsConsts.GeneralTreeCreatePermissionName,
                                       "新增".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            root.CreateChildPermission(BXJGUtilsConsts.GeneralTreeUpdatePermissionName,
                                       "修改".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            root.CreateChildPermission(BXJGUtilsConsts.GeneralTreeDeletePermissionName,
                                       "删除".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            return root;
        }
    }
}
