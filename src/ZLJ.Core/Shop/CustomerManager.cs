using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Users;

namespace ZLJ.Shop
{
    public class CustomerManager : CustomerManager<User>
    {
        public CustomerManager(IRepository<CustomerEntity<User>, long> repository) : base(repository)
        {
        }
    }
}
