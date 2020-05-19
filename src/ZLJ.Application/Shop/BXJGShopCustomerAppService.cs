using Abp.Domain.Repositories;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Administrative;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.BaseInfo;
using ZLJ.MultiTenancy;

namespace ZLJ.Shop
{
    /// <summary>
    /// 后台管理员对顾客信息进行管理的应用服务
    /// </summary>
    public class BXJGShopCustomerAppService : BXJGShopCustomerAppService<Tenant, User, Role, TenantManager, UserManager, AdministrativeEntity>
    {
        public BXJGShopCustomerAppService(IRepository<CustomerEntity<User, AdministrativeEntity>, long> repository) : base(repository)
        {
        }
    }
}
