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
namespace BXJG.WorkOrder
{
    /// <summary>
    /// 工单类型应用服务
    /// </summary>
    [AbpAuthorize]
    public class BXJGWorkOrderTypeAppService : AppServiceBase
    {
        BXJGWorkOrderConfig config;

        public BXJGWorkOrderTypeAppService(BXJGWorkOrderConfig config)
        {
            this.config = config;
        }
        /// <summary>
        /// 获取所有工单类型
        /// </summary>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public IEnumerable<ComboboxItemDto> GetAllAsync(GetForSelectInput input)
        {
            var list = config.WorkOrderTypes.Select(c => new ComboboxItemDto(c.Key, c.Value.Localize(LocalizationManager))).ToList();
            if (input.ForType > 0) {
                if (!input.ParentText.IsNullOrWhiteSpace())
                {
                    list.Insert(0, new ComboboxItemDto(null, input.ParentText));
                }
                else if(input.ForType <= 2)
                {
                    list.Insert(0, new ComboboxItemDto(null, "==工单类型==".BXJGWorkOrderL()));
                }
                else if (input.ForType <= 4)
                {
                    list.Insert(0, new ComboboxItemDto(null, "==请选择==".UtilsL()));
                }
            }
            return list;
        }
    }
}
