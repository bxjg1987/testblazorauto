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
namespace BXJG.WorkOrder.WorkOrderCategory
{
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
                //dto.IsDefault = entity.IsDefault;
                //dto.WorkOrderType = entity.WorkOrderType;
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace() ? default : bXJGWorkOrderConfig.WorkOrderTypes[entity.WorkOrderType];
            };
            base.ComboTreeMap = (entity, dto) =>
            {
                //dto.IsDefault = entity.IsDefault;
                //dto.WorkOrderType = entity.WorkOrderType;
                dto.WorkOrderTypeName = entity.WorkOrderType.IsNullOrWhiteSpace()? default: bXJGWorkOrderConfig.WorkOrderTypes[entity.WorkOrderType];
            };
        }
        protected override IQueryable<CategoryEntity> ComboboxFilter(GetWorkOrderCategoryForSelectInput input, long? parentId)
        {
            return base.ComboboxFilter(input, parentId).WhereIf(!input.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == input.WorkOrderType);
        }
        protected override IQueryable<CategoryEntity> ComboTreeFilter(GetWorkOrderCategoryForSelectInput input, string parentCode)
        {
            return base.ComboTreeFilter(input, parentCode).WhereIf(!input.WorkOrderType.IsNullOrWhiteSpace(), c => c.WorkOrderType == input.WorkOrderType);
        }
    }
}
