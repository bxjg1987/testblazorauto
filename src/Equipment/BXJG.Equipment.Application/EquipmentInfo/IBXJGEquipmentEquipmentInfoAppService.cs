using Abp.Application.Services;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Equipment.EquipmentInfo
{
    public interface IBXJGEquipmentEquipmentInfoAppService : IAsyncCrudAppService<EquipmentInfoDto, long, EquipmentInfoGetAllInput, EquipmentInfoEditDto>
    {
        Task<BatchOperationResultLong> BatchDeleteAsync(BatchOperationInputLong input);
    }
}
