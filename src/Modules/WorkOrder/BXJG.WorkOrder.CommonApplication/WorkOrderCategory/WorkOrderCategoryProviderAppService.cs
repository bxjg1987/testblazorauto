using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BXJG.Utils.GeneralTree;
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
    public class WorkOrderCategoryProviderAppService : GeneralTreeProviderBaseAppService<GetWorkOrderCategoryForSelectInput,
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
        //   override comfi
        protected override IQueryable<CategoryEntity> ComboboxFilter(GetWorkOrderCategoryForSelectInput input, long? parentId)
        {
            var query = base.ComboboxFilter(input, parentId);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }

        //protected override async ValueTask<List<WorkOrderCategoryComboboxItemDto>> EntityToComboboDtoAsync(IEnumerable<CategoryEntity> entities, IDictionary<string, object> context = null)
        //{
        //    //HandDefault(entities);
        //    return base.EntityToComboboDtoAsync(entities, context);
        //}

        protected override IQueryable<CategoryEntity> ComboTreeFilter(GetWorkOrderCategoryForSelectInput input, string parentCode)
        {
            // var sdf = base.CurrentUnitOfWork;
            var query = base.ComboTreeFilter(input, parentCode);
            return query.Include(c => c.WorkOrderTypes).WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
        }

        protected override List<WorkOrderCategoryTreeNodeDto> EntityToTreeDto(IEnumerable<CategoryEntity> entities)
        {
            var dtos = base.EntityToTreeDto(entities);
            foreach (var item in entities)
            {
                if (item.WorkOrderTypes.Any(c => c.IsDefault))
                {
                    dtos.Single(c => c.Id == item.Id.ToString()).IsDefault = false;
                }
            }
            return dtos;
        }
    }
}
