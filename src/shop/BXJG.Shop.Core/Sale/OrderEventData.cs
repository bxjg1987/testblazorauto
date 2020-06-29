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
    public class OrderPaidEventData<TUser, TArea, TDataDictionary> : EntityEventData<OrderEntity<TUser, TArea, TDataDictionary>>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        public OrderPaidEventData(OrderEntity<TUser, TArea, TDataDictionary> order) : base(order)
        { }
    }
    /// <summary>
    /// 订单发货事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderShipedEventData<TUser, TArea, TDataDictionary> : EntityEventData<OrderEntity<TUser, TArea, TDataDictionary>>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        public OrderShipedEventData(OrderEntity<TUser, TArea, TDataDictionary> order) : base(order)
        { }
    }
    /// <summary>
    /// 订单签收事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderSignedEventData<TUser, TArea, TDataDictionary> : EntityEventData<OrderEntity<TUser, TArea, TDataDictionary>>
        where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
    {
        public OrderSignedEventData(OrderEntity<TUser, TArea, TDataDictionary> order) : base(order)
        { }
    }
}
