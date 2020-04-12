using Abp.Authorization.Users;
using Abp.Domain.Entities;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 商品上架信息ItemEntity中的信息可能发生变化，比如销售价，OrderItem会在顾客购买时记录下来购买时的售价 类似的字段有很多
     * 总额 总积分等 本可以查询时计算，但为将来查询更方便直接在保存信息时计算
     * 由于涉及到钱，最后做下并发控制，虽然订单明细不属于聚合根，订单才是聚合根，所以并发控制应该放订单上
     * 但是将来可能不遵循ddd，而直接操作订单明细，因此在订单明细上也加个并发控制字段
     * 原本应该只给关键字段，如数量、金额之类的的字段加并发控制，但是比较麻烦，现在就直接使用rowVersion来做吧
     * 
     * 恶心的泛型
     */

    /// <summary>
    /// 订单中的产品信息
    /// </summary>
    public class OrderItemEntity<TUser> : Entity
        where TUser:AbpUserBase
    {
        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 关联的订单实体
        /// </summary>
        public virtual OrderEntity<TUser> Order { get; set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ItemId { get; set; }
        /// <summary>
        /// 关联的商品上架信息
        /// </summary>
        public virtual ItemEntity Item { get; set; }
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

        //以下汇总信息是方便将来统计查询

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get; set; }

        /// <summary>
        /// 乐观并发控制标记
        /// https://docs.microsoft.com/zh-cn/ef/core/modeling/concurrency?tabs=data-annotations
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
