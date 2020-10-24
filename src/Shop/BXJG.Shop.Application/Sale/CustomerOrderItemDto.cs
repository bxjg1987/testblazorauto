using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 目前看后端对订单管理 和 前端顾客订单管理 查询的数据几乎一样
     * 但是毕竟是两种场景，因此先直接分开
     */

    /// <summary>
    /// 顾客查询订单时包含的订单明细
    /// </summary>
    public class CustomerOrderItemDto : EntityDto<long>
    {
        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 产品标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 产品图片
        /// 与商品上架信息不同，这里只需要单张图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get; set; }
    }
}
