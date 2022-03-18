using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Utils.GeneralTree
{
    /// <summary>
    /// 为了模块的使用方更容易的将此模块中的菜单插入到主菜单的任意节点，这里定义此模块中的菜单相关的扩展方法
    /// </summary>
    public static class GeneralTreeNavigationExt
    {
        static MenuItemDefinition Create()
        {
            var zcgl = new MenuItemDefinition(BXJGUtilsConsts.GeneralTreeMenuName,
                                              BXJGUtilsConsts.GeneralTreeMenuName.UtilsLI(),
                                              icon: "generalTree",
                                              permissionDependency: new SimplePermissionDependency(BXJGUtilsConsts.GeneralTreeGetPermissionName));

            return zcgl;
        }

        public static MenuItemDefinition AddGeneralTreeNavigation(this MenuDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
        public static MenuItemDefinition AddGeneralTreeNavigation(this MenuItemDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
    }
}
