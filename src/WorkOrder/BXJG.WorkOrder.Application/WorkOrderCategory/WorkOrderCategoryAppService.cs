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
using Abp.Linq.Expressions;
using System.Linq.Expressions;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using BXJG.Common.Dto;

namespace BXJG.WorkOrder.WorkOrderCategory
{
    /// <summary>
    /// 工单分类应用服务
    /// </summary>
    public class WorkOrderCategoryAppService : GeneralTreeAppServiceBase<WorkOrderCategroyDto,
                                                                         WorkOrderCategoryEditInput,
                                                                         WorkOrderCategoryEditInput,
                                                                         BatchOperationInputLong,
                                                                         GetAllWorkOrderCategoryInput,
                                                                         EntityDto<long>,
                                                                         GeneralTreeNodeMoveInput,
                                                                         CategoryEntity,
                                                                         CategoryManager>
    {
        /// <summary>
        /// 工单类型管理器
        /// </summary>
        protected readonly WorkOrderTypeManager workOrderTypeManager;
        /// <summary>
        /// 实例化工单类别应用服务
        /// </summary>
        /// <param name="ownRepository"></param>
        /// <param name="clsManager"></param>
        /// <param name="workOrderTypeManager"></param>
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> ownRepository,
                                           CategoryManager clsManager,
                                           WorkOrderTypeManager workOrderTypeManager) : base(ownRepository,
                                                                                            clsManager,
                                                                                            CoreConsts.WorkOrderCategoryCreate,
                                                                                            CoreConsts.WorkOrderCategoryUpdate,
                                                                                            CoreConsts.WorkOrderCategoryDelete,
                                                                                            CoreConsts.WorkOrderCategoryManager)
        {
            this.workOrderTypeManager = workOrderTypeManager;
            //虽然性能低，但访问不高
            //base.GetAllMap = (entity, dto) =>
            //{
            //    //dto.WorkOrderTypeName = entity.WorkOrderTypes.Count==0 ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
            //    dto.WorkOrderTypes = entity.WorkOrderTypes.Select(c => new CategoryWorkOrderTypeDto
            //    {
            //        //WorkOrderType = c.WorkOrderType,
            //        WorkOrderTypeDisplayName = bXJGWorkOrderConfig[c.WorkOrderType].DisplayName.Localize(LocalizationManager)
            //    });
            //    //dto.WorkOrderTypeName = string.Join(',', entity.WorkOrderTypes.Select(c => bXJGWorkOrderConfig[c.WorkOrderType].DisplayName.Localize(LocalizationManager)));
            //};
        }

        protected override ValueTask BeforeCreateAsync(WorkOrderCategoryEditInput input, CategoryEntity entity, IDictionary<string, object> context = null)
        {
            return generalTreeManager.HandDefaultAsync(entity);
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> UpdateGetAsync(WorkOrderCategoryEditInput input, IDictionary<string, object> context = null)
        {
            var query = await base.UpdateGetAsync(input, context);
            return query.Include(c => c.WorkOrderTypes);
        }
        protected override ValueTask BeforeUpdateAsync(WorkOrderCategoryEditInput input, CategoryEntity entity, IDictionary<string, object> context = null)
        {
            return generalTreeManager.HandDefaultAsync(entity);
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> GetQueryAsync(EntityDto<long> input, IDictionary<string, object> context = null)
        {
            var query = await base.GetQueryAsync(input, context);
            return query.Include(c => c.WorkOrderTypes);
        }
        protected override ValueTask EntityToDtoAsync(CategoryEntity entity, WorkOrderCategroyDto dto, IDictionary<string, object> context = null)
        {
            foreach (var item in dto.WorkOrderTypes)
            {
                item.WorkOrderTypeDisplayName = item.WorkOrderType.GeneralTreeL();
            }
            return ValueTask.CompletedTask;
        }

        protected override async ValueTask<IQueryable<CategoryEntity>> GetAllFilteredAsync(GetAllWorkOrderCategoryInput input, string parentCode, IDictionary<string, object> context = null)
        {
            var query = await base.GetAllFilteredAsync(input, parentCode, context);
            query = query.Include(c => c.WorkOrderTypes)
                         .Where(generalTreeManager.GetWhereExpression(input.WorkOrderTypes, input.ContainsNullWorkOrderType));
            return query;
        }
    }
}
