using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前端顾客对订单的操作接口
    /// </summary>
    public interface ICustomerOrderAppService : IApplicationService
    {
        /// <summary>
        /// 前端顾客下订单
        /// </summary>
        /// <param name="input">前端顾客下单时需要提供的数据</param>
        /// <returns>创建好的订单</returns>
        Task<CustomerOrderDto> CreateAsync(CustomerOrderCreateDto input);
        /// <summary>
        /// 前台顾客发起订单支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<CustomerPaymentResult> PaymentAsync(CustomerPaymentInput input);
    }
}
