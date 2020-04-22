using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Events.Bus;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
     * 订单中的商品 数量 价格信息一旦购买后就说明成交了，不允许任意调整
     * 非要调整 应该定义单独的方法，调整需要有操作日志之类的，且这种日志必须使用数据库日志，与业务记录保持在同一个事务
     */

    /// <summary>
    /// 订单中的产品和数量信息，将来可能包含更多信息
    /// 目前所有属性私有化，将来根据业务需要 提供相应的业务方法
    /// </summary>
    public class OrderItemEntity<TUser> : Entity
        where TUser : AbpUserBase
    {
        public IEventBus EventBus
        {
            get
            {
                return Order.EventBus;
            }
        }

        private OrderItemEntity() { }//ef专用
        /// <summary>
        /// 创建一个顾客购买的产品信息
        /// 它是一张订单中的产品条码，包含商品信息 和 数量
        /// </summary>
        /// <param name="order">所属订单</param>
        /// <param name="item">关联的商品/上架信息</param>
        /// <param name="quantity">数量</param>
        /// <param name="title">商品标题，若不填则取商品信息的同名属性</param>
        /// <param name="image">商品封面图，若不填则取商品信息的第一张图片</param>
        /// <param name="price">商品单价，若不填则取商品信息的同名属性</param>
        /// <param name="integral">商品积分，若不填则取商品信息的同名属性</param>
        /// <param name="amount">商品金额，若不填则取商品信息单价*数量</param>
        /// <param name="totalIntegral">商品积分，若不填则取商品信息积分*数量</param>
        public OrderItemEntity(
            OrderEntity<TUser> order,
            ItemEntity item,
            decimal quantity,
            string title = "",
            string image = "",
            decimal? price = default,
            int? integral = default,
            decimal? amount = default,
            int? totalIntegral = default)
        {
            Order = order;
            Item = item;
            Quantity = quantity;
            //OrderId = order.Id; //新增时没有
            ItemId = item.Id;
            Title = title ?? item.Title;
            Image = image ?? item.GetImages().FirstOrDefault();
            Price = price ?? item.Price;
            Integral = integral ?? item.Integral;
            Amount = amount ?? CalculationAmount();
            TotalIntegral = totalIntegral ?? CalculationTotalIntegral();
        }

        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get; private set; }
        /// <summary>
        /// 关联的订单实体
        /// </summary>
        public virtual OrderEntity<TUser> Order { get; private set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ItemId { get; private set; }
        /// <summary>
        /// 关联的商品上架信息
        /// </summary>
        public virtual ItemEntity Item { get; private set; }
        /// <summary>
        /// 产品标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 产品图片
        /// 与商品上架信息不同，这里只需要单张图片
        /// </summary>
        public string Image { get; private set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; private set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; private set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; private set; }

        //以下冗余字段 而非每次计算，方便统计

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get; private set; }
        /// <summary>
        /// 乐观并发控制标记
        /// https://docs.microsoft.com/zh-cn/ef/core/modeling/concurrency?tabs=data-annotations
        /// </summary>
        public byte[] RowVersion { get; private set; }

        #region 方法
        /// <summary>
        /// 计算总金额 单价 * 数量
        /// </summary>
        /// <returns></returns>
        public decimal CalculationAmount()
        {
            return Price * Quantity;
        }
        /// <summary>
        /// 计算总积分 积分 * 数量
        /// </summary>
        /// <returns></returns>
        public int CalculationTotalIntegral()
        {
            return Convert.ToInt32(Integral * Quantity);
        }
        #endregion
    }
}
