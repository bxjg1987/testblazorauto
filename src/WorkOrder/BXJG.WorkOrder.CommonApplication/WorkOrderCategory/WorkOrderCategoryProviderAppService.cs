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
using Abp.Linq.Extensions;
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
            //base.LocalizationSourceName = CoreConsts.LocalizationSourceName;//基类不能用
            this.categoryManager = categoryManager;
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboboxFilterAsync(GetWorkOrderCategoryForSelectInput input, long? parentId, IDictionary<string, object> context = null)
        {
            var query = await base.ComboboxFilterAsync(input, parentId, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }

        //protected override async ValueTask<List<WorkOrderCategoryComboboxItemDto>> EntityToComboboDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        //{
        //    //HandDefault(entities);
        //    return base.EntityToComboboDtoAsync(entities, context);
        //}

        protected override async ValueTask<IQueryable<CategoryEntity>> ComboTreeFilterAsync(GetWorkOrderCategoryForSelectInput input, string parentCode, IDictionary<string, object> context = null)
        {
            var sdf = base.CurrentUnitOfWork;
            var query = await base.ComboTreeFilterAsync(input, parentCode, context);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }

        protected override async ValueTask<List<WorkOrderCategoryTreeNodeDto>> EntityToTreeDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        {
            var dtos = await base.EntityToTreeDtoAsync(entities, context);
            foreach (var item in entities)
            {
                if (item.WorkOrderTypes.Any(c => c.IsDefault)) {
                    dtos.Single(c => c.Id == item.Id.ToString()).IsDefault = false;
                }
            }
            return dtos;
        }
    }
}
