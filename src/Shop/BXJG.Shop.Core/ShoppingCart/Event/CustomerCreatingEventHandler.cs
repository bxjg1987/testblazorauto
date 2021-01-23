using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    public class CustomerCreatingEventHandler : IAsyncEventHandler<EntityCreatingEventData<CustomerEntity>>,ITransientDependency
    {
        protected readonly IRepository<ShoppingCartEntity, long> repository;

        public CustomerCreatingEventHandler(IRepository<ShoppingCartEntity, long> repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// 通过顾客新增事件建立购物车
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public virtual Task HandleEventAsync(EntityCreatingEventData<CustomerEntity> eventData)
        {
            var shoppingCart = new ShoppingCartEntity(eventData.Entity);
            return repository.InsertAsync(shoppingCart);
        }

    }
}
