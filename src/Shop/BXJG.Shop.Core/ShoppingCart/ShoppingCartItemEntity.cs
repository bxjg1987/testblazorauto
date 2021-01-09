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
    */

    /// <summary>
    /// 购物车中的商品明细
    /// </summary>
    public class ShoppingCartItemEntity : Entity<long>
    {
        /// <summary>
        /// 所属购物车id
        /// </summary>
        public long ShoppingCartId { get; set; }
        /// <summary>
        /// 所属购物车
        /// </summary>
        public virtual ShoppingCartEntity ShoppingCart { get; set; }
        /// <summary>
        /// 所属产品(spu)的Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 所属产品(spu)
        /// </summary>
        public virtual ProductEntity Product { get; set; }
        /// <summary>
        /// 购物扯中的商品的skuid
        /// 若购买的商品是简单商品，没有sku，则sku属性可为空
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 购物扯中的商品的sku
        /// 若购买的商品是简单商品，没有sku，则sku属性可为空
        /// </summary>
        public virtual SkuEntity Sku { get; set; }
        /// <summary>
        /// 预购买的数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
