using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.EquipmentInfo
{
    public interface IBXJGEquipmentInfoAppService : IAsyncCrudAppService<EquipmentInfoDto, long, EquipmentInfoGetAllInput, EquipmentInfoEditDto>
    {
    }
}
