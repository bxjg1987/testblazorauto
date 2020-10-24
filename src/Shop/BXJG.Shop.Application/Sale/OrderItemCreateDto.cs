using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前端顾客创建订单的商品明细
    /// </summary>
    public class OrderItemCreateDto
    {
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// skuId
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
    }
}
