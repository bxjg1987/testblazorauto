using Abp.Authorization.Users;
using Abp.Events.Bus.Entities;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
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
    public class OrderPaidEventData<TUser> : EntityEventData<OrderEntity<TUser>>
        where TUser : AbpUserBase
        
    {
        public OrderPaidEventData(OrderEntity<TUser> order) : base(order)
        { }
    }
    /// <summary>
    /// 订单发货事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderShipedEventData<TUser> : EntityEventData<OrderEntity<TUser>>
        where TUser : AbpUserBase
        
    {
        public OrderShipedEventData(OrderEntity<TUser> order) : base(order)
        { }
    }
    /// <summary>
    /// 订单签收事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderSignedEventData<TUser> : EntityEventData<OrderEntity<TUser>>
        where TUser : AbpUserBase
        
    {
        public OrderSignedEventData(OrderEntity<TUser> order) : base(order)
        { }
    }
}
