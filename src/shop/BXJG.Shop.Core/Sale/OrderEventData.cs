using Abp.Authorization.Users;
using Abp.Events.Bus.Entities;
using BXJG.Common;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 订单支付事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderPaidEventData : EntityEventData<OrderEntity>
    {
        public OrderPaidEventData(OrderEntity order) : base(order)
        { }
    }
    /// <summary>
    /// 订单发货事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderShipedEventData : EntityEventData<OrderEntity>
    {
        public OrderShipedEventData(OrderEntity order) : base(order)
        { }
    }
    /// <summary>
    /// 订单签收事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderSignedEventData : EntityEventData<OrderEntity>
    {
        public OrderSignedEventData(OrderEntity order) : base(order)
        { }
    }
    /// <summary>
    /// 订单明细数量变更时的事件
    /// </summary>
    public class OrderItemQuantityChanged : EntityEventData<OrderItemEntity>
    {
        public OrderItemQuantityChanged(OrderItemEntity entity) : base(entity)
        {
        }
    }
}
