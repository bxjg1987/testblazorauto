using Abp.Domain.Repositories;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Administrative;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;

namespace ZLJ.Shop
{
    /// <summary>
    /// 
    /// </summary>
    public class BXJGCustomerOrderAppService
        : CustomerOrderAppService<Tenant, User, Role, TenantManager, UserManager, AdministrativeEntity>
    {
        public BXJGCustomerOrderAppService(
            IRepository<OrderEntity<User, AdministrativeEntity>, long> repository,
            OrderManager<User, AdministrativeEntity> orderManager,
            CustomerManager<User, AdministrativeEntity> customerManager,
            IRepository<AdministrativeEntity, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository)
            : base(repository, orderManager, customerManager, generalTreeManager, itemRepository)
        { }
    }
}
