using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BXJG.Utils.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 提供与订单相关的，前后端都需要用到的接口
    /// </summary>
    public interface IBXJGCommonOrderAppService : IApplicationService
    {
        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ComboboxItemDto> GetAllOrderStatus(GetForSelectInput input);
        /// <summary>
        /// 获取支付状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ComboboxItemDto> GetAllPaymentStatus(GetForSelectInput input);
        /// <summary>
        /// 获取物流状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<ComboboxItemDto> GetAllLogisticsStatus(GetForSelectInput input);
    }
}
