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

        WorkOrderTypeManager bXJGWorkOrderConfig;
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> ownRepository,
                                           CategoryManager organizationUnitManager,
                                           WorkOrderTypeManager bXJGWorkOrderConfig,
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
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderType].DisplayName.Localize(LocalizationManager);
            };
        }

        protected override IQueryable<CategoryEntity> GetAllFiltered(GetAllWorkOrderCategoryInput q, string parentCode)
        {
            return base.GetAllFiltered(q, parentCode).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderType.IsNullOrWhiteSpace()));
        }

        public override async Task<WorkOrderCategroyDto> CreateAsync(WorkOrderCategoryEditInput input)
        {
            if (!input.WorkOrderType.IsNullOrWhiteSpace() && !bXJGWorkOrderConfig.ContainsKey(input.WorkOrderType))
                throw new ApplicationException("不支持的工单类型");
            await HandDefault(input.WorkOrderType, input.IsDefault);
            return await base.CreateAsync(input);
        }

        public override async Task<WorkOrderCategroyDto> UpdateAsync(WorkOrderCategoryEditInput input)
        {
            if (!input.WorkOrderType.IsNullOrWhiteSpace() && !bXJGWorkOrderConfig.ContainsKey(input.WorkOrderType))
                throw new ApplicationException("不支持的工单类型");
            await HandDefault(input.WorkOrderType, input.IsDefault);
            return await base.UpdateAsync(input);
        }
        //这里体现出来做默认类别还是用settings科学点，暂时这么招吧
        async Task HandDefault(string workOrderType, bool isDefault)
        {
            if (!isDefault)
                return;


            var list = await ownRepository.GetAll().Where(c => c.WorkOrderType == workOrderType).ToListAsync();

            foreach (var item in list)
            {
                item.IsDefault = false;
            }
            //var query = this.ownRepository.GetAll().Where(c=>string.IsNullOrWhiteSpace( workOrderType)|| c.WorkOrderType )
        }
    }
}
