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

using BXJG.Shop.Customer;
using BXJG.WeChat.Payment;
using BXJG.GeneralTree;
using ZLJ.BaseInfo.Administrative;

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
            OrderManager,
            CustomerManager>
    {
        public BXJGShopOrderAppService(
            IRepository<CustomerEntity<User>, long> customerRepository,
            CustomerManager customerManager,
            IRepository<OrderEntity<User>, long> repository,
            OrderManager orderManager,
            IRepository<AdministrativeEntity, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository, WeChatPaymentService weChatPaymentService)
            : base(customerRepository, customerManager, repository, orderManager, generalTreeManager, itemRepository, weChatPaymentService)
        {
            //sd
        }
    }
}
