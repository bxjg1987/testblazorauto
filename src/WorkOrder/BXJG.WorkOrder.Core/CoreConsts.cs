using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder
{
    public class CoreConsts
    {
        public const string LocalizationSourceName = "BXJGWorkOrder";

        #region 实体
        public const int OrderTitleMaxLength = 500;
        public const int OrderDescriptionMaxLength = 5000;
        public const int OrderStatusChangedDescriptionMaxLength = 500;
        public const int OrderEmployeeIdMaxLength = 64;
        public const int OrderEntityTypeMaxLength = 100;
        public const int OrderEntityIdMaxLength = 100;
        #endregion

        #region 权限和菜单

        #region 后台
        public const string WorkOrder = "BXJGWorkOrder";
        public const string WorkOrderManager = "BXJGWorkOrderManager";

        //public const string WorkOrderGet = "BXJGWorkOrderGet";
        public const string WorkOrderCreate = "BXJGWorkOrderCreate";
        public const string WorkOrderUpdate = "BXJGWorkOrderUpdate";
        public const string WorkOrderDelete = "BXJGWorkOrderDelete";
        public const string WorkOrderConfirme = "BXJGWorkOrderConfirme";
        public const string WorkOrderAllocate = "BXJGWorkOrderAllocate";
        public const string WorkOrderExecute = "BXJGWorkOrderExecute";
        public const string WorkOrderCompletion = "BXJGWorkOrderCompletion";
        public const string WorkOrderReject = "BXJGWorkOrderReject";


        public const string WorkOrderCategory = "BXJGWorkOrderCategory";
        public const string WorkOrderCategoryManager = "BXJGWorkOrderCategoryManager";

        //public const string WorkOrderCategoryGet = "BXJGWorkOrderCategoryGet";
        public const string WorkOrderCategoryCreate = "BXJGWorkOrderCategoryCreate";
        public const string WorkOrderCategoryUpdate = "BXJGWorkOrderCategoryUpdate";
        public const string WorkOrderCategoryDelete = "BXJGWorkOrderCategoryDelete";
        #endregion

        #region 员工端
        
        
        //public const string EmployeeWorkOrder = "BXJGEmployeeWorkOrder";

        public const string EmployeeWorkOrderManager = "BXJGEmployeeWorkOrderManager";
        //public const string EmployeeWorkOrderAllocate = "BXJGEmployeeWorkOrderCreate";
        //public const string EmployeeWorkOrderExecute = "BXJGEmployeeWorkOrderUpdate";
        //public const string EmployeeWorkOrderCompletion = "BXJGEmployeeWorkOrderDelete";
        #endregion

        #endregion

    }
}
