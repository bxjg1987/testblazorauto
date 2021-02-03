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
                               IRepository<ProductEntity, long> itemRepository, IRepository<GeneralTreeEntity, long> generalTreeRepository)
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
        public async Task<PagedResultDto<OrderDto>> GetAllAsync(GetAllOrderInput input)
        {
            var query = repository.GetAll()
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

            var count = await AsyncQueryableExecuter.CountAsync(query);
            var list = await AsyncQueryableExecuter.ToListAsync(query.OrderBy(input.Sorting).PageBy(input));
            
            var ids = list.Select(c => c.AreaId);
            
            var ids1 = list.Where(c => c.DistributionMethodId.HasValue).Select(c => c.DistributionMethodId.Value);
            var ids2 = list.Where(c => c.PaymentMethodId.HasValue).Select(c => c.PaymentMethodId.Value);
            ids1 = ids1.Concat(ids2);

            var list2 = await AsyncQueryableExecuter.ToListAsync(administrativeRepository.GetAll().Where(c => ids.Contains(c.Id)));
            var list3 = await AsyncQueryableExecuter.ToListAsync(generalTreeRepository.GetAll().Where(c => ids1.Contains(c.Id)));

            var r = new PagedResultDto<OrderDto>(count, ObjectMapper.Map<IReadOnlyList<OrderDto>>(list));
            foreach (var item in r.Items)
            {
                item.AreaDisplayName = list2.Single(c => c.Id == item.AreaId).DisplayName;
                item.DistributionMethodDisplayName = list3.SingleOrDefault(c => c.Id == item.DistributionMethodId)?.DisplayName;
                item.PaymentMethodDisplayName = list3.SingleOrDefault(c => c.Id == item.PaymentMethodId)?.DisplayName;
            }
            return r;
        }
        /// <summary>
        /// 根据内部Id查询订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OrderDto> GetAsync(EntityDto<long> input)
        {
            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c=>c.Items).Where(c => c.Id == input.Id));
            var r = base.ObjectMapper.Map<OrderDto>(entity);

            var area = await administrativeRepository.GetAsync(entity.AreaId);
            r.AreaDisplayName = area.DisplayName;

            var ids = new List<long>();
            if (entity.DistributionMethodId.HasValue) {
                ids.Add(entity.DistributionMethodId.Value);          
            }
            if (entity.PaymentMethodId.HasValue)
            {
                ids.Add(entity.PaymentMethodId.Value);
            }
            var list2 = await AsyncQueryableExecuter.ToListAsync(generalTreeRepository.GetAll().Where(c => ids.Contains(c.Id)));
            r.DistributionMethodDisplayName = list2.SingleOrDefault(c => c.Id == entity.DistributionMethodId)?.DisplayName;
            r.PaymentMethodDisplayName = list2.SingleOrDefault(c => c.Id == entity.PaymentMethodId)?.DisplayName;
            return r;
        }
    }
}
