using Abp.Configuration;
using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Administrative;
using ZLJ.Authorization.Users;

namespace ZLJ.Shop
{
    public class OrderManager : OrderManager<User, AdministrativeEntity>
    {
        public OrderManager(IRepository<OrderEntity<User, AdministrativeEntity>, long> repository, IRepository<CustomerEntity<User>, long> customerRepository, ISettingManager settingManager) : base(repository, customerRepository, settingManager)
        {
        }
    }
}
