using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Extensions;
using BXJG.WorkOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder
{
    public static class EmployeeWorkOrderNavigationExtensions
    {
        static MenuItemDefinition Create()
        {
            return new MenuItemDefinition(CoreConsts.EmployeeWorkOrderManager,
                                          CoreConsts.EmployeeWorkOrderManager.BXJGWorkOrderLI(),
                                          icon: CoreConsts.EmployeeWorkOrderManager,
                                          url: $"/{CoreConsts.WorkOrder}/{CoreConsts.EmployeeWorkOrderManager.RemovePreFix(CoreConsts.WorkOrder)}/index.html",
                                          permissionDependency: new SimplePermissionDependency(CoreConsts.EmployeeWorkOrderManager));
        }
        /// <summary>
        /// 注册工单模块中员工需要的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuDefinition AddBXJGEmployeeWorkOrderNavigation(this MenuDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return parent;
        }
        /// <summary>
        /// 注册工单模块中员工需要的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGEmployeeWorkOrderNavigation(this MenuItemDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return parent;
        }
    }
}
