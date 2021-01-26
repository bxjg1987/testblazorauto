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
    //经过测试自带的实体事件要在ing事件中才是事务性的
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
            var shoppingCart = new ShoppingCartEntity(eventData.Entity.Id);
            return repository.InsertAsync(shoppingCart);
        }
    }

    public class AddItemEventHandler : IAsyncEventHandler<ChangeItemQuantityEventData>, ITransientDependency
    {
        public Task HandleEventAsync(ChangeItemQuantityEventData eventData)
        {
            //throw new NotImplementedException();
            //经过测试这里确实是在事务中
            return Task.CompletedTask;
        }
    }
}
