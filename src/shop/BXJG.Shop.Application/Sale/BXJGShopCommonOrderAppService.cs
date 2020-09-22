using Abp;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Localization;
using BXJG.Shop.Common;
using BXJG.Common.Dto;
using BXJG.Utils.Enums;
using BXJG.Utils.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 提供与订单相关的，前后端都需要用到的接口
    /// </summary>
    public class BXJGShopCommonOrderAppService : BXJGShopAppServiceBase, IBXJGShopCommonOrderAppService
    {
        private readonly EnumManagerFactory enumManagerFactory;
        public BXJGShopCommonOrderAppService(EnumManagerFactory enumManagerFactory)
        {
            this.enumManagerFactory = enumManagerFactory;
        }
        /// <summary>
        /// 获取订单状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ComboboxItemDto> GetAllOrderStatus(GetForSelectInput input)
        {
            return enumManagerFactory.EnumManager.GetAllOrderStatus<OrderStatus>(input);
        }
        /// <summary>
        /// 获取支付状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ComboboxItemDto> GetAllPaymentStatus(GetForSelectInput input)
        {
            return enumManagerFactory.EnumManager.GetAllOrderStatus<PaymentStatus>(input);
        }
        /// <summary>
        /// 获取物流状态列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ComboboxItemDto> GetAllLogisticsStatus(GetForSelectInput input)
        {
            return enumManagerFactory.EnumManager.GetAllOrderStatus<LogisticsStatus>(input);
        }


    }
}
