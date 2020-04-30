using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo;
using ZLJ.MultiTenancy;

namespace ZLJ.Shop
{
    public class BXJGShopCustomerAppService : BXJGShopCustomerAppService<Tenant, User, Role, TenantManager, UserManager, AdministrativeEntity>
    {
        public BXJGShopCustomerAppService(IRepository<CustomerEntity<User, AdministrativeEntity>, long> repository) : base(repository)
        {
        }
    }
}
