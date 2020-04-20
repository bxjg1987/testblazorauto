using Abp.Authorization.Users;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客Customer积分变更(增加/减少)事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class CustomerIntegralChangedEventData<TUser> : EntityEventData<CustomerEntity<TUser>>
         where TUser : AbpUserBase
    {
        public CustomerIntegralChangedEventData(CustomerEntity<TUser> customer):base(customer)
        {
            
        }
      
    }
}
