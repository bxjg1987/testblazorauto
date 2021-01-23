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
     * 
     * 定义私有属性的目的是强制要求对象状态是符合业务要求的
     * EF查询时可以对私有属性赋值
     * AutoMapper可以使用构造函数映射
     */

    /// <summary>
    /// 购物车实体
    /// </summary>
    public class ShoppingCartEntity : FullAuditedAggregateRoot<long>, IMustHaveTenant, IExtendableObject
    {
        #region 字段和属性
        /// <summary>
        /// 用来存储购物车明细，也就是购物车中的商品和对应的数量
        /// </summary>
        private List<ShoppingCartItemEntity> items;
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
        public long CustomerId => Customer.Id;
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
        #endregion

        #region 构造函数
        /// <summary>
        /// 私有的，防止调用方创建不符合业务要求的购物车实体
        /// <br />这个是给ef用的，用ef查询时ef可以访问到此构造函数
        /// </summary>
        private ShoppingCartEntity() { }
        /// <summary>
        /// 开发人员可以调用此构造函数创建符合业务要求的购物车实体
        /// <br />AutoMapper也可以使用构造函数映射
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="customer"></param>
        /// <param name="tenantId"></param>
        /// <param name="extensionData"></param>
        /// <param name="items"></param>
        public ShoppingCartEntity(CustomerEntity customer,
                                  int tenantId = default,
                                  string extensionData = default,
                                  IReadOnlyList<ShoppingCartItemEntity> items = default)
        {
            TenantId = tenantId;
            ExtensionData = extensionData;
            
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

        #endregion

        #region 公共方法
        /// <summary>
        /// 添加购物车明细
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(ShoppingCartItemEntity item)
        {
            //CheckNullCustomer();
            RegisterValueChangedEvent(item);
            items.Add(item);
            Calculate();
            DomainEvents.Add(new AddItemToShoppingCartEventData(Customer, this, item));
        }
        /// <summary>
        /// 移除指定购物车明细
        /// </summary>
        /// <param name="item"></param>
        public void RemoveItem(ShoppingCartItemEntity item)
        {
            //CheckNullCustomer();
            UnRegisterValueChangedEvent(item);//存在隐患，因为此操作不参与数据库事务
            items.Remove(item);
            Calculate();
            DomainEvents.Add(new RemoveItemFromShoppingCartEventData(Customer, this, item));
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
            UnRegisterValueChangedEvent();//存在隐患，因为此操作不参与数据库事务
            items.Clear();
            Calculate();
            DomainEvents.Add(new ClearShoppingCartEventData(Customer, this));
        }
        /// <summary>
        /// 获取指定id的购物车明细
        /// </summary>
        /// <param name="itemId">购物车明细id，非商品id</param>
        /// <returns></returns>
        public ShoppingCartItemEntity GetById(long itemId)
        {
            return items.Single(c => c.Id == itemId);
        }
        /// <summary>
        /// 重置购物车明细值变化的事件
        /// </summary>
        public void RegisterValueChangedEvent()
        {
            foreach (var item in items)
            {
                RegisterValueChangedEvent(item);
            }
        }
        /// <summary>
        /// 取消明细数量变化的事件
        /// </summary>
        public void UnRegisterValueChangedEvent()
        {
            foreach (var item in items)
            {
                UnRegisterValueChangedEvent(item);
            }
        }
        #endregion

        #region 私有的辅助方法
        /// <summary>
        /// 当明细的值变化时重新计算总金额和总积分
        /// </summary>
        private void Item_ValueChanged(ShoppingCartItemEntity item, ShoppingCartChangeData shoppingCartEventData)
        {
            Calculate();
            DomainEvents.Add(new ChangeShoppingCartItemQuantityEventData(Customer, this, item, shoppingCartEventData));
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
        ///// <summary>
        ///// 若Customer为空则抛异常
        ///// </summary>
        //private void CheckNullCustomer()
        //{
        //    if (Customer == null)
        //        throw new NullReferenceException("添加商品到购物车时会触发相应事件，此事件依赖Customer，但此属性为空");
        //}
        /// <summary>
        /// 为购物车明细事件注册处理程序
        /// </summary>
        /// <param name="item"></param>
        private void RegisterValueChangedEvent(ShoppingCartItemEntity item)
        {
            item.ValueChanged -= Item_ValueChanged;
            item.ValueChanged += Item_ValueChanged;
        }
        /// <summary>
        /// 为购物车明细事件注销处理程序
        /// </summary>
        /// <param name="item"></param>
        private void UnRegisterValueChangedEvent(ShoppingCartItemEntity item)
        {
            item.ValueChanged -= Item_ValueChanged;
        }
        #endregion
    }
}
