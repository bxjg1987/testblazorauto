using Abp.Authorization.Users;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Customer
{
    /// <summary>
    /// 顾客Customer积分变更(增加/减少)事件
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class CustomerIntegralChangedEventData<TUser,TArea> : EntityEventData<CustomerEntity<TUser,TArea>>
         where TUser : AbpUserBase
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        public CustomerIntegralChangedEventData(CustomerEntity<TUser,TArea> customer):base(customer)
        {
            
        }
      
    }
}
