using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;

namespace BXJG.Shop.ShoppingCart
{
    /*
     * 可以在顾客首次将商品加入购物车时建立顾客的购物车，以后做此操作时先检查购物车是否存在，若存在则直接将商品加入购物车
     * 若这样做，每次将商品加入购物车都会做一次判断，比较浪费
     * 所以直接在顾客建立时为他建立购物车，将来将商品加入购物车时就不需要判断了，但这样那些从未使用购物车的顾客将浪费数据库空间，这是一个以空间换时间的问题
     * 
     * 我们可以在顾客的Create中的代码里来建立购物车，但这样会破坏建立顾客的简单逻辑，所以这里采用事件的方式更合理
     * 需要测试是否在同一个事务中
     */

    public class ShoppingCartManager : DomainServiceBase, IAsyncEventHandler<EntityCreatedEventData<CustomerEntity>>
    {
        private readonly IRepository<ShoppingCartEntity, long> repository;
        ///private readonly OrderManager orderManager;

        public ShoppingCartManager(IRepository<ShoppingCartEntity, long> repository/*, OrderManager orderManager*/)
        {
            this.repository = repository;
            //this.orderManager = orderManager;
        }
        /// <summary>
        /// 通过顾客新增事件建立购物车
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public Task HandleEventAsync(EntityCreatedEventData<CustomerEntity> eventData)
        {
            var shoppingCart = new ShoppingCartEntity
            {
                CustomerId = eventData.Entity.Id
            };
            return repository.InsertAsync(shoppingCart);
        }
        ///// <summary>
        ///// 购物车计算，将根据购物车生成新的订单，单这个订单并未存储到数据库中，且购物车也不会清空
        ///// 而是等到真正产生订单并保存到数据库后才会清空购物车
        ///// </summary>
        ///// <param name="shoppingCart"></param>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public Task<OrderEntity> BuildOrder(ShoppingCartEntity shoppingCart)
        //{
        //    var order = new OrderEntity { 
             
        //    };
        //}
    }
}
