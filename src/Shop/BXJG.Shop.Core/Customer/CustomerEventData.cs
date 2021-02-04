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
        /// <summary>
        /// 获取原始积分值
        /// </summary>
        public long OriginalValue { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customer">顾客实体</param>
        /// <param name="originalValue">原始值</param>
        public CustomerIntegralChangedEventData(CustomerEntity customer,long originalValue) : base(customer)
        {
            this.OriginalValue = originalValue;
        }
    }
}
