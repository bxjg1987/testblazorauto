using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Events.Bus;
using Abp.UI;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 商品上架信息ProductEntity中的信息可能发生变化，比如销售价，OrderItem会在顾客购买时记录下来购买时的售价 类似的字段有很多
     * 总额 总积分等 本可以查询时计算，但为将来查询更方便直接在保存信息时计算
     * 
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
    public class OrderItemEntity : Entity<long>
    {
        /// <summary>
        /// 订单明细数量改变时触发
        /// 设计时考虑是给OrderEntity用的
        /// </summary>
        public event Action<OrderItemEntity> QuitityChanged;
        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get; private set; }
        /// <summary>
        /// 关联的订单实体
        /// </summary>
        public virtual OrderEntity Order { get; private set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ProductId { get; private set; }
        ///// <summary>
        ///// 关联的商品上架信息
        ///// 按abp vNext建议，不要在一个聚合中关联另一个聚合根的东东，只关联外键就好了
        ///// </summary>
        //public virtual ProductEntity Product { get; private set; }
        /// <summary>
        /// 关联的skuId，可选
        /// </summary>
        public long? SkuId { get; private set; }
        ///// <summary>
        ///// 关联的sku
        /////// 按abp vNext建议，不要在一个聚合中关联另一个聚合根的东东，只关联外键就好了
        ///// </summary>
        //public virtual SkuEntity Sku { get; private set; }
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

        private decimal quantity;

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                //简单的业务判断，未作深入思考
                //业务明细始终可以被外界访问，也可能被领域服务调整，因此事件和业务判断应该写在此属性内部
                //if (Order.PaymentStatus != PaymentStatus.WaitingForPayment)
                //{
                //    throw new UserFriendlyException("未付款的订单才允许调整明细数量");
                //}
                var temp = quantity;
                quantity = value;
                if (temp == value)
                    return;
                Amount = quantity * Price;
                TotalIntegral = Convert.ToInt32(Integral * quantity);
                QuitityChanged?.Invoke(this);
            }
        }
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
        /// <summary>
        /// 此构造函数给efcore用的
        /// </summary>
        private OrderItemEntity() { }
        /// <summary>
        /// 实例化订单明细
        /// 如果automapper可以沟通过构造函数映射，这里的order空检测将带来问题，此时建议手动将dto转换为实体。
        /// </summary>
        /// <param name="order">关联的订单</param>
        /// <param name="productId">关联的商品Id</param>
        /// <param name="skuId">关联的skuId，可空</param>
        /// <param name="title">商品标题，必填</param>
        /// <param name="image">商品图片，可空</param>
        /// <param name="price">单价，必须大于等于0</param>
        /// <param name="integral">可得积分，必须大于等于0</param>
        /// <param name="quantity">数量，比如大于0</param>
        public OrderItemEntity(OrderEntity order,
                               long productId,
                               long? skuId,
                               string title,
                               string image,
                               decimal price,
                               int integral,
                               decimal quantity)
        {
            Order = order ?? throw new ArgumentNullException(nameof(order));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Image = image;
            OrderId = order.Id;
            ProductId = productId <= 0 ? throw new ArgumentException(nameof(productId)) : productId;
            SkuId = skuId;
            Price = price < 0 ? throw new ArgumentException(nameof(price)) : price;
            Integral = integral < 0 ? throw new ArgumentException(nameof(integral)) : integral;
            Quantity = quantity <= 0 ? throw new ArgumentException(nameof(quantity)) : quantity;//这个一定要单价、积分之后，以便触发计算
        }
    }
}