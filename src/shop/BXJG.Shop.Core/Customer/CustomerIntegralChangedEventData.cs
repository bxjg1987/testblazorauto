using Abp.Authorization.Users;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using BXJG.Common;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客Customer积分变更(增加/减少)事件
    /// </summary>
    public class CustomerIntegralChangedEventData : EntityEventData<CustomerEntity>
    {
        public CustomerIntegralChangedEventData(CustomerEntity customer) : base(customer)
        {

        }
    }
}
