using BXJG.Shop.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale.Dto
{
    /// <summary>
    /// 顾客端和小程序端目前都使用此对象作为订单列表的显示模式
    /// 将来可能按后台管理员和前端顾客分开定义不同对象（比如顾客id、顾客名称，后台管理时需要，但前端顾客的订单列表不需要返回这些字段）
    /// </summary>
    public class OrderListDto
    {
        #region 订单信息
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 顾客Id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTimeOffset OrderTime { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// 获取订单状态的本地化文本
        /// </summary>
        public string StatusText
        {
            get
            {
                return Status.BXJGShopOrderStatus();
            }
        }
        /// <summary>
        /// 顾客下单时填写的备注
        /// </summary>
        public string CustomerRemark { get; set; }
        #endregion
        #region 支付信息
        /// <summary>
        /// 商品小计
        /// 一个订单的中的多个商品价格相加的价格，但是商品列表可能随时在变动，所以这个属性只代表数据库中的商品小计字段的值
        /// 可以通过对应的方法来根据商品列表计算得到商品小计
        /// </summary>
        public decimal MerchandiseSubtotal { get; set; }
        /// <summary>
        /// 可得积分
        /// </summary>
        public long Integral { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentMethodDisplayName { get; set; }
        /// <summary>
        /// 支付方式Id
        /// 未支付时 就不存在支付方式，因此可空
        /// </summary>
        public long? PaymentMethodId { get; set; }
        /// <summary>
        /// 付款金额
        /// 顾客最终支付金额
        /// </summary>
        public decimal PaymentAmount { get; set; }
        /// <summary>
        /// 支付状态
        /// 某些场景下，并不是顾客下单就可以付款，而是需要后台审核后才能付款
        /// 因此使用? 表示此时订单处于不可付款状态，也就是没有付款状态
        /// </summary>
        public PaymentStatus? PaymentStatus { get; set; }
        /// <summary>
        /// 支付状态本地化文本
        /// </summary>
        public string PaymentStatusText
        {
            get
            {
                if (PaymentStatus.HasValue)
                    return PaymentStatus.Value.BXJGShopPaymentStatus();
                return null;
            }
        }
        #endregion
        #region 物流信息
        /// <summary>
        /// 送货地址所属区域Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 送货地址所属区域的显示名
        /// </summary>
        public string AreaDisplayName { get; set; }
        /// <summary>
        /// 收货人 不一定就是下单人
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhoneNumber { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceivingAddress { get; set; }
        /// <summary>
        /// 配送方式名称
        /// </summary>
        public string DistributionMethodDisplayName { get; set; }
        /// <summary>
        /// 配送方式Id 刚创建订单时配送方式尚未确定，将为空
        /// </summary>
        public long? DistributionMethodId { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { get; set; }
        /// <summary>
        /// 物流状态
        /// 刚创建订单时没有物流状态，因此加个?
        /// </summary>
        public LogisticsStatus? LogisticsStatus { get; set; }
        /// <summary>
        /// 物流状态
        /// 刚创建订单时没有物流状态，因此加个?
        /// </summary>
        public string LogisticsStatusText
        {
            get
            {
                if (LogisticsStatus.HasValue)
                    return LogisticsStatus.Value.BXJGShopLogisticsStatus();
                return null;
            }
        }
        #endregion
        #region 订单明细
        /// <summary>
        /// 订单明细
        /// </summary>
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        #endregion
    }


}
