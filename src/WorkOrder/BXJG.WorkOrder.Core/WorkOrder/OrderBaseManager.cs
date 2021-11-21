using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Timing;
using Abp.UI;
using BXJG.WorkOrder.WorkOrderCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public class WorkOrderCreateDtoBase
    {
        public long? CategoryId { get; set; }
        public UrgencyDegree? UrgencyDegree { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string EmployeeId { get; set; }
        public DateTimeOffset? Time { get; set; }
        public DateTimeOffset? EstimatedExecutionTime { get; set; }
        public DateTimeOffset? EstimatedCompletionTime { get; set; }
    }

    public class WorkOrderCreateDto : WorkOrderCreateDtoBase
    {
        public string ExtendedField1 { get; set; }
        public string ExtendedField2 { get; set; }
        public string ExtendedField3 { get; set; }
        public string ExtendedField4 { get; set; }
        public string ExtendedField5 { get; set; }
    }

    public abstract class OrderBaseManager<TEntity> : DomainServiceBase where TEntity : OrderBaseEntity
    {
        protected readonly IRepository<TEntity, long> repository;
        //protected readonly DefaultClsManager defaultClsManager;
        protected readonly string workOrderType;
        protected readonly IRepository<CategoryEntity, long> clsRepository;
        protected readonly CategoryManager clsManager;
        protected readonly OrderNoGenerator orderNoGenerator;


        protected OrderBaseManager(IRepository<TEntity, long> repository, IRepository<CategoryEntity, long> clsRepository, CategoryManager clsManager, string workOrderType, OrderNoGenerator orderNoGenerator)
        {
            this.repository = repository;
            this.workOrderType = workOrderType;
            this.clsRepository = clsRepository;
            this.clsManager = clsManager;
            this.orderNoGenerator = orderNoGenerator;
        }
        //分类 紧急程度可以定义参数默认值，进一步获取设置系统的默认值
        //工单的创建场景有：后台管理员创建、客户提交、某些事件如销售订单产生时自动创建，这些场景通常对应应用层方法或事件处理程序，它们都调用此方法

        public virtual async Task<TEntity> CreateAsync(WorkOrderCreateDtoBase dto)
        {
            //其它逻辑，暂时忽略
            if (!dto.CategoryId.HasValue)
            {
                dto.CategoryId = (await clsRepository.GetDefaultAsync(workOrderType)).Id;
            }
            else
                dto.CategoryId = dto.CategoryId;
            if (!dto.Time.HasValue)
                dto.Time = Clock.Now;
            if (!dto.UrgencyDegree.HasValue)
                dto.UrgencyDegree = OrderBaseEntity.DefaultUrgencyDegree;
            var entity = await Create(dto);
            entity.Id = orderNoGenerator.NewLong();
            await repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();//保存以更新id为自增id
            return entity;
        }

        protected abstract ValueTask< TEntity> Create(WorkOrderCreateDtoBase dto);

        public virtual Task DeleteAsync(TEntity entity)
        {
            if (entity.Status != Status.ToBeConfirmed)
                throw new UserFriendlyException("此状态的工单不允许删除！");

            return repository.DeleteAsync(entity);
        }
    }

    public class OrderManager : OrderBaseManager<OrderEntity>
    {
        public OrderManager(IRepository<OrderEntity, long> repository, 
                            IRepository<CategoryEntity, long> clsRepository,
                            CategoryManager clsManager,OrderNoGenerator orderNoGenerator) : base(repository,
                                                               clsRepository,
                                                               clsManager, 
                                                               CoreConsts.DefaultWorkOrderTypeName, orderNoGenerator)
        {
        }

        protected override ValueTask<OrderEntity> Create(WorkOrderCreateDtoBase input)
        {
            var dto = input as WorkOrderCreateDto;
            var entity = new OrderEntity(dto.Time.Value,
                                   dto.CategoryId.Value,
                                   dto.Title,
                                   dto.Description,
                                   dto.UrgencyDegree.Value,
                                   dto.EmployeeId,
                                   dto.EstimatedExecutionTime,
                                   dto.EstimatedCompletionTime,
                                   dto.ExtendedField1,
                                   dto.ExtendedField2,
                                   dto.ExtendedField3,
                                   dto.ExtendedField4,
                                   dto.ExtendedField5);
            return ValueTask.FromResult(entity);    
        }
    }
}
