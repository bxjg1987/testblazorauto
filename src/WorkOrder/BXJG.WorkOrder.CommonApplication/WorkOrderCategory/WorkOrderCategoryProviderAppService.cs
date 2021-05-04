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
                                                   BXJGWorkOrderConfig bXJGWorkOrderConfig,
                                                   string allTextForSearch = "不限",
                                                   string allTextForForm = "请选择") : base(ownRepository,
                                                                                            allTextForSearch,
                                                                                            allTextForForm)
        {
            base.ComboboxMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig.WorkOrderTypes[entity.WorkOrderType].Localize(LocalizationManager);
            };
            base.ComboTreeMap = (entity, dto) =>
            {
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace()? default: bXJGWorkOrderConfig.WorkOrderTypes[entity.WorkOrderType].Localize(LocalizationManager);
            };
        }
        protected override IQueryable<CategoryEntity> ComboboxFilter(GetWorkOrderCategoryForSelectInput q, long? parentId)
        {
            return base.ComboboxFilter(q, parentId).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderType.IsNullOrWhiteSpace()));
        }
        protected override IQueryable<CategoryEntity> ComboTreeFilter(GetWorkOrderCategoryForSelectInput q, string parentCode)
        {
            return base.ComboTreeFilter(q, parentCode).WhereIf(!q.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == q.WorkOrderType || (q.ContainsNullWorkOrderType && c.WorkOrderType.IsNullOrWhiteSpace()));
        }
    }
}
