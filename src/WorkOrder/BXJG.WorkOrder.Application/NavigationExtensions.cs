using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Extensions;
using BXJG.WorkOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.WorkOrder
{
    public static class NavigationExtensions
    {
        /// <summary>
        /// 注册工单模块后台管理端的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderAllNav(this IHasMenuItemDefinitions parent)
        {
            var root = parent.AddBXJGWorkOrderRootNav();
            root.AddBXJGWorkOrderCategoryNav();
            root.AddBXJGWorkOrderDefaultNav();
            return root;
        }
        /// <summary>
        /// 注册工单模块后台管理端的根菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderRootNav(this IHasMenuItemDefinitions parent)
        {
            var item = new MenuItemDefinition(CoreConsts.WorkOrder,
                                              CoreConsts.WorkOrderManager.BXJGWorkOrderLI(),
                                              icon: "BXJGShopOrder",
                                              permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrder));

            parent.Items.Add(item);
            return item;
        }
        /// <summary>
        /// 注册工单模块后台管理端的工单分类菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderCategoryNav(this IHasMenuItemDefinitions parent)
        {
            var item = new MenuItemDefinition(CoreConsts.WorkOrderCategoryManager,
                                              CoreConsts.WorkOrderCategoryManager.BXJGWorkOrderLI(),
                                              icon: CoreConsts.WorkOrderCategoryManager,
                                              url: $"/{CoreConsts.WorkOrder}/WorkOrderCategory/index.html",
                                              permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrderCategoryManager));
            parent.Items.Add(item);
            return item;
        }
        /// <summary>
        /// 注册工单模块后台管理端的默认工单管理菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderDefaultNav(this IHasMenuItemDefinitions parent)
        {
            var item = new MenuItemDefinition(CoreConsts.WorkOrderManager,
                                              CoreConsts.WorkOrderManager.BXJGWorkOrderLI(),
                                              icon: "BXJGShopOrder",
                                              url: $"/{CoreConsts.WorkOrder}/WorkOrder/index.html",
                                              permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrderManager));
            parent.Items.Add(item);
            return item;
        }
    }
}
