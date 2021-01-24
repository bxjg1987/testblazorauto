using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    /*
     * 若购买的商品是简单商品，没有sku，则sku属性可为空
     * 若购买的商品的某个sku，则商品和sku都要设置对应的值
     * 
     * ef查询时好像可以给私有字段赋值
     * automapper貌似可以通过构造函数做映射
     * 这样保证领域实体的状态正确
     */

    /// <summary>
    /// 购物车中的商品明细
    /// </summary>
    public class ShoppingCartItemEntity : Entity<long>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 当金额和积分总额变化时触发
        /// </summary>
        public event Action<ShoppingCartItemEntity, ShoppingCartChangeData> ValueChanged;

        //SkuEntity sku;
        //ProductEntity product;
        decimal quantity;
        private ShoppingCartItemEntity() { }//此构造函数给ef用
        //此构造函数给automapper或开发人员用
        public ShoppingCartItemEntity(ShoppingCartEntity shoppingCart,
                                      ProductEntity product,
                                      SkuEntity sku,
                                      decimal quantity = default,
                                      int tenantId = default,
                                      string extensionData = default)
        {
            TenantId = tenantId;
            ShoppingCartId = shoppingCart.Id;
            ProductId = product.Id;
            SkuId = sku?.Id;
            ExtensionData = extensionData;
            ShoppingCart = shoppingCart;
            Product = product;
            Sku = sku;
            Quantity = quantity;
        }

        /// <summary>
        /// 所属租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// abp方式的扩展字段
        /// </summary>
        public string ExtensionData { get; set; }
        /// <summary>
        /// 所属购物车id
        /// </summary>
        public long ShoppingCartId { get; private set; }
        /// <summary>
        /// 所属购物车
        /// </summary>
        public virtual ShoppingCartEntity ShoppingCart { get; private set; }
        /// <summary>
        /// 所属产品(spu)的Id
        /// </summary>
        public long ProductId { get; private set; }
        /// <summary>
        /// 所属产品(spu)
        /// </summary>
        public virtual ProductEntity Product { get; private set; }
        /// <summary>
        /// 购物扯中的商品的skuid
        /// 若购买的商品是简单商品，没有sku，则sku属性可为空
        /// </summary>
        public long? SkuId { get; private set; }
        /// <summary>
        /// 购物扯中的商品的sku
        /// 若购买的商品是简单商品，没有sku，则sku属性可为空
        /// </summary>
        public virtual SkuEntity Sku { get; private set; }
        /// <summary>
        /// 预购买的数量
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                var eventData = new ShoppingCartChangeData(quantity, Amount, IntegralTotal);
               
                quantity = value;
                if (eventData.OriginalQuantity == value)
                    return;
                if (Sku == null)
                {
                    Amount = quantity * Product.Price;
                    IntegralTotal = Convert.ToInt32(quantity * Product.Integral);
                }
                else
                {
                    Amount = quantity * Sku.Price;
                    IntegralTotal = Convert.ToInt32(quantity * Sku.Integral);
                }
                ValueChanged?.Invoke(this, eventData);//目前只有数量改变时才会重新计算积分和金额，因此这里调用，后续考虑此逻辑移动到Amount的Setter中
            }
        }
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 可得积分
        /// </summary>
        public int IntegralTotal { get; private set; }
    }
}