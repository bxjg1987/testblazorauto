using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus.Handlers;
using BXJG.Common;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 顾客支付订单成功后的事件处理器
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TArea"></typeparam>
    public class OrderPaidEventHandler : IEventHandler<OrderPaidEventData>
    {
        public void HandleEvent(OrderPaidEventData eventData)
        {
            eventData.Entity.Customer.ChangeIntegral(eventData.Entity.Integral);
        }
    }
}
