using Abp.Configuration;
using Abp.Domain.Repositories;
using BXJG.GeneralTree;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Administrative;
using ZLJ.Authorization.Users;

namespace ZLJ.Shop
{
    public class OrderManager : OrderManager<User, AdministrativeEntity, GeneralTreeEntity>
    {
        public OrderManager(IRepository<OrderEntity<User, AdministrativeEntity, GeneralTreeEntity>, long> repository, IRepository<CustomerEntity<User, AdministrativeEntity>, long> customerRepository, ISettingManager settingManager) : base(repository, customerRepository, settingManager)
        {
        }
    }
}
