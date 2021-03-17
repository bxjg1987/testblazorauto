using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    public class WorkOrderCategoryProviderAppService : UnAuthGeneralTreeAppServiceBase<GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryTreeNodeDto,
                                                                                       GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryComboboxItemDto,
                                                                                       CategoryEntity>
    {
        public WorkOrderCategoryProviderAppService(IRepository<CategoryEntity, long> ownRepository, 
                                                   string allTextForSearch = "不限", 
                                                   string allTextForForm = "请选择") : base(ownRepository,
                                                                                            allTextForSearch, 
                                                                                            allTextForForm)
        {
        }
    }
}
