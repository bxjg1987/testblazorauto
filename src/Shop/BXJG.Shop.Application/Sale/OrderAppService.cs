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
        protected readonly IRepository<GeneralTreeEntity, long> generalTreeRepository;
        //private readonly WeChatPaymentService wechatPaymentService;

        //public ICancellationTokenProvider CancellationToken { get; set; } = NullCancellationTokenProvider.Instance;

        public OrderAppService(IRepository<OrderEntity, long> repository,
                               OrderManager orderManager,
                               IRepository<AdministrativeEntity, long> administrativeRepository,
                               IRepository<ProductEntity, long> itemRepository,
                               IRepository<GeneralTreeEntity, long> generalTreeRepository)
        {
            this.repository = repository;
            this.orderManager = orderManager;
            this.administrativeRepository = administrativeRepository;
            this.itemRepository = itemRepository;
            this.generalTreeRepository = generalTreeRepository;
        }
        /// <summary>
        /// 根据条件查询订单列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<OrderDto>> GetAllAsync(GetAllOrderInput input)
        {
            var query = from c in repository.GetAll()
                        join qy1 in administrativeRepository.GetAll() on c.AreaId equals qy1.Id into dc2
                        from ar in dc2.DefaultIfEmpty()
                        join d in generalTreeRepository.GetAll() on c.DistributionMethodId equals d.Id into dc
                        from e in dc.DefaultIfEmpty()
                        join f in generalTreeRepository.GetAll() on c.PaymentMethodId equals f.Id into dc1
                        from g in dc1.DefaultIfEmpty()
                        select new { c, ar, e, g };

            query = query.WhereIf(input.OrderStartTime.HasValue, c => c.c.OrderTime >= input.OrderStartTime.Value)
                         .WhereIf(input.OrderEndTime.HasValue, c => c.c.OrderTime < input.OrderEndTime.Value)
                         .WhereIf(input.OrderStatus.HasValue, c => c.c.Status == input.OrderStatus.Value)
                         .WhereIf(input.PaymentStatus.HasValue, c => c.c.PaymentStatus == input.PaymentStatus.Value)
                         .WhereIf(input.LogisticsStatusStatus.HasValue, c => c.c.LogisticsStatus == input.LogisticsStatusStatus.Value)
                         .WhereIf(input.AreaId.HasValue, c => c.c.AreaId == input.AreaId.Value)
                         .WhereIf(input.PaymentMethodId.HasValue, c => c.c.PaymentMethodId == input.PaymentMethodId.Value)
                         .WhereIf(input.DistributionMethodId.HasValue, c => c.c.DistributionMethodId == input.DistributionMethodId.Value)
                         .WhereIf(!input.Keywords.IsNullOrEmpty(), c => c.c.OrderNo.Contains(input.Keywords) ||
                                                                        c.c.Consignee.Contains(input.Keywords) ||
                                                                        c.c.ConsigneePhoneNumber.Contains(input.Keywords) ||
                                                                        c.c.ReceivingAddress.Contains(input.Keywords) ||
                                                                        c.c.LogisticsNumber.Contains(input.Keywords));

            var count = await AsyncQueryableExecuter.CountAsync(query);
            var list = await AsyncQueryableExecuter.ToListAsync(query.OrderBy("c." + input.Sorting).PageBy(input));
            var dtos = list.Select(c => MapToDto(c.c, c.ar, c.g, c.e)).ToList();
            return new PagedResultDto<OrderDto>(count, dtos);
        }
        /// <summary>
        /// 根据内部Id查询订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<OrderDto> GetAsync(EntityDto<long> input)
        {
            var query = from c in repository.GetAllIncluding(c => c.Items)
                        join qy1 in administrativeRepository.GetAll() on c.AreaId equals qy1.Id into dc2
                        from ar in dc2.DefaultIfEmpty()
                        join d in generalTreeRepository.GetAll() on c.DistributionMethodId equals d.Id into dc
                        from e in dc.DefaultIfEmpty()
                        join f in generalTreeRepository.GetAll() on c.PaymentMethodId equals f.Id into dc1
                        from g in dc1.DefaultIfEmpty()
                        where c.Id == input.Id
                        select new { c, ar, e, g };

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(query);
            return MapToDto(entity.c, entity.ar, entity.g, entity.e);
        }
        /// <summary>
        /// 实体映射到dto
        /// </summary>
        /// <param name="c">收货地址中的区域</param>
        /// <param name="area"></param>
        /// <param name="g1">支付方式</param>
        /// <param name="g2">配送方式</param>
        /// <returns></returns>
        protected virtual OrderDto MapToDto(OrderEntity c, AdministrativeEntity area, GeneralTreeEntity g1 = default, GeneralTreeEntity g2 = default)
        {
            var dto = ObjectMapper.Map<OrderDto>(c);
            ObjectMapper.Map(area, dto);
            if (g1 != default)
                ObjectMapper.Map(g1, dto);
            if (g2 != default)
                ObjectMapper.Map(g2, dto);
            return dto;
        }
    }
}
