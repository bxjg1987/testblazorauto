using Abp.Events.Bus;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    /*
     * 以购物车关联顾客实体为例
     * 若事件中关联的顾客实体，因为事件是在购物车实体中被触发的，所以购物车实体中也需要关联顾客实体，从而导致应用层查询购物车时也需要把关联的对象都查出来。
     * 同理，商品明细关联的商品、sku等也必须是实体，查询时也得关联查询出来。
     * 这种情况事件处理程序就比较简单，能直接拿到事件相关的信息，不用自己再去查
     * 当事件没有被订阅，或事件处理程序中根本用不到关联的实体信息时，会比较浪费性能
     * 
     * 若事件关联的是实体id，如：购物车相关事件中的顾客id，那么上面说的几条就刚好相反
     * 
     * 综合考虑后我们还是选择后者，但由于这里是购物车相关事件，所以购物车及明细我们还是使用实体，明细中关联的商品、sku也使用关联id而非实体
     * 
     */

    /// <summary>
    /// 购物车相关事件抽象类
    /// </summary>
    public class ShoppingCartEventData : EventData
    {
        public long CustomerId { get; }
        public ShoppingCartEntity ShoppingCart { get; }
        public ShoppingCartEventData(long customerId, ShoppingCartEntity shoppingCart)
        {
            CustomerId = customerId;
            ShoppingCart = shoppingCart;
        }
    }
    /// <summary>
    /// 购物车明细相关事件
    /// </summary>
    public class ShoppingCartItemEventData : ShoppingCartEventData
    {
        public ShoppingCartItemEntity ShoppingCartItem { get; }
        public ShoppingCartItemEventData(long customerId, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customerId, shoppingCart)
        {
            ShoppingCartItem = shoppingCartItem;
        }
    }
    /// <summary>
    /// 当商品被加入购物车时的事件，处理中抛出异常可以阻止此操作
    /// </summary>
    public class AddItemEventData : ShoppingCartItemEventData
    {
        /// <summary>
        /// 实例化当商品被加入购物车时的事件实例
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="shoppingCartItem"></param>
        public AddItemEventData(long customerId, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customerId, shoppingCart, shoppingCartItem)
        {
        }
    }
    /// <summary>
    /// 从购物车中移除商品时的事件
    /// </summary>
    public class RemoveItemEventData : ShoppingCartItemEventData
    {
        /// <summary>
        /// 实例化从购物车中移除商品时的事件实例
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="shoppingCartItem"></param>
        public RemoveItemEventData(long customerId, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customerId, shoppingCart, shoppingCartItem)
        {
        }
    }
    /// <summary>
    /// 当购物车明细数量发生改变时会触发事件，此类则是这个事件需要携带的参数
    /// <br />注意这不是abp事件，而是购物车明细中的c#事件
    /// </summary>
    public class ShoppingCartItemChangeData
    {
        public decimal OriginalQuantity { get; }
        public decimal OriginalAmount { get; }
        public int OriginalIntegralTotal { get; }
        public ShoppingCartItemChangeData( decimal originalQuantity, decimal originalAmount, int originalIntegralTotal)
        {
            OriginalQuantity = originalQuantity;
            OriginalAmount = originalAmount;
            OriginalIntegralTotal = originalIntegralTotal;
        }
    }
    /// <summary>
    /// 购物车明细数量变更时的事件
    /// </summary>
    public class ChangeItemQuantityEventData : ShoppingCartItemEventData
    {
        public decimal OriginalQuantity { get; }
        public decimal OriginalAmount { get; }
        public int OriginalIntegralTotal { get; }

        public ChangeItemQuantityEventData(long customerId, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem, ShoppingCartItemChangeData shoppingCartChangeData) : base(customerId, shoppingCart, shoppingCartItem)
        {
            OriginalQuantity = shoppingCartChangeData.OriginalQuantity;
            OriginalAmount = shoppingCartChangeData.OriginalAmount;
            OriginalIntegralTotal = shoppingCartChangeData.OriginalIntegralTotal;
        }
    }
    /// <summary>
    /// 清空购物车时的事件
    /// </summary>
    public class ClearEventData : ShoppingCartEventData
    {
        /// <summary>
        /// 清空购物车时的事件
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="shoppingCart"></param>
        public ClearEventData(long customerId, ShoppingCartEntity shoppingCart) : base(customerId, shoppingCart)
        {
        }
    }
}