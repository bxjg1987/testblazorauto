using Abp.Events.Bus;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    /// <summary>
    /// 购物车相关事件抽象类
    /// </summary>
    public class ShoppingCartEventData : EventData
    {
        public virtual CustomerEntity Customer { get; }
        public virtual ShoppingCartEntity ShoppingCart { get; }
        public ShoppingCartEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart)
        {
            Customer = customer;
            ShoppingCart = shoppingCart;
        }
    }
    /// <summary>
    /// 购物车明细相关事件
    /// </summary>
    public class ShoppingCartItemEventData : ShoppingCartEventData
    {
        public virtual ShoppingCartItemEntity ShoppingCartItem { get; }
        public ShoppingCartItemEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customer, shoppingCart)
        {
            ShoppingCartItem = shoppingCartItem;
        }
    }
    /// <summary>
    /// 当商品被加入购物车时的事件，处理中抛出异常可以阻止此操作
    /// </summary>
    public class AddItemToShoppingCartEventData : ShoppingCartItemEventData
    {
        public AddItemToShoppingCartEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customer, shoppingCart, shoppingCartItem)
        {
        }
    }
    /// <summary>
    /// 从购物车中移除商品时的事件
    /// </summary>
    public class RemoveItemFromShoppingCartEventData : ShoppingCartItemEventData
    {
        public RemoveItemFromShoppingCartEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem) : base(customer, shoppingCart, shoppingCartItem)
        {
        }
    }
    public class ShoppingCartChangeData
    {
        public ShoppingCartChangeData(decimal originalQuantity, decimal originalAmount, int originalIntegralTotal)
        {
            OriginalQuantity = originalQuantity;
            OriginalAmount = originalAmount;
            OriginalIntegralTotal = originalIntegralTotal;
        }

        public decimal OriginalQuantity { get; }
        public decimal OriginalAmount { get; }
        public int OriginalIntegralTotal { get; }
    }
    /// <summary>
    /// 从购物车中移除商品时的事件
    /// </summary>
    public class ChangeShoppingCartItemQuantityEventData : ShoppingCartItemEventData
    {
        public decimal OriginalQuantity { get; }
        public decimal OriginalAmount { get; }
        public int OriginalIntegralTotal { get; }

        public ChangeShoppingCartItemQuantityEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart, ShoppingCartItemEntity shoppingCartItem, ShoppingCartChangeData shoppingCartChangeData) : base(customer, shoppingCart, shoppingCartItem)
        {
            OriginalQuantity = shoppingCartChangeData.OriginalQuantity;
            OriginalAmount = shoppingCartChangeData.OriginalAmount;
            OriginalIntegralTotal = shoppingCartChangeData.OriginalIntegralTotal;
        }
    }
    /// <summary>
    /// 清空购物车时的事件
    /// </summary>
    public class ClearShoppingCartEventData : ShoppingCartEventData
    {
        public ClearShoppingCartEventData(CustomerEntity customer, ShoppingCartEntity shoppingCart) : base(customer, shoppingCart)
        {
        }
    }
}