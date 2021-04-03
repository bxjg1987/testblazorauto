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
        /// 注册工单模块管理端权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderAllPermission(this Permission context)
        {
            var workOrder = context.AddBXJGWorkOrderPermission();
            workOrder.AddBXJGWorkOrderCategoryPermission();
            workOrder.AddBXJGDefaultWorkOrderPermission();
            workOrder.AddBXJGEmployeeWorkOrderPermission();
            return workOrder;
        }
        /// <summary>
        /// 注册工单模块管理端根权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderPermission(this Permission context)
        {
            return context.CreateChildPermission(CoreConsts.WorkOrder, CoreConsts.WorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
        }
        /// <summary>
        /// 注册工单模块管理端工单分类权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderCategoryPermission(this Permission root)
        {
            var category = root.CreateChildPermission(CoreConsts.WorkOrderCategoryManager, CoreConsts.WorkOrderCategoryManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            category.CreateChildPermission(CoreConsts.WorkOrderCategoryDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            return category;
        }
        /// <summary>
        /// 注册工单模块管理端默认工单权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGDefaultWorkOrderPermission(this Permission root)
        {
            var item = root.CreateChildPermission(CoreConsts.WorkOrderManager, CoreConsts.WorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderCreate, "新增".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderUpdate, "修改".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderDelete, "删除".UtilsLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderToBeConfirmed, "待确认".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderConfirme, "确认".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderAllocate, "分配".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderExecute, "执行".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderCompletion, "完成".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            item.CreateChildPermission(CoreConsts.WorkOrderReject, "拒绝".BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
            return item;
        }
        /// <summary>
        /// 注册工单模块员工端默认工单权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGEmployeeWorkOrderPermission(this Permission context)
        {
            return context.CreateChildPermission(CoreConsts.EmployeeWorkOrderManager, CoreConsts.EmployeeWorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
        }

        /// <summary>
        /// 注册工单模块管理端权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderAllPermission(this IPermissionDefinitionContext context)
        {
            var workOrder = context.AddBXJGWorkOrderPermission();
            workOrder.AddBXJGWorkOrderCategoryPermission();
            workOrder.AddBXJGDefaultWorkOrderPermission();
            workOrder.AddBXJGEmployeeWorkOrderPermission();
            return workOrder;
        }
        /// <summary>
        /// 注册工单模块管理端根权限
        /// </summary>
        /// <param name="context"></param>
        public static Permission AddBXJGWorkOrderPermission(this IPermissionDefinitionContext context)
        {
            return context.CreatePermission(CoreConsts.WorkOrder, CoreConsts.WorkOrderManager.BXJGWorkOrderLI(), multiTenancySides: MultiTenancySides.Tenant);
        }
    }
}
