using Abp.Application.Services;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Linq;
using Abp.MultiTenancy;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic;
using Abp.Extensions;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using BXJG.Shop.Common;
using BXJG.Shop.Customer;
using Abp.Threading;
using BXJG.Shop.Catalogue;
using BXJG.WeChat.Payment;

namespace BXJG.Shop.Sale
{
    public abstract class BXJGShopOrderAppService<TTenant, TUser, TRole, TTenantManager, TUserManager, TArea, TOrderManager, TCustomerManager>
        : BXJGShopAppServiceBase<TTenant, TUser, TRole, TTenantManager, TUserManager>, IBXJGShopOrderAppService
        where TUser : AbpUser<TUser>
        where TRole : AbpRole<TUser>, new()
        where TTenant : AbpTenant<TUser>
        where TTenantManager : AbpTenantManager<TTenant, TUser>
        where TUserManager : AbpUserManager<TRole, TUser>
        where TArea : GeneralTreeEntity<TArea>, IAdministrative
        where TOrderManager : OrderManager<TUser, TArea>
        where TCustomerManager : CustomerManager<TUser,TArea>
    {
        private readonly IRepository<OrderEntity<TUser, TArea>, long> repository;
        private readonly TOrderManager orderManager;
        private readonly IRepository<TArea, long> generalTreeManager;
        private readonly IRepository<ItemEntity, long> itemRepository;
        private readonly WeChatPaymentService weChatPaymentService;

        public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;

        public BXJGShopOrderAppService(
            IRepository<CustomerEntity<TUser,TArea>, long> customerRepository,
            TCustomerManager customerManager,
            IRepository<OrderEntity<TUser, TArea>, long> repository,
            TOrderManager orderManager,
            IRepository<TArea, long> generalTreeManager,
            IRepository<ItemEntity, long> itemRepository,
            WeChatPaymentService weChatPaymentService)
        {
            this.repository = repository;
            this.orderManager = orderManager;
            this.generalTreeManager = generalTreeManager;
            this.itemRepository = itemRepository;
            this.weChatPaymentService = weChatPaymentService;
        }
        /// <summary>
        /// 根据条件查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderDto>> GetAllAsync(GetAllOrderInput input)
        {
            var query = repository.GetAllIncluding(c => c.Customer.User, c => c.Area,c=>c.DistributionMethod, c => c.PaymentMethod)
                .WhereIf(input.OrderStartTime.HasValue,c=>c.OrderTime>=input.OrderStartTime.Value)
                .WhereIf(input.OrderEndTime.HasValue, c => c.OrderTime < input.OrderEndTime.Value)
                .WhereIf(input.OrderStatus.HasValue, c => c.Status==input.OrderStatus.Value)
                .WhereIf(input.PaymentStatus.HasValue, c => c.PaymentStatus == input.PaymentStatus.Value)
                .WhereIf(input.LogisticsStatusStatus.HasValue, c => c.LogisticsStatus == input.LogisticsStatusStatus.Value)
                .WhereIf(input.AreaId.HasValue, c => c.AreaId==input.AreaId.Value)
                .WhereIf(input.PaymentMethodId.HasValue, c => c.PaymentMethodId == input.PaymentMethodId.Value)
                .WhereIf(input.DistributionMethodId.HasValue, c => c.DistributionMethodId == input.DistributionMethodId.Value)
                .WhereIf(!input.Keywords.IsNullOrEmpty(), c =>
                    c.OrderNo.Contains(input.Keywords)
                    || c.Consignee.Contains(input.Keywords)
                    || c.ConsigneePhoneNumber.Contains(input.Keywords)
                    || c.ReceivingAddress.Contains(input.Keywords)
                    || c.LogisticsNumber.Contains(input.Keywords));

            var count = await query.CountAsync();
            var list = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            return new PagedResultDto<OrderDto>(count, ObjectMapper.Map<IReadOnlyList<OrderDto>>(list));
        }
    }
}
