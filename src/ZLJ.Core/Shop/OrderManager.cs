using Abp.Configuration;
using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Users;

namespace ZLJ.Shop
{
    public class OrderManager : OrderManager<User>
    {
        public OrderManager(IRepository<OrderEntity<User>, long> repository, IRepository<CustomerEntity<User>, long> customerRepository, ISettingManager settingManager) : base(repository, customerRepository, settingManager)
        {
        }
    }
}
