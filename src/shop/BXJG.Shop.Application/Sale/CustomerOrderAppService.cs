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
    /// <typeparam name="TArea"></typeparam>
    public class CustomerOrderAppService<TTenant, TUser, TRole, TTenantManager, TUserManager, TArea>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, ICustomerOrderAppService
        where TUser : AbpUser<TUser>, new()
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
        where TArea : GeneralTreeEntity<TArea>, IShopAdministrative
    {
        private readonly IRepository<OrderEntity<TUser, TArea>, long> repository;
        private readonly OrderManager<TUser, TArea> orderManager;
        private readonly CustomerManager<TUser, TArea> customerManager;
        private readonly IRepository<TArea, long> generalTreeManager;
        private readonly IRepository<ItemEntity, long> itemRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="orderManager"></param>
        /// <param name="customerManager"></param>
        /// <param name="generalTreeManager"></param>
        /// <param name="itemRepository"></param>
        public CustomerOrderAppService(
            IRepository<OrderEntity<TUser, TArea>, long> repository,
            OrderManager<TUser, TArea> orderManager,
            CustomerManager<TUser, TArea> customerManager,
            IRepository<TArea, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository)
        {
            this.repository = repository;
            this.orderManager = orderManager;
            this.customerManager = customerManager;
            this.generalTreeManager = generalTreeManager;
            this.itemRepository = itemRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CustomerOrderDto> CreateAsync(CustomerOrderCreateDto input)
        {
            var customer = await customerManager.SingleByUserIdWithoutUserAsync(AbpSession.UserId.Value);
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
    }
}
