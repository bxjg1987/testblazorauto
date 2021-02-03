using Abp;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.UI;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 订单实体类
    /// </summary>
    public class OrderEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        #region 订单信息
        /// <summary>
        /// 关联的顾客的Id
        /// </summary>
        public long CustomerId { get; private set; }
        ///// <summary>
        ///// 关联的顾客的实体
        /////// 按abp vNext建议，不要在一个聚合中关联另一个聚合根的东东，只关联外键就好了
        ///// </summary>
        //public virtual CustomerEntity Customer { get; private set; }
        /// <summary>
        /// 订单号
        /// 系统里面的关联、处理尽量通过Id属性来，这里的订单号是业务上的概念
        /// </summary>
        public string OrderNo { get; private set; }
        /// <summary>
        /// 下单时间
        /// 虽然父类已经有了CreateDate，但是类型为DateTime。况且CreateDate是表示这条信息的创建时间，OrderTime是下单业务发生的时间，这是两个不一样的概念
        /// 最好用abp的Clock
        /// </summary>
        public DateTimeOffset OrderTime { get; private set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; private set; }
        /// <summary>
        /// 顾客下单时填写的备注
        /// </summary>
        public string CustomerRemark { get; set; }
        /// <summary>
        /// 订单商品明细
        /// </summary>
        private List<OrderItemEntity> items;
        /// <summary>
        /// 订单商品明细
        /// </summary>
        public virtual IReadOnlyList<OrderItemEntity> Items => items == default ? new List<OrderItemEntity>() : items.AsReadOnly();
        /// <summary>
        /// 商品小计
        /// 一个订单的中的多个商品价格相加的价格，但是商品列表可能随时在变动，所以这个属性只代表数据库中的商品小计字段的值
        /// 可以通过对应的方法来根据商品列表计算得到商品小计
        /// </summary>
        public decimal MerchandiseSubtotal { get; private set; }
        ///// <summary>
        ///// 配送费
        ///// </summary>
        //public decimal DistributionFee { get; set; }

        ///// <summary>
        ///// 是否需要开票
        ///// </summary>
        //public InvoiceType InvoiceType { get; set; }
        ///// <summary>
        ///// 发票抬头
        ///// </summary>
        //public string InvoiceTitle { get; set; }
        ///// <summary>
        ///// 税号
        ///// </summary>
        //public string TaxId { get; set; }
        ///// <summary>
        ///// 发票税金
        ///// </summary>
        //public decimal InvoiceTax { get; set; }

        /// <summary>
        /// 可得积分
        /// </summary>
        public long Integral { get; private set; }
        /// <summary>
        /// 乐观并发
        /// </summary>
        public byte[] RowVersion { get; private set; }
        #endregion

        #region 支付信息
        /*
         * 订单支付时一并设置支付相关属性，不允许单个修改
         */
        ///// <summary>
        ///// 支付方式
        ///// 它是另一个领域根，不用导航属性关联
        ///// </summary>
        //public virtual GeneralTreeEntity PaymentMethod { get; private set; }
        /// <summary>
        /// 支付方式Id
        /// 未支付时 就不存在支付方式，因此可空
        /// </summary>
        public long? PaymentMethodId { get; private set; }
        /// <summary>
        /// 付款金额
        /// 顾客最终支付金额
        /// </summary>
        public decimal PaymentAmount { get; private set; }
        /// <summary>
        /// 支付状态
        /// 某些场景下，并不是顾客下单就可以付款，而是需要后台审核后才能付款
        /// 因此使用? 表示此时订单处于不可付款状态，也就是没有付款状态
        /// </summary>
        public PaymentStatus? PaymentStatus { get; private set; }
        #endregion

        #region 物流配送
        /*
         * 配送信息跟订单当前的各种状态有关，不允许胡乱修改
         * 比如订单已发货，要修改收货人信息时需要做别的业务处理
         * 
         * 单独提供修改收货人信息的方法
         * 单独提供配送的方法
         */
        ///// <summary>
        ///// 送货地址所属区域，另一个聚合中的类，不要直接使用导航属性
        ///// </summary>
        //public virtual AdministrativeEntity Area { get; private set; }
        /// <summary>
        /// 送货地址所属区域Id
        /// </summary>
        public long AreaId { get; private set; }
        /// <summary>
        /// 收货人
        /// 不一定就是下单人
        /// </summary>
        public string Consignee { get; private set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        public string ConsigneePhoneNumber { get; private set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceivingAddress { get; private set; }
        ///// <summary>
        ///// 已弃用，京东没有这个字段
        ///// </summary>
        //public string ZipCode { get; set; }
        ///// <summary>
        ///// 配送方式，另一个聚合中的类，不要直接使用导航属性
        ///// </summary>
        //public virtual GeneralTreeEntity DistributionMethod { get; private set; }
        /// <summary>
        /// 配送方式
        /// 刚创建订单时配送方式尚未确定
        /// </summary>
        public long? DistributionMethodId { get; private set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { get; private set; }
        /// <summary>
        /// 物流状态
        /// 刚创建订单时没有物流状态，因此加个?
        /// </summary>
        public LogisticsStatus? LogisticsStatus { get; private set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 此构造函数给efcore用的
        /// </summary>
        private OrderEntity() { }
        /// <summary>
        /// 实例化订单
        /// </summary>
        /// <param name="customerId">顾客id</param>
        /// <param name="orderNo">单号</param>
        /// <param name="orderTime">下单时间</param>
        /// <param name="areaId">收货人所属区域id</param>
        /// <param name="consignee">收货人姓名</param>
        /// <param name="consigneePhoneNumber">收货人联系电话</param>
        /// <param name="receivingAddress">收货地址</param>
        /// <param name="items">商品明细</param>
        /// <param name="customerRemark">顾客下单时的备注</param>
        public OrderEntity(long customerId,
                           string orderNo,
                           DateTimeOffset orderTime,
                           long areaId,
                           string consignee,
                           string consigneePhoneNumber,
                           string receivingAddress,
                           string customerRemark = default,
                           IList<OrderItemEntity> items = default)
        {
            CustomerId = customerId <= 0 ? throw new ArgumentOutOfRangeException(nameof(customerId)) : customerId;
            OrderNo = Check.NotNullOrWhiteSpace(orderNo, nameof(orderNo));
            OrderTime = orderTime == default ? throw new ArgumentException(nameof(orderTime)) : orderTime;
            UpdateLogisticsInfo(areaId, consignee, consigneePhoneNumber, receivingAddress);
            if (items == default)
            {
                this.items = new List<OrderItemEntity>();
            }
            else
            {
                this.items = items.ToList();
            }
            RegisterItemsEvent();
            CalculateMerchandiseSubtotal();
            CustomerRemark = customerRemark;
            Status = OrderStatus.Created;
        }
        #endregion

        #region 方法
        public void AddItem(OrderItemEntity item)
        {
            var oldItem = items.SingleOrDefault(c => c.ProductId == item.ProductId && c.SkuId == item.SkuId);
            if (oldItem != null)
            {
                oldItem.Quantity += item.Quantity;
                return;
            }

            RegisterItemEvent(item);
            items.Add(item);
            CalculateMerchandiseSubtotal();
            //DomainEvents.Add(new AddItemEventData(this, item));
        }
        /// <summary>
        /// 支付
        /// 目前只考虑全额支付
        /// </summary>
        /// <param name="payMethodId">支付方式Id</param>
        /// <returns></returns>
        public void Pay(long payMethodId)
        {
            //简单的业务判断，未作深入思考
            if (Status != OrderStatus.Created)
            {
                throw new UserFriendlyException("此状态的订单不允许支付操作");
            }
            PaymentMethodId = payMethodId;
            PaymentAmount = MerchandiseSubtotal;
            PaymentStatus = Sale.PaymentStatus.Paid;
            Status = OrderStatus.Processing;
            LogisticsStatus = Sale.LogisticsStatus.WaitShip;
            DomainEvents.Add(new OrderPaidEventData(this));
        }
        /// <summary>
        /// 设置物流信息
        /// 设置物流信息时使用判断物流状态的，这是业务规则，因此单独提供物流状态修改的方法
        /// </summary>
        /// <param name="areaId">收货人所属区域id</param>
        /// <param name="consignee">收货人姓名</param>
        /// <param name="consigneePhoneNumber">收货人联系电话</param>
        /// <param name="receivingAddress">收货地址</param>
        public void UpdateLogisticsInfo(long areaId, string consignee, string consigneePhoneNumber, string receivingAddress)
        {
            //真实业务中肯定有已发货后修改收货人信息的情况，那时应该允许修改并触发相应事件，目前未实现这个规则，将来再重构
            //按ddd的方式应该抛出对应的异常类，这里偷个懒，业务逻辑异常统一抛出UserFriendlyException
            if (LogisticsStatus.HasValue && (int)LogisticsStatus.Value > 0)
            {
                throw new UserFriendlyException("当前物流状态不允许修改收货人信息");
            }

            AreaId = areaId <= 0 ? throw new ArgumentOutOfRangeException(nameof(areaId)) : areaId;
            Consignee = Check.NotNullOrWhiteSpace(consignee, nameof(consignee));
            ConsigneePhoneNumber = Check.NotNullOrWhiteSpace(consigneePhoneNumber, nameof(consigneePhoneNumber));
            ReceivingAddress = Check.NotNullOrWhiteSpace(receivingAddress, nameof(receivingAddress));

            //触发收货人信息变更事件
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="shipmentMethod">发货方式|物流公司</param>
        /// <param name="logisticsNumber">物流单号，可能是类似虚拟商品，不需要物理发货，所以可为空</param>
        /// <returns></returns>
        public void ShipmentAsync(long shipmentMethod, string logisticsNumber = default)
        {
            //简单的业务判断，未作深入思考
            if (Status != OrderStatus.Processing)
            {
                throw new UserFriendlyException("未付款，不允许发货！");
            }

            DistributionMethodId = shipmentMethod;
            LogisticsStatus = Sale.LogisticsStatus.Shipped;
            LogisticsNumber = logisticsNumber;
            DomainEvents.Add(new OrderShipedEventData(this));
        }
        /// <summary>
        /// 签收
        /// </summary>
        /// <returns></returns>
        public void SignAsync()
        {
            //简单的业务判断，未作深入思考
            if (Status != OrderStatus.Processing || LogisticsStatus != Sale.LogisticsStatus.Shipped)
            {
                throw new UserFriendlyException("此状态的订单不允许做签收处理！");
            }
            LogisticsStatus = Sale.LogisticsStatus.Signed;
            Status = OrderStatus.Completed;
            DomainEvents.Add(new OrderSignedEventData(this));
        }
        /// <summary>
        /// 注册订单明细的事件
        /// </summary>
        public void RegisterItemsEvent()
        {
            foreach (var item in items)
            {
                RegisterItemEvent(item);
            }
        }
        /// <summary>
        /// 注册订单明细的事件
        /// </summary>
        /// <param name="item"></param>
        private void RegisterItemEvent(OrderItemEntity item)
        {
            item.QuitityChanged -= Item_QuitityChanged;
            item.QuitityChanged += Item_QuitityChanged;
        }
        /// <summary>
        /// 订单明细数量变化时的事件处理
        /// </summary>
        /// <param name="arg1"></param>
        private void Item_QuitityChanged(OrderItemEntity arg1)
        {
            CalculateMerchandiseSubtotal();
            DomainEvents.Add(new OrderItemQuantityChanged(arg1));
        }
        /// <summary>
        /// 计算商品小计、积分总额
        /// </summary>
        private void CalculateMerchandiseSubtotal()
        {
            MerchandiseSubtotal = Items.Sum(c => c.Amount);
            Integral = Items.Sum(c => c.TotalIntegral);
        }
        #endregion
    }
}
