using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.Shop
{
    public class CustomerAppService : CustomerAppService<Tenant, User, Role, TenantManager, UserManager>
    {
        public CustomerAppService(IRepository<CustomerEntity<User>, long> repository) : base(repository)
        {
        }
    }
}
