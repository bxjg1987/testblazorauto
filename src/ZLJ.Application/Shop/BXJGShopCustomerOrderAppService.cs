using Abp.Domain.Repositories;
using BXJG.GeneralTree;
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
    public class BXJGShopCustomerOrderAppService : BXJGShopCustomerOrderAppService<
            Tenant,
            User,
            Role,
            TenantManager,
            UserManager,
            AdministrativeEntity,
            OrderManager,
            CustomerManager,
            GeneralTreeEntity>
    {
        public BXJGShopCustomerOrderAppService(
            IRepository<CustomerEntity<User, AdministrativeEntity>, long> customerRepository,
            CustomerManager customerManager,
            BXJGShopCustomerSession<User, AdministrativeEntity> customerSession,
            IRepository<OrderEntity<User, AdministrativeEntity, GeneralTreeEntity>, long> repository,
            OrderManager orderManager,
            IRepository<AdministrativeEntity, long> generalTreeManager, 
            IRepository<ItemEntity<GeneralTreeEntity>, long> itemRepository, WeChatPaymentService weChatPaymentService)
            : base(customerRepository, customerManager, customerSession, repository, orderManager, generalTreeManager, itemRepository, weChatPaymentService)
        {
            //sd
        }
    }
}
