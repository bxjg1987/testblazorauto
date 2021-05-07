using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Localization;
using BXJG.WorkOrder.WorkOrderType;
using Microsoft.EntityFrameworkCore;

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
        private readonly WorkOrderTypeManager workOrderTypeManager;
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> ownRepository,
                                           CategoryManager organizationUnitManager,
                                           WorkOrderTypeManager bXJGWorkOrderConfig) : base(ownRepository,
                                                                                            organizationUnitManager,
                                                                                            CoreConsts.WorkOrderCategoryCreate,
                                                                                            CoreConsts.WorkOrderCategoryUpdate,
                                                                                            CoreConsts.WorkOrderCategoryDelete,
                                                                                            CoreConsts.WorkOrderCategoryManager)
        {
            this.workOrderTypeManager = bXJGWorkOrderConfig;
            base.GetAllMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderType].DisplayName.Localize(LocalizationManager);
            };
        }

        protected override IQueryable<CategoryEntity> GetAllFiltered(GetAllWorkOrderCategoryInput q, string parentCode)
        {
            return base.GetAllFiltered(q, parentCode).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderType.IsNullOrWhiteSpace()));
        }

        public override async Task<WorkOrderCategroyDto> CreateAsync(WorkOrderCategoryEditInput input)
        {
            await HandDefault(input.WorkOrderType, input.IsDefault);
            return await base.CreateAsync(input);
        }

        public override async Task<WorkOrderCategroyDto> UpdateAsync(WorkOrderCategoryEditInput input)
        {
            await HandDefault(input.WorkOrderType, input.IsDefault);
            return await base.UpdateAsync(input);
        }

        /// <summary>
        /// 根据工单类型处理默认分类
        /// </summary>
        /// <param name="workOrderType"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        private async ValueTask HandDefault(string workOrderType, bool isDefault)
        {
            if (!workOrderType.IsNullOrWhiteSpace() && !workOrderTypeManager.ContainsKey(workOrderType))
                throw new ApplicationException("不支持的工单类型");

            if (!isDefault)
                return;

            var list = await ownRepository.GetAll().Where(c => c.WorkOrderType == workOrderType).ToListAsync();
            foreach (var item in list)
            {
                item.IsDefault = false;
            }
        }
    }
}
