using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.Authorization.Roles;
using ZLJ.Authorization.Users;
using ZLJ.MultiTenancy;
using BXJG.Shop.Catalogue;
using Abp.Domain.Repositories;
using BXJG.Shop.Common;
using BXJG.Shop.Sale;
using ZLJ.Administrative;
using BXJG.Shop.Customer;
using BXJG.WeChat.Payment;

namespace ZLJ.Shop
{
    /// <summary>
    /// 后台管理员对订单操作的相关功能
    /// </summary>
    public class BXJGShopOrderAppService : BXJGShopOrderAppService<
            Tenant,
            User,
            Role,
            TenantManager,
            UserManager,
            AdministrativeEntity,
            OrderManager,
            CustomerManager>
    {
        public BXJGShopOrderAppService(
            IRepository<CustomerEntity<User, AdministrativeEntity>, long> customerRepository,
            CustomerManager customerManager,
            IRepository<OrderEntity<User, AdministrativeEntity>, long> repository,
            OrderManager orderManager,
            IRepository<AdministrativeEntity, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository, WeChatPaymentService weChatPaymentService)
            : base(customerRepository, customerManager, repository, orderManager, generalTreeManager, itemRepository, weChatPaymentService)
        {
            //sd
        }
    }
}
