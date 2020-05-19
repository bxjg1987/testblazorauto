using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Administrative;
using ZLJ.Authorization.Users;

namespace ZLJ.Shop
{
    public class CustomerManager : CustomerManager<User, AdministrativeEntity>
    {
        public CustomerManager(IRepository<CustomerEntity<User, AdministrativeEntity>, long> repository) : base(repository)
        {
        }
    }
}
