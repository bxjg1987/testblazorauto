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
            //虽然性能低，但访问不高
            base.GetAllMap = (entity, dto) =>
            {
                //dto.WorkOrderTypeName = entity.WorkOrderTypes.Count==0 ? default : bXJGWorkOrderConfig[entity.WorkOrderTypes].DisplayName.Localize(LocalizationManager);
                dto.WorkOrderTypes = entity.WorkOrderTypes.Select(c => new CategoryWorkOrderTypeDto
                {
                    //WorkOrderType = c.WorkOrderType,
                    WorkOrderTypeDisplayName = bXJGWorkOrderConfig[c.WorkOrderType].DisplayName.Localize(LocalizationManager)
                });
                //dto.WorkOrderTypeName = string.Join(',', entity.WorkOrderTypes.Select(c => bXJGWorkOrderConfig[c.WorkOrderType].DisplayName.Localize(LocalizationManager)));
            };
        }
        [UnitOfWork(false)]
        public override async Task<WorkOrderCategroyDto> GetAsync(EntityDto<long> input)
        {
            await CheckGetPermissionAsync();
            var entity = await ownRepository.GetAsync(input.Id);

            var n = ObjectMapper.Map<TDto>(entity);
            //if (!string.IsNullOrWhiteSpace(entity.ExtensionData))
            //    n.ExtData = JsonConvert.DeserializeObject<dynamic>(entity.ExtensionData);
            return n;
        }
        protected override IQueryable<CategoryEntity> GetAllFiltered(GetAllWorkOrderCategoryInput q, string parentCode)
        {
            var query = base.GetAllFiltered(q, parentCode).Include(c => c.WorkOrderTypes); //虽然性能低，但访问量不大
            if (q.WorkOrderTypes != null && q.WorkOrderTypes.Any())
            {
                Expression<Func<CategoryEntity, bool>> where1 = c => q.WorkOrderTypes.Any(d => c.WorkOrderTypes.Any(e => e.WorkOrderType == d));

                if (q.ContainsNullWorkOrderType)
                {
                    where1 = where1.Or(c => c.WorkOrderTypes == null);
                }

                return query.Where(where1);

            }

            return query;
            //.WhereIf(!q.WorkOrderTypes.IsNullOrWhiteSpace(), c => c.WorkOrderTypes == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderTypes.IsNullOrWhiteSpace()));
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

            var list = await ownRepository.GetAll().Where(c => c.WorkOrderTypes == workOrderType).ToListAsync();
            foreach (var item in list)
            {
                item.IsDefault = false;
            }
        }
    }
}
