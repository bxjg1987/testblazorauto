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
    [AbpAuthorize]
    public class WorkOrderCategoryProviderAppService : UnAuthGeneralTreeAppServiceBase<GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryTreeNodeDto,
                                                                                       GetWorkOrderCategoryForSelectInput,
                                                                                       WorkOrderCategoryComboboxItemDto,
                                                                                       CategoryEntity>
    {
        protected readonly CategoryManager categoryManager;

        public WorkOrderCategoryProviderAppService(IRepository<CategoryEntity, long> ownRepository,
                                                   CategoryManager categoryManager) : base(ownRepository)
        {
            base.LocalizationSourceName = CoreConsts.LocalizationSourceName;
            this.categoryManager = categoryManager;
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboboxFilterAsync(GetWorkOrderCategoryForSelectInput input, long? parentId, IDictionary<string, object> context = null)
        {
            var query = await base.ComboboxFilterAsync(input, parentId, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }

        protected override ValueTask<List<WorkOrderCategoryComboboxItemDto>> EntityToComboboDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        {
            HandDefault(entities);
            return base.EntityToComboboDtoAsync(entities, context);
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboTreeFilterAsync(GetWorkOrderCategoryForSelectInput input, string parentCode, IDictionary<string, object> context = null)
        {
            var query = await base.ComboTreeFilterAsync(input, parentCode, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }
        
        protected override ValueTask<List<WorkOrderCategoryTreeNodeDto>> EntityToTreeDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        {
            HandDefault(entities);
            return base.EntityToTreeDtoAsync(entities, context);
        }

        /// <summary>
        /// 若关联的工单类型设置了默认，则取消未关联工单类型的分类的默认设置，调用方不应使用事务
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        static void HandDefault( IEnumerable<CategoryEntity> entities)
        {
            if (entities.Any(c => c.WorkOrderTypes.Any(d => d.IsDefault)))
            {
                var list = entities.Where(c => !c.WorkOrderTypes.Any());
                foreach (var item in list)
                {
                    item.IsDefault = false; //这里修改值，调用方不应使用事务
                }
            }
        }
    }
}
