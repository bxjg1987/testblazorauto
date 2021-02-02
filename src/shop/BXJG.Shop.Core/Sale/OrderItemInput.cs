using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 一个手机 可能存在 全网通、电信、移动的选择，也包含颜色的选择
     * 
     * 注意应用DDD 原则是针对内部属性的处理放在当前类中，某些逻辑设计到其它组件时 应该将对应逻辑放进领域服务
     * 比如：将OrderItemInput转换为OrderItemEntity的逻辑不要定义在这个类里面，而应该单独定义一个类或直接放在订单的领域服务中
     */

    /// <summary>
    /// 订单明细创建项 就是一个普通的传输对象
    /// 创建订单时 用来向订单添加购买的产品明细时需要的输入模型
    /// 将根据此模型创建OrderItemEntity，它内部的很多属性的值都来自商品上架信息（ProductEntity）
    /// </summary>
    public class OrderItemInput
    {
        /// <summary>
        /// 商品上架信息
        /// </summary>
        public ProductEntity Product { get; private set; }
        /// <summary>
        /// sku，如果没有则null
        /// </summary>
        public SkuEntity Sku { get; private set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; private set; }

        //不要定义无参的构造函数，不包含的购买的产品或数量 此对象都不是一个有意义的对象
        //public OrderItemInput(){}

        /// <summary>
        /// 实例化订单明细创建项
        /// </summary>
        /// <param name="itemEntity">商品上架信息</param>
        /// <param name="sku">sku，如果没有请传入null</param>
        /// <param name="quantity">要购买的数量</param>
        public OrderItemInput(ProductEntity itemEntity, SkuEntity sku, decimal quantity)
        {
            this.Product = itemEntity;
            this.Sku = sku;
            this.Quantity = quantity;
        }
        ///// <summary>
        ///// 计算售价金额
        ///// 定义成方法而不是属性，这样调用方明确知道此逻辑是每次计算的
        ///// </summary>
        ///// <returns></returns>
        //public decimal CalculationAmount()
        //{
        //    decimal price;
        //    if (Sku != null)
        //        price = Sku.Price;
        //    else
        //        price = Product.Price;

        //    return price * Quantity;
        //}
        ///// <summary>
        ///// 计算原价金额
        ///// 定义成方法而不是属性，这样调用方明确知道此逻辑是每次计算的
        ///// </summary>
        ///// <returns></returns>
        //public decimal CalculationOldAmount()
        //{
        //    decimal price;
        //    if (Sku != null)
        //        price = Sku.OldPrice;
        //    else
        //        price = Product.OldPrice;

        //    return price * Quantity;
        //}
        ///// <summary>
        ///// 计算积分
        ///// </summary>
        ///// <returns></returns>
        //public int CalculationIntegral()
        //{
        //    int price;
        //    if (Sku != null)
        //        price = Sku.Integral;
        //    else
        //        price = Product.Integral;

        //    return Convert.ToInt32(price * Quantity);
        //}
    }
}
