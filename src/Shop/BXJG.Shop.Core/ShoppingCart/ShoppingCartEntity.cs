using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Shop.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart
{
    /*
     * 简单的情况可以不定义购物车，而直接将购物明细关联到顾客
     * 单购物车是业务场景中实际存在的概念，定义购物车更符合实际场景，也便于将来扩展
     */

    /// <summary>
    /// 购物车实体
    /// </summary>
    public class ShoppingCartEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        /// <summary>
        /// 所属租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 所属顾客id
        /// </summary>
        public long CustomerId { get; set; }
        /// <summary>
        /// 所属顾客
        /// </summary>
        public virtual CustomerEntity Customer { get; set; }
        /// <summary>
        /// 购物车中的商品明细
        /// </summary>
        public virtual IList<ShoppingCartItemEntity> Items { get; set; }

        public void AddItem(ShoppingCartItemEntity item)
        {
            Items.Add(item);
        }
        public void RemoveItem(ShoppingCartItemEntity item)
        {
            Items.Remove(item);
        }
        /*
         * 可以考虑定义成扩展方法
         */
        public void RemoveItem(long itemId)
        {
            Items.Remove(Items.Single(c => c.Id == itemId));
        }
        public void ClearItems()
        {
            Items.Clear();
        }
    }
}
