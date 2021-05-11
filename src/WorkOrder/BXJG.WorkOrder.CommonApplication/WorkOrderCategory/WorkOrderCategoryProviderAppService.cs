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
using Abp.Authorization;
using BXJG.WorkOrder.WorkOrderType;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    [AbpAuthorize]
    public class WorkOrderCategoryProviderAppService : UnAuthGeneralTreeAppServiceBase<GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryTreeNodeDto,
                                                                                       GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryComboboxItemDto,
                                                                                       CategoryEntity>
    {
        public WorkOrderCategoryProviderAppService(IRepository<CategoryEntity, long> ownRepository,
                                                   WorkOrderTypeManager bXJGWorkOrderConfig) : base(ownRepository)
        {
            base.ComboboxMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderTypes.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
            };
            base.ComboTreeMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderTypes.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
            };
        }
        protected override IQueryable<CategoryEntity> ComboboxFilterAsync(GetWorkOrderCategoryForSelectInput q, long? parentId)
        {
            return base.ComboboxFilterAsync(q, parentId).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderTypes == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderTypes.IsNullOrWhiteSpace()));
        }
        protected override IQueryable<CategoryEntity> ComboTreeFilterAsync(GetWorkOrderCategoryForSelectInput q, string parentCode)
        {
            return base.ComboTreeFilterAsync(q, parentCode).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderTypes == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderTypes.IsNullOrWhiteSpace()));
        }
    }
}
