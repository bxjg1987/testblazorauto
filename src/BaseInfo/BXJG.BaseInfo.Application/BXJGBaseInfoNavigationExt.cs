using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using BXJG.BaseInfo.Authorization;
using BXJG.BaseInfo.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.BaseInfo
{
    /// <summary>
    /// 为了模块的使用方更容易的将此模块中的菜单插入到主菜单的任意节点，这里定义此模块中的菜单相关的扩展方法
    /// </summary>
    public static class BXJGBaseInfoNavigationExt
    {
        static MenuItemDefinition Create()
        {
            var jczl = new MenuItemDefinition(BXJGBaseInfoPermissionNames.BXJGBaseInfo,
                                              BXJGBaseInfoPermissionNames.BXJGBaseInfo.BXJGBaseInfoL(),
                                              icon: "dangan",
                                              permissionDependency: new SimplePermissionDependency(BXJGBaseInfoPermissionNames.BXJGBaseInfo));

            //初始化更多菜单或子菜单
            //{codegenerator}

            //添加设备信息的菜单定义
            jczl.AddItem(new MenuItemDefinition(name: BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative,
                                                displayName: BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative.BXJGBaseInfoL(),
                                                icon: "qizi",
                                                url: $"/{BXJGBaseInfoPermissionNames.BXJGBaseInfo}/{BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative}/index.html",
                                                requiresAuthentication: true,
                                                permissionDependency: new SimplePermissionDependency(BXJGBaseInfoPermissionNames.BXJGBaseInfoAdministrative)));
            return jczl;
        }

        public static MenuDefinition AddBXJGBaseInfoNavigation(this MenuDefinition parent)
        {
            parent.AddItem(Create());
            return parent;
        }
        public static MenuItemDefinition AddBXJGBaseInfoNavigation(this MenuItemDefinition parent)
        {
            parent.AddItem(Create());
            return parent;  }
    }
}
