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
     * 但定义购物车更符合实际场景，也便于将来扩展
     */

    /// <summary>
    /// 购物车实体
    /// </summary>
    public class ShoppingCartEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        List<ShoppingCartItemEntity> items;

        private ShoppingCartEntity() { }//此构造函数给ef用
        //此构造函数给automapper或开发人员用
        public ShoppingCartEntity(long customerId,
                                  CustomerEntity customer = default,
                                  int tenantId = default,
                                  string extensionData = default,
                                  IReadOnlyList<ShoppingCartItemEntity> items = default)
        {
            TenantId = tenantId;
            ExtensionData = extensionData;
            CustomerId = customerId;
            Customer = customer;
            if (items != null)
            {
                this.items = items.ToList();
                RegisterValueChangedEvent();
            }
            if (this.items == default)
                this.items = new List<ShoppingCartItemEntity>();
            Calculate();
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
        /// 所属顾客id
        /// </summary>
        public long CustomerId { get; private set; }
        /// <summary>
        /// 所属顾客
        /// </summary>
        public virtual CustomerEntity Customer { get; private set; }
        /// <summary>
        /// 购物车中的商品明细
        /// </summary>
        public virtual IReadOnlyList<ShoppingCartItemEntity> Items => items.AsReadOnly();
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get; private set; }
        /// <summary>
        /// 购物车中包含的商品明细的可得积分总额
        /// </summary>
        public int IntegralTotal { get; private set; }
        /// <summary>
        /// 添加购物车明细
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ShoppingCartItemEntity item)
        {
            items.Add(item);
            item.ValueChanged -= Item_ValueChanged;
            item.ValueChanged += Item_ValueChanged;
            Calculate();
            //base.DomainEvents.Add( )
            //触发事件..略..或直接用购物车更新事件，但不推荐
        }
        /// <summary>
        /// 重置购物车明细值变化的事件
        /// </summary>
        public void RegisterValueChangedEvent()
        {
            foreach (var item in Items)
            {
                item.ValueChanged -= Item_ValueChanged;
                item.ValueChanged += Item_ValueChanged;
            }
        }
        /// <summary>
        /// 当明细的值变化时重新计算总金额和总积分
        /// </summary>
        private void Item_ValueChanged()
        {
            Calculate();
        }
        /// <summary>
        /// 移除指定购物车明细
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ShoppingCartItemEntity item)
        {
            items.Remove(item);
            Calculate();
            //触发事件..略..或直接用购物车更新事件，但不推荐
        }
        /// <summary>
        /// 移除指定的购物车明细
        /// </summary>
        /// <param name="itemId">购物车明细id，注意不是商品id</param>
        public void RemoveItem(long itemId)
        {
            RemoveItem(items.Single(c => c.Id == itemId));
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        public void ClearItems()
        {
            items.Clear();
            Calculate();
            //触发事件..略..或直接用购物车更新事件，但不推荐
        }
        /// <summary>
        /// 获取指定id的购物车明细
        /// </summary>
        /// <param name="itemId">购物车明细id，非商品id</param>
        /// <returns></returns>
        public ShoppingCartItemEntity GetById(long itemId)
        {
            return Items.Single(c => c.Id == itemId);
        }
        /// <summary>
        /// 计算金额
        /// </summary>
        private void CalculateAmount()
        {
            Amount = Items.Sum(c => c.Amount);
        }
        /// <summary>
        /// 计算积分总额
        /// </summary>
        private void CalculateIntegralTotal()
        {
            IntegralTotal = Items.Sum(c => c.IntegralTotal);
        }
        /// <summary>
        /// 重新计算积分和金额
        /// </summary>
        private void Calculate()
        {
            CalculateAmount();
            CalculateIntegralTotal();
        }
    }
}
