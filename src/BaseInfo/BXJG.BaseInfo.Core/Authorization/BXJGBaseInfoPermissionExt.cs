using Abp.Authorization;
using BXJG.BaseInfo.Localization;
using BXJG.GeneralTree;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace BXJG.BaseInfo.Authorization
{
    public static class BXJGBaseInfoPermissionExt
    {
        /// <summary>
        /// 将基础信息模块中 权限树 的顶级节点 添加到 此节点的子节点中。返回此节点以便实现链式编程
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Permission AddBaseInfoPermission(this Permission parent)
        {
            var root = parent.CreateChildPermission(BXJGBaseInfoPermissionNames.BXJGBaseInfo,
                                                    BXJGBaseInfoPermissionNames.BXJGBaseInfo.BXJGBaseInfoL(),
                                                    multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            //{codegenerator}

            var sbxx = root.CreateChildPermission(BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative,
                                                  BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative.BXJGBaseInfoL(),
                                                  multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeCreate,
                                       "新增".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeUpdate,
                                       "修改".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);
            sbxx.CreateChildPermission(BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrativeDelete,
                                       "删除".UtilsLI(),
                                       multiTenancySides: Abp.MultiTenancy.MultiTenancySides.Tenant);

            root.AddGeneralTreePermission();

            return root;
        }
    }
}
