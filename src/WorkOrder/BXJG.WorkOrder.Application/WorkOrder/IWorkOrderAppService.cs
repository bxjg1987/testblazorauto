using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public interface IWorkOrderAppService<TEntityDto,
                                          in TGetAllInput,
                                          in TCreateInput,
                                          in TUpdateInput,
                                          in TGetInput,
                                          in TBatchDeleteInput,
                                          TBatchDeleteOutput,
                                          in TBatchChangeStatusInput,
                                          TBatchChangeStatusOutput> : IApplicationService
        where TEntityDto : OrderDto
        where TUpdateInput : UpdateInput
        where TGetInput : GetInput
        where TGetAllInput : GetAllInput
        where TBatchDeleteInput : BatchOperationInput
        where TBatchDeleteOutput : BatchOperationResult
        where TBatchChangeStatusInput : BatchChangeStatusInput
        where TBatchChangeStatusOutput : BatchChangeStatusOutput
    {
        Task<TEntityDto> CreateAsync(TCreateInput input);
        Task<TEntityDto> UpdateAsync(TUpdateInput input);
        Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input);
        Task<TEntityDto> GetAsync(TGetInput input);
        Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input);
        /// <summary>
        /// 万能状态修改api，内部需要调用真实的业务方法，包括回退的处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TBatchChangeStatusOutput> ChangeStatusAsync(TBatchChangeStatusInput input);
    }
}
