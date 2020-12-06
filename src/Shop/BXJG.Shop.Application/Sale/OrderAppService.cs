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
using BXJG.Shop.Customer;
using Abp.Threading;
using BXJG.Shop.Catalogue;
using BXJG.Common;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 后台管理员管理订单的应用服务
    /// </summary>
    public class OrderAppService : AppServiceBase, IOrderAppService
    {
        private readonly IRepository<OrderEntity, long> repository;
        private readonly OrderManager orderManager;
        private readonly IRepository<AdministrativeEntity, long> administrativeRepository;
        private readonly IRepository<ProductEntity, long> itemRepository;
      //  private readonly WeChatPaymentService wechatPaymentService;

        public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;

        public OrderAppService(IRepository<OrderEntity, long> repository,
                               OrderManager orderManager,
                               IRepository<AdministrativeEntity, long> administrativeRepository,
                               IRepository<ProductEntity, long> itemRepository)
        {
            this.repository = repository;
            this.orderManager = orderManager;
            this.administrativeRepository = administrativeRepository;
            this.itemRepository = itemRepository;
        }
        /// <summary>
        /// 根据条件查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderDto>> GetAllAsync(GetAllOrderInput input)
        {
            var query = repository.GetAllIncluding(c => c.Area, c => c.DistributionMethod, c => c.PaymentMethod)
                                  .WhereIf(input.OrderStartTime.HasValue, c => c.OrderTime >= input.OrderStartTime.Value)
                                  .WhereIf(input.OrderEndTime.HasValue, c => c.OrderTime < input.OrderEndTime.Value)
                                  .WhereIf(input.OrderStatus.HasValue, c => c.Status == input.OrderStatus.Value)
                                  .WhereIf(input.PaymentStatus.HasValue, c => c.PaymentStatus == input.PaymentStatus.Value)
                                  .WhereIf(input.LogisticsStatusStatus.HasValue, c => c.LogisticsStatus == input.LogisticsStatusStatus.Value)
                                  .WhereIf(input.AreaId.HasValue, c => c.AreaId == input.AreaId.Value)
                                  .WhereIf(input.PaymentMethodId.HasValue, c => c.PaymentMethodId == input.PaymentMethodId.Value)
                                  .WhereIf(input.DistributionMethodId.HasValue, c => c.DistributionMethodId == input.DistributionMethodId.Value)
                                  .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.OrderNo.Contains(input.Keywords) ||
                                                                                 c.Consignee.Contains(input.Keywords) ||
                                                                                 c.ConsigneePhoneNumber.Contains(input.Keywords) ||
                                                                                 c.ReceivingAddress.Contains(input.Keywords) ||
                                                                                 c.LogisticsNumber.Contains(input.Keywords));

            var count = await query.CountAsync();
            var list = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();
            return new PagedResultDto<OrderDto>(count, ObjectMapper.Map<IReadOnlyList<OrderDto>>(list));
        }

        public async Task<OrderDto> GetAsync(EntityDto<long> input)
        {
            var entity = await repository.GetAllIncluding(c => c.Area, c => c.DistributionMethod, c => c.PaymentMethod)
                .Include(c => c.Items).ThenInclude(c => c.Sku)
                .Where(c => c.Id == input.Id)
                .SingleAsync();
            return base.ObjectMapper.Map<OrderDto>(entity);
        }
    }
}
