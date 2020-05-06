using Abp.Domain.Repositories;
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using BXJG.WeChat.Payment;
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
    public class BXJGCustomerOrderAppService : CustomerOrderAppService<
            Tenant,
            User,
            Role,
            TenantManager,
            UserManager,
            AdministrativeEntity,
            OrderManager<User, AdministrativeEntity>,
            CustomerManager<User>>
    {
        public BXJGCustomerOrderAppService(
            IRepository<CustomerEntity<User>, long> customerRepository,
            CustomerManager<User> customerManager,
            BXJGShopCustomerSession<User> customerSession,
            IRepository<OrderEntity<User, AdministrativeEntity>, long> repository,
            OrderManager<User, AdministrativeEntity> orderManager,
            IRepository<AdministrativeEntity, long> generalTreeManager, 
            IRepository<ItemEntity, long> itemRepository, WeChatPaymentService weChatPaymentService)
            : base(customerRepository, customerManager, customerSession, repository, orderManager, generalTreeManager, itemRepository, weChatPaymentService)
        {
            //sd
        }
    }
}
