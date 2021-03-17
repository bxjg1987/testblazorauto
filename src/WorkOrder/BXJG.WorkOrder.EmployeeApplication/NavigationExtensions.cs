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
        static MenuItemDefinition Create()
        {
            throw new NotImplementedException();
            //var root = new MenuItemDefinition(CoreConsts.WorkOrder,
            //                                  CoreConsts.WorkOrderManager.BXJGWorkOrderL(),
            //                                  icon: "BXJGShopOrder",
            //                                  permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrder));

            ////代码生成器的占位符，它将在这里插入更多菜单
            ////{codegenerator}

            //root.AddItem(new MenuItemDefinition(CoreConsts.WorkOrderCategoryManager,
            //                                    CoreConsts.WorkOrderCategoryManager.BXJGWorkOrderL(),
            //                                    icon: "BXJGShopProductCategory",
            //                                    url: $"/{CoreConsts.WorkOrder}/{CoreConsts.WorkOrderCategoryManager.RemovePreFix(CoreConsts.WorkOrder)}/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrderCategoryManager)))
            //    .AddItem(new MenuItemDefinition(CoreConsts.WorkOrderManager,
            //                                    CoreConsts.WorkOrderManager.BXJGWorkOrderL(),
            //                                    icon: "BXJGShopOrder",
            //                                    url: $"/{CoreConsts.WorkOrder}/{CoreConsts.WorkOrderManager.RemovePreFix(CoreConsts.WorkOrder)}/index.html",
            //                                    permissionDependency: new SimplePermissionDependency(CoreConsts.WorkOrderManager)));

            //return root;
        }
        /// <summary>
        /// 注册工单模块中员工需要的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderEmployeeNavigation(this MenuDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
        /// <summary>
        /// 注册工单模块中员工需要的菜单
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static MenuItemDefinition AddBXJGWorkOrderEmployeeNavigation(this MenuItemDefinition parent)
        {
            var p = Create();
            parent.AddItem(p);
            return p;
        }
    }
}
