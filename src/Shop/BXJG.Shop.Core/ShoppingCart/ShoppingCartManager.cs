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
using BXJG.Shop.Catalogue;
using BXJG.Shop.Customer;
using BXJG.Shop.Sale;

namespace BXJG.Shop.ShoppingCart
{
    /*
     * 可以在顾客首次将商品加入购物车时建立顾客的购物车，以后做此操作时先检查购物车是否存在，若存在则直接将商品加入购物车
     * 若这样做，每次将商品加入购物车都会做一次判断，比较浪费
     * 所以直接在顾客建立时为他建立购物车，将来将商品加入购物车时就不需要判断了，但这样那些从未使用购物车的顾客将浪费数据库空间，这是一个以空间换时间的问题
     * 
     * 我们可以在顾客的Create中的代码里来建立购物车，但这样会破坏建立顾客的简单逻辑，切购物车也不是购物系统必须的功能，它只是辅助功能，所以这里采用事件的方式更合理
     * 需要测试是否在同一个事务中
     */

    public class ShoppingCartManager : DomainServiceBase
    {
        protected readonly IRepository<ShoppingCartEntity, long> repository;
        //protected readonly Lazy<OrderManager> orderManager;
        protected readonly IRepository<ProductEntity, long> productRepository;
        public ShoppingCartManager(IRepository<ShoppingCartEntity, long> repository/*, Lazy<OrderManager> orderManager*/, IRepository<ProductEntity, long> productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            //this.orderManager = orderManager;
        }
        /// <summary>
        /// 获取当前顾客的购物车
        /// <br />包含关联的顾客和明细(包含明细关联的商品、sku)
        /// </summary>
        /// <returns></returns>
        public async Task<ShoppingCartEntity> GetShoppingCartAsync(long customerId)
        {
            /*
             * 为了不在应用层依赖ef，这里使用多次查询方式。多次查询性能也并不一定比关联查询差，看系统架构
             * 也可以考虑将此操作放在自定义的仓储中来实现
             */

            var entity = await AsyncQueryableExecuter.FirstOrDefaultAsync(repository.GetAllIncluding(c => c.Items, c => c.Customer).Where(c => c.CustomerId == customerId));
            var productIds = entity.Items.Select(c => c.ProductId);
            //var skuIds = entity.Items.Select(c => c.SkuId); //sku不是聚合根，没有仓储
            /*var productWithSkus = */
            await AsyncQueryableExecuter.ToListAsync(productRepository.GetAllIncluding(c => c.Skus).Where(c => productIds.Contains(c.Id)));
            //ef查询后默认会建立关联关系，若换其它仓储实现，可以考虑这里重组关联关系，由于某些属性在领域实体是私有的，应该重新new
            return entity;
        }
        //若将来要添加逻辑，也可以在事件中去处理
        ///// <summary>
        ///// 向购物车添加商品明细
        ///// 将来可能涉及类似库存判断和其它也处理，因此这里定义领域服务方法
        ///// </summary>
        ///// <param name="shoppingCart"></param>
        ///// <param name="item"></param>
        ///// <returns></returns>
        //public Task AddItemAsync(ShoppingCartEntity shoppingCart, ShoppingCartItemEntity item)
        //{
        //    shoppingCart.AddItem(item);
        //    return Task.CompletedTask;
        //}

        //下单页面并不只是包含订单信息，还包含其它信息，如：可选收货地址，各种优惠、其它可选信息等
        //订单实体严格来说应该是一个符合订单要求的严谨的实体，因此箱单页面类似OrderBuilder它可以多次配置，最终生成一个严谨的订单对象
        ///// <summary>
        ///// 购物车结算，将根据购物车生成新的订单，但这个订单并未存储到数据库中，且购物车也不会清空
        ///// 而是等到真正产生订单并保存到数据库后才会清空购物车
        ///// </summary>
        ///// <param name="shoppingCart"></param>
        ///// <param name="itemIds">顾客指定的要结算的购物车明细的id集合</param>
        ///// <returns></returns>
        //public virtual async Task<OrderEntity> BuildOrder(ShoppingCartEntity shoppingCart, params long[] itemIds)
        //{
        //    var order = await orderManager.Value.BuildOrderAsync(shoppingCart.Customer,
        //        shoppingCart.Items.Where(c => itemIds.Contains(c.Id)).Select(c => new OrderItemInput(c.Product, c.Sku, c.Quantity)).ToArray());

        //    return order;
        //}
    }
}
