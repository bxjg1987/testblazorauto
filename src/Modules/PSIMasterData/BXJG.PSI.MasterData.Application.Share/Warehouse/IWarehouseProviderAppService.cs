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
    /// 仓库提供者应用服务接口
    /// 用于提供仓库的下拉数据选择功能
    /// </summary>
    public interface IWarehouseProviderAppService : IProviderBaseAppService<WarehousePagedResultRequestDto,
                                                                        WarehouseSelectDto,
                                                                        Guid>
    {
    }
}
