using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Localization;
using Abp.Domain.Uow;
using Abp.Authorization;
using BXJG.Common.Dto;
using BXJG.Utils.Localization;
using BXJG.WorkOrder.WorkOrderType;

namespace BXJG.WorkOrder.WorkOrderType
{

    public class WorkOrderTypeDefineDto : ComboboxItemDto
    {
        public WorkOrderTypeDefineDto(string value, string displayText) : base(value, displayText)
        {
        }

        public bool IsDefault { get; set; }
    }
    /// <summary>
    /// 工单类型应用服务
    /// </summary>
    [AbpAuthorize]
    public class BXJGWorkOrderTypeAppService : AppServiceBase
    {
        WorkOrderTypeManager wotManager;

        public BXJGWorkOrderTypeAppService(WorkOrderTypeManager config)
        {
            this.wotManager = config;
        }
        /// <summary>
        /// 获取所有工单类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public IEnumerable<WorkOrderTypeDefineDto> GetAll(GetForSelectInput input)
        {
            var list = wotManager.List.Select(c => new WorkOrderTypeDefineDto(c.Name, c.DisplayName.Localize(LocalizationManager)) { IsDefault = c.IsDefault }).ToList();
            if (input.ForType > 0)
            {
                if (!input.ParentText.IsNullOrWhiteSpace())
                {
                    list.Insert(0, new WorkOrderTypeDefineDto(null, input.ParentText));
                }
                else if (input.ForType <= 2)
                {
                    list.Insert(0, new WorkOrderTypeDefineDto(null, "==工单类型==".BXJGWorkOrderL()));
                }
                else if (input.ForType <= 4)
                {
                    list.Insert(0, new WorkOrderTypeDefineDto(null, "==请选择==".UtilsL()));
                }
            }
            return list;
        }
    }
}
