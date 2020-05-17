using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 后台管理获取订单列表时提供的输入参数
    /// </summary>
    public class GetAllOrderInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 订单开始时间
        /// </summary>
        public DateTimeOffset? OrderStartTime { get; set; }
        /// <summary>
        /// 订单结束时间
        /// </summary>
        public DateTimeOffset? OrderEndTime { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus? OrderStatus { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaymentStatus? PaymentStatus { get; set; }
        /// <summary>
        /// 物流状态
        /// </summary>
        public LogisticsStatus? LogisticsStatusStatus { get; set; }
        /// <summary>
        /// 收货地址所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 配送方式Id
        /// </summary>
        public long? DistributionMethodId { get; set; }
        /// <summary>
        /// 支付方式Id
        /// </summary>
        public long? PaymentMethodId { get; set; }
        /// <summary>
        /// 关键字 订单号
        /// </summary>
        public string Keywords { get; set; }

        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}
