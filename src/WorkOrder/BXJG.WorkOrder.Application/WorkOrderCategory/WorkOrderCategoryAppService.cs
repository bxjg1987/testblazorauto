using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
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

        BXJGWorkOrderConfig bXJGWorkOrderConfig;
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> ownRepository,
                                           CategoryManager organizationUnitManager,
                                           BXJGWorkOrderConfig bXJGWorkOrderConfig,
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
        {
            this.bXJGWorkOrderConfig = bXJGWorkOrderConfig;
            base.GetAllMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig.WorkOrderTypes[entity.WorkOrderType];
            };
        }

        protected override IQueryable<CategoryEntity> GetAllFiltered(GetAllWorkOrderCategoryInput q, string parentCode)
        {
            return base.GetAllFiltered(q, parentCode).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == q.WorkOrderType);
        }

        public override Task<WorkOrderCategroyDto> CreateAsync(WorkOrderCategoryEditInput input)
        {
            if (!bXJGWorkOrderConfig.WorkOrderTypes.ContainsKey(input.WorkOrderType))
                throw new ApplicationException("不支持的工单类型");
            return base.CreateAsync(input);
        }

        public override Task<WorkOrderCategroyDto> UpdateAsync(WorkOrderCategoryEditInput input)
        {
            if (!bXJGWorkOrderConfig.WorkOrderTypes.ContainsKey(input.WorkOrderType))
                throw new ApplicationException("不支持的工单类型");
            return base.UpdateAsync(input);
        }
    }
}
