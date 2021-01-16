using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Linq;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    public class OrderCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<OrderEntity>>, ITransientDependency
    {
        protected readonly IRepository<ShoppingCartEntity, long> repository;
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; } = NullAsyncQueryableExecuter.Instance;
        public OrderCreatingEventHandler(IRepository<ShoppingCartEntity, long> repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 订单建立成功时清空服务端购物车
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public virtual async Task HandleEventAsync(EntityCreatingEventData<OrderEntity> eventData)
        {
            var shoppintCart = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.Items).Where(c => c.CustomerId == eventData.Entity.CustomerId));
            shoppintCart.ClearItems();
        }
    }
}
