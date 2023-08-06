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
using BXJG.WorkOrder.WorkOrderType;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Expressions;
using System.Linq.Expressions;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using BXJG.Common.Dto;
using BXJG.Utils.Localization;
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
        /// <param name="repository"></param>
        /// <param name="clsManager"></param>
        /// <param name="workOrderTypeManager"></param>
        public WorkOrderCategoryAppService(IRepository<CategoryEntity, long> repository,
                                           CategoryManager clsManager,
                                           WorkOrderTypeManager workOrderTypeManager) : base(repository,
                                                                                             clsManager,
                                                                                             CoreConsts.WorkOrderCategoryCreate,
                                                                                             CoreConsts.WorkOrderCategoryUpdate,
                                                                                             CoreConsts.WorkOrderCategoryDelete,
                                                                                             CoreConsts.WorkOrderCategoryManager)
        {
            //base.LocalizationSourceName = CoreConsts.LocalizationSourceName; 基类不能用
            this.workOrderTypeManager = workOrderTypeManager;
        }

        //manager中已有新增或修改的逻辑
       
        //protected override async ValueTask<IQueryable<CategoryEntity>> GetQueryAsync(EntityDto<long> input, IDictionary<string, object> context = null)
        //{
        //    var query = await base.GetQueryAsync(input, context);
        //    return query.Include(c => c.WorkOrderTypes);
        //}
        protected override Task<CategoryEntity> GetEntityByIdAsync(long id)
        {
            return base.repository.GetAll() .Include(c => c.WorkOrderTypes).Where(c=>c.Id==id).SingleAsync();
        }

        protected override IQueryable<CategoryEntity> GetAllFiltered(GetAllWorkOrderCategoryInput input, string parentCode)
        {
            var query =  base.GetAllFiltered(input, parentCode);
            query = query.Include(c => c.WorkOrderTypes)
                         .WhereWorkOrderType(input.CategoryTypeQueryType, input.WorkOrderTypes, input.ContainsNullWorkOrderType);
            return query;
        }
        protected override void EntityToDto(CategoryEntity entity, WorkOrderCategroyDto dto)
        {
            dto.WorkOrderTypeDisplayName = "";
            foreach (var item in dto.WorkOrderTypes)
            {
                item.WorkOrderTypeDisplayName = workOrderTypeManager[item.WorkOrderType].DisplayName.Localize(LocalizationManager);
                dto.WorkOrderTypeDisplayName += item.WorkOrderTypeDisplayName;
                if (item.IsDefault)
                    dto.WorkOrderTypeDisplayName += $"({"默认".UtilsL()})";
                dto.WorkOrderTypeDisplayName += ",";
            }
            dto.WorkOrderTypeDisplayName = dto.WorkOrderTypeDisplayName.TrimEnd(',');
            if (dto.WorkOrderTypes.Any())
                dto.IsDefault = false;
        }
    }
}
