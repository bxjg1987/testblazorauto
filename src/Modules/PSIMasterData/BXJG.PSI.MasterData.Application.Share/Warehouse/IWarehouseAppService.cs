using Abp.Application.Services.Dto;
using BXJG.Utils.Application.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.PSI.MasterData.Application.Share.Warehouse
{
    /// <summary>
    /// 仓库应用服务接口
    /// </summary>
    public interface IWarehouseAppService : ICrudBaseAppService<WarehouseDto,
                                                               Guid,
                                                               WarehousePagedResultRequestDto,
                                                               WarehouseEditDto,
                                                               WarehouseEditDto,
                                                               EntityDto<Guid>,
                                                               EntityDto<Guid>>
    {
    }
}
