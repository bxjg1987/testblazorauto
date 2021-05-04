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
        [UnitOfWork(false)]
        public IEnumerable<ComboboxItemDto> GetAllAsync()
        {
            return config.WorkOrderTypes.Select(c => new ComboboxItemDto(c.Key, c.Value));
        }
    }
}
