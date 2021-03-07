using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Timing;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.WorkOrder.WorkOrder
{
    public abstract class OrderBaseManager<TEntity> : DomainServiceBase where TEntity : OrderBaseEntity
    {
        /*
         * 虽然可以通过构造函数创建工单，但创建核心实体时最好使用领域服务，因为创建过程可能使用到其它服务
         * 需要经过多步骤配置后生成实体的可以考虑Builder模式
         */

        protected readonly IRepository<TEntity, long> repository;

        protected OrderBaseManager(IRepository<TEntity, long> repository)
        {
            this.repository = repository;
        }

        public async Task<TEntity> CreateAsync(long categoryId,
                                               UrgencyDegree urgencyDegree,
                                               string title,
                                               string description = default,
                                               DateTimeOffset? estimatedExecutionTime = default,
                                               DateTimeOffset? estimatedCompletionTime = default,
                                               string extendedField1 = default,
                                               string extendedField2 = default,
                                               string extendedField3 = default,
                                               string extendedField4 = default,
                                               string extendedField5 = default)
        {
            //其它逻辑，暂时忽略
            var entity = Create(categoryId,
                                urgencyDegree, title,
                                Clock.Now,
                                description,
                                estimatedExecutionTime,
                                estimatedCompletionTime,
                                extendedField1,
                                extendedField2,
                                extendedField3,
                                extendedField4,
                                extendedField5);
            //其它逻辑，暂时忽略
            return entity;
        }
        /// <summary>
        /// 子类重写，调用构造函数new一个工单
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="urgencyDegree"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        /// <param name="description"></param>
        /// <param name="estimatedExecutionTime"></param>
        /// <param name="estimatedCompletionTime"></param>
        /// <param name="extendedField1"></param>
        /// <param name="extendedField2"></param>
        /// <param name="extendedField3"></param>
        /// <param name="extendedField4"></param>
        /// <param name="extendedField5"></param>
        /// <returns></returns>
        protected abstract TEntity Create(long categoryId,
                                          UrgencyDegree urgencyDegree,
                                          string title,
                                          DateTimeOffset time,
                                          string description = default,
                                          DateTimeOffset? estimatedExecutionTime = default,
                                          DateTimeOffset? estimatedCompletionTime = default,
                                          string extendedField1 = default,
                                          string extendedField2 = default,
                                          string extendedField3 = default,
                                          string extendedField4 = default,
                                          string extendedField5 = default);

        public virtual Task DeleteAsync(TEntity entity)
        {
            if (entity.Status != Status.ToBeConfirmed)
                throw new UserFriendlyException("此状态的工单不允许删除！");

            return repository.DeleteAsync(entity);
        }
    }

    public class OrderManager : OrderBaseManager<OrderEntity>
    {
        public OrderManager(IRepository<OrderEntity, long> repository) : base(repository)
        {
        }

        protected override OrderEntity Create(long categoryId,
                                              UrgencyDegree urgencyDegree,
                                              string title,
                                              DateTimeOffset time,
                                              string description = null,
                                              DateTimeOffset? estimatedExecutionTime = null,
                                              DateTimeOffset? estimatedCompletionTime = null,
                                              string extendedField1 = null,
                                              string extendedField2 = null,
                                              string extendedField3 = null,
                                              string extendedField4 = null,
                                              string extendedField5 = null)
        {
            return new OrderEntity(categoryId,
                                   urgencyDegree,
                                   title,
                                   time,
                                   description,
                                   estimatedExecutionTime,
                                   estimatedCompletionTime,
                                   extendedField1,
                                   extendedField2,
                                   extendedField3,
                                   extendedField4,
                                   extendedField5);
        }
    }
}
