using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BXJG.Shop.Catalogue;
using BXJG.WeChat.Payment;
using Abp.Threading;
using BXJG.Common;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前台顾客对订单的操作接口
    /// 你需要在主程序中提供一个子类以指定泛型参数
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TTenantManager"></typeparam>
    /// <typeparam name="TUserManager"></typeparam>
    /// <typeparam name="TOrderManager"></typeparam>
    /// <typeparam name="TCustomerManager"></typeparam>
    /// <typeparam name="TDataDictionary"></typeparam>
    public abstract class BXJGShopCustomerOrderAppService<TTenant, TUser, TRole, TTenantManager, TUserManager, TOrderManager, TCustomerManager>
        : BXJGShopCustomerAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager, TCustomerManager>, IBXJGShopCustomerOrderAppService
        where TUser : AbpUser<TUser>, new()
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
        
        where TOrderManager : OrderManager<TUser>
        where TCustomerManager : CustomerManager<TUser>
        
    {
        private readonly IRepository<OrderEntity<TUser>, long> repository;
        private readonly TOrderManager orderManager;
        private readonly IRepository<AdministrativeEntity, long> generalTreeManager;
        private readonly IRepository<ItemEntity, long> itemRepository;
        private readonly WeChatPaymentService weChatPaymentService;

        public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;

        public BXJGShopCustomerOrderAppService(
            IRepository<CustomerEntity<TUser>, long> customerRepository,
            TCustomerManager customerManager,
            CustomerSession<TUser> customerSession,
            IRepository<OrderEntity<TUser>, long> repository,
            TOrderManager orderManager, 
            IRepository<AdministrativeEntity, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository,
            WeChatPaymentService weChatPaymentService)
            : base(customerRepository, customerManager, customerSession)
        {
            this.repository = repository;
            this.orderManager = orderManager;
            this.generalTreeManager = generalTreeManager;
            this.itemRepository = itemRepository;
            this.weChatPaymentService = weChatPaymentService;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CustomerOrderDto> CreateAsync(CustomerOrderCreateDto input)
        {
            var customer = await base.GetCurrentCustomerAsync();
            var area = await generalTreeManager.GetAsync(input.AreaId);
            var itemIds = input.Items.Select(c => c.ItemId);
            var items = await itemRepository.GetAllListAsync(c => itemIds.Contains(c.Id));
            var itemEntities = new List<OrderItemInput>();
            foreach (var item in input.Items)
            {
                var k = items.Single(c => c.Id == item.ItemId);
                itemEntities.Add(new OrderItemInput(k, item.Quantity));
            }
            var order = await orderManager.CreateAsync(
                customer,
                area,
                input.Consignee,
                input.ConsigneePhoneNumber,
                input.ReceivingAddress,
                input.CustomerRemark,
                itemEntities.ToArray());
            return ObjectMapper.Map<CustomerOrderDto>(order);
        }
        /// <summary>
        /// 前台顾客发起订单支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CustomerPaymentResult> PaymentAsync(CustomerPaymentInput input)
        {
            var customerId = await base.GetCurrentCustomerIdAsync();
            var order = await repository.GetAsync(input.OrderId);
            if (customerId != order.CustomerId)
                throw new ApplicationException();

            WeChatPaymentUnifyOrderResult wpor = await weChatPaymentService.PayAsync("ABP-商城", order.OrderNo, order.PaymentAmount);
            return new CustomerPaymentResult(wpor);
        }
    }
}
