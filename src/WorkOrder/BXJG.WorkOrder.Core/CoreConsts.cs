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

        public const string WorkOrder = "BXJGWorkOrder";
        public const string WorkOrderManager = "BXJGWorkOrderManager";

        //public const string WorkOrderGet = "BXJGWorkOrderGet";
        public const string WorkOrderCreate = "BXJGWorkOrderCreate";
        public const string WorkOrderUpdate = "BXJGWorkOrderUpdate";
        public const string WorkOrderDelete = "BXJGWorkOrderDelete";

        public const string WorkOrderCategory = "BXJGWorkOrderCategory";
        public const string WorkOrderCategoryManager = "BXJGWorkOrderCategoryManager";

        //public const string WorkOrderCategoryGet = "BXJGWorkOrderCategoryGet";
        public const string WorkOrderCategoryCreate = "BXJGWorkOrderCategoryCreate";
        public const string WorkOrderCategoryUpdate = "BXJGWorkOrderCategoryUpdate";
        public const string WorkOrderCategoryDelete = "BXJGWorkOrderCategoryDelete";
        #endregion

    }
}
