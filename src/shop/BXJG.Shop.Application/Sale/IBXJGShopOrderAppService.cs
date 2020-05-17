using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 后台管理订单的应用服务接口
    /// </summary>
    public interface IBXJGShopOrderAppService : IApplicationService
    {
        Task<PagedResultDto<OrderDto>> GetAllAsync(GetAllOrderInput input);
    }
}
