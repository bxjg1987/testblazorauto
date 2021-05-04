using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using BXJG.Common.Dto;
using BXJG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    [AbpAuthorize]
    public class WorkOrderCommonAppService : AppServiceBase
    {
        /// <summary>
        /// 获取工单状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public List<ComboboxItemDto> GetAllStatus(GetForSelectInput input)
        {
            return new EnumManager(LocalizationSource).ConvertToComboboxData<Status>(input);
        }
        /// <summary>
        /// 获取工单紧急程度列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(IsDisabled = true)]
        public List<ComboboxItemDto> GetAllUrgencyDegree(GetForSelectInput input)
        {
            return new EnumManager(LocalizationSource).ConvertToComboboxData<UrgencyDegree>(input);
        }
    }
}
