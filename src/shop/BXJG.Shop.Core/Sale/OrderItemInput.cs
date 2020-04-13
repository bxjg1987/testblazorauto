using BXJG.Shop.Catalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /*
     * 一个手机 可能存在 全网通、电信、移动的选择，也包含颜色的选择
     * 购买时这些是由顾客决定的，将来这些数据也需要定义在这里
     * 
     * 注意应用DDD 原则是针对内部属性的处理放在当前类中，某些逻辑设计到其它组件时 应该将对应逻辑放进领域服务
     * 比如：将OrderItemInput转换为OrderItemEntity的逻辑不要定义在这个类里面，而应该单独定义一个类或直接放在订单的领域服务中
     */

    /// <summary>
    /// 订单明细创建项
    /// 创建订单时 用来向订单添加购买的产品明细时需要的输入模型
    /// 将根据此模型创建OrderItemEntity，它内部的很多属性的值都来自商品上架信息（ItemEntity）
    /// </summary>
    public class OrderItemInput
    {
        /// <summary>
        /// 商品上架信息
        /// </summary>
        public ItemEntity Item { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Count { get; set; }

        //不要定义无参的构造函数，不包含的购买的产品或数量 此对象都不是一个有意义的对象
        //public OrderItemInput(){}

        /// <summary>
        /// 实例化订单明细创建项
        /// </summary>
        /// <param name="itemEntity">商品上架信息</param>
        /// <param name="count">要购买的数量</param>
        public OrderItemInput(ItemEntity itemEntity, decimal count)
        {
            this.Item = itemEntity;
            this.Count = count;

            //此次倒是可以对ItemEntity进行数据校验
            //判断库存是否够，需要使用到其它组件的功能，因此不要放这里面
        }
    }
}
