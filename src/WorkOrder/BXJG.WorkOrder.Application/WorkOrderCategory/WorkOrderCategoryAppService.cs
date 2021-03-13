using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class WorkOrderCategoryAppService : GeneralTreeAppServiceBase<WorkOrderCategroyDto,
                                                                         WorkOrderCategoryEditInput,
                                                                         GetAllWorkOrderCategoryInput,
                                                                         GetWorkOrderCategoryForSelectInput,
                                                                         WorkOrderCategoryTreeNodeDto,
                                                                         GetWorkOrderCategoryForSelectInput,
                                                                         WorkOrderCategoryComboboxItemDto,
                                                                         GeneralTreeNodeMoveInput,
                                                                         CategoryEntity,
                                                                         CategoryManager>
    {
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> ownRepository,
                                           CategoryManager organizationUnitManager,
                                           string allTextForManager = "全部",
                                           string allTextForSearch = "不限",
                                           string allTextForForm = "请选择") : base(ownRepository,
                                                                                    organizationUnitManager,
                                                                                    CoreConsts.WorkOrderCategoryCreate,
                                                                                    CoreConsts.WorkOrderCategoryUpdate,
                                                                                    CoreConsts.WorkOrderCategoryDelete,
                                                                                    CoreConsts.WorkOrderCategoryManager,
                                                                                    allTextForManager,
                                                                                    allTextForSearch,
                                                                                    allTextForForm)
        { }
    }
}
