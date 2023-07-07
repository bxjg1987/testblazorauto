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
    public interface IWorkOrderAppService<in TCreateInput,
                                          in TUpdateInput,
                                          in TBatchDeleteInput,
                                          TBatchDeleteOutput,
                                          in TGetInput,
                                          in TGetAllInput,
                                          TEntityDto,
                                          in TBatchChangeStatusInput,
                                          TBatchChangeStatusOutput,
                                          in TBatchAllocateInput,
                                          TBatchAllocateOutput> : IApplicationService
    {
        Task<TEntityDto> CreateAsync(TCreateInput input);
        Task<TEntityDto> UpdateAsync(TUpdateInput input);
        Task<TBatchDeleteOutput> DeleteAsync(TBatchDeleteInput input);
        Task<TEntityDto> GetAsync(TGetInput input);
        Task<PagedResultDto<TEntityDto>> GetAllAsync(TGetAllInput input);
        Task<TBatchChangeStatusOutput> ConfirmeAsync(TBatchChangeStatusInput input);
        Task<TBatchAllocateOutput> AllocateAsync(TBatchAllocateInput input);
        Task<TBatchChangeStatusOutput> ExecuteAsync(TBatchChangeStatusInput input);
        Task<TBatchChangeStatusOutput> RejectAsync(TBatchChangeStatusInput input);
        Task<TBatchChangeStatusOutput> CompletionAsync(TBatchChangeStatusInput input);
    }
    /// <summary>
    /// 普通工单
    /// </summary>
    public interface IWorkOrderAppService : IWorkOrderAppService<WorkOrderCreateInputBase,
                                                                 UpdateInput,
                                                                 BatchOperationInputLong,
                                                                 BatchOperationOutputLong,
                                                                 EntityDto<long>,
                                                                 GetAllWorkOrderInputBase,
                                                                 WorkOrderDtoBase,
                                                                 WorkOrderBatchChangeStatusInputBase,
                                                                 WorkOrderBatchChangeStatusOutputBase,
                                                                 WorkOrderBatchAllocateInputBase,
                                                                 WorkOrderBatchAllocateOutputBase>
    { }
}
