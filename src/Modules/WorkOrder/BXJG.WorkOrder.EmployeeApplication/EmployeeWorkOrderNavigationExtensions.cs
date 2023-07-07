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
        public static MenuItemDefinition AddBXJGWorkOrderNav(this IHasMenuItemDefinitions parent)
        {
            var item = new MenuItemDefinition(CoreConsts.EmployeeWorkOrderManager,
                                          CoreConsts.EmployeeWorkOrderManager.BXJGWorkOrderLI(),
                                          icon: CoreConsts.EmployeeWorkOrderManager,
                                          url: $"/{CoreConsts.WorkOrder}/{CoreConsts.EmployeeWorkOrderManager.RemovePreFix(CoreConsts.WorkOrder)}/index.html",
                                          permissionDependency: new SimplePermissionDependency(CoreConsts.EmployeeWorkOrderManager));
            parent.Items.Add(item);
            return item;
        }
    }
}
