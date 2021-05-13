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
using Abp.Linq;
using Microsoft.EntityFrameworkCore;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    //[AbpAuthorize]
    public class WorkOrderCategoryProviderAppService : UnAuthGeneralTreeAppServiceBase<GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryTreeNodeDto,
                                                                                       GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryComboboxItemDto,
                                                                                       CategoryEntity>
    {
        protected readonly CategoryManager categoryManager;

        public WorkOrderCategoryProviderAppService(IRepository<CategoryEntity, long> ownRepository,
                                                   WorkOrderTypeManager workOrderTypeManager,
                                                   CategoryManager categoryManager) : base(ownRepository)
        {
            this.categoryManager = categoryManager;
            //base.ComboboxMap = (entity, dto) =>
            //{
            //    dto.WorkOrderTypeName = entity.WorkOrderTypes.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
            //};
            //base.ComboTreeMap = (entity, dto) =>
            //{
            //    dto.WorkOrderTypeName = entity.WorkOrderTypes.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
            //};
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboboxFilterAsync(GetWorkOrderCategoryForSelectInput input, long? parentId, IDictionary<string, object> context = null)
        {
            var query = await base.ComboboxFilterAsync(input, parentId, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.WorkOrderType, input.ContainsNullWorkOrderType);
        }

        protected override ValueTask<List<WorkOrderCategoryComboboxItemDto>> EntityToComboboDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        {
            entities.HandDefault();
            return base.EntityToComboboDtoAsync(entities, context);
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboTreeFilterAsync(GetWorkOrderCategoryForSelectInput input, string parentCode, IDictionary<string, object> context = null)
        {
            var query = await base.ComboTreeFilterAsync(input, parentCode, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.WorkOrderType, input.ContainsNullWorkOrderType);
        }
        
        protected override ValueTask<List<WorkOrderCategoryTreeNodeDto>> EntityToTreeDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        {
            entities.HandDefault();
            return base.EntityToTreeDtoAsync(entities, context);
        }
    }
}
