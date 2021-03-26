using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.MultiTenancy;
using BXJG.Utils.Localization;

namespace BXJG.WorkOrder
{
    public static class PermissionExtensions
    {
        /// <summary>
        /// 注册工单模块权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderPermission(this Permission context)
        {
            var root = context.CreateChildPermission(CoreConsts.WorkOrder, CoreConsts.WorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
          
            var category = root.CreateChildPermission(CoreConsts.WorkOrderCategoryManager, CoreConsts.WorkOrderCategoryManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            var item = root.CreateChildPermission(CoreConsts.WorkOrderManager, CoreConsts.WorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);

            return context;
        }
    }
}
