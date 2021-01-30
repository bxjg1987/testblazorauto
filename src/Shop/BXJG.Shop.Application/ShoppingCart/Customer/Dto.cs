using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Shop.ShoppingCart.Customer
{
    /*
     * 顾客端针对购物车的操作相关dto
     */

    ///// <summary>
    ///// 购物车明细Dto基类
    ///// </summary>
    //public class ItemBaseDto
    //{
    //    /// <summary>
    //    /// 商品Id
    //    /// </summary>
    //    [Required]
    //    public long ProductId { get; set; }
    //    /// <summary>
    //    /// skuId，如果有的话
    //    /// </summary>
    //    public long? SkuId { get; set; }
    //    /// <summary>
    //    /// 数量
    //    /// </summary>
    //    public decimal Quantity { get; set; }
    //}

    #region 合并并获取购物车信息的相关dto
    /// <summary>
    /// 顾客获取服务端购物车时的输入模型
    /// <rb />需要上传本地购物车，由服务端决定是否合并
    /// </summary>
    public class GetInput
    {
        /// <summary>
        /// 购物车明细
        /// </summary>
        public List<GetItemInput> Items { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public Dictionary<string, string> ExtensionData { get; set; }
    }
    /// <summary>
    /// 顾客获取服务端购物车时的输入模型的购物车明细
    /// </summary>
    public class GetItemInput
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public long ProductId { get; set; }
        /// <summary>
        /// skuId，如果有的话
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; } = 1;
        /// <summary>
        /// 扩展数据
        /// </summary>
        public Dictionary<string, string> ExtensionData { get; set; }
    }
    /// <summary>
    /// 顾客获取购物车信息的输出模型
    /// </summary>
    public class GetOutput
    {
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get;  set; }
        /// <summary>
        /// 购物车中包含的商品明细的可得积分总额
        /// </summary>
        public int IntegralTotal { get;  set; }
        /// <summary>
        /// 购物车明细
        /// </summary>
        public IReadOnlyList<GetItemOutput> Items { get;  set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtensionData { get;  set; }
    }
    /// <summary>
    /// 顾客获取购物车信息的输出模型中的购物车明细
    /// </summary>
    public class GetItemOutput
    {
        /// <summary>
        /// 关联的商品id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 关联的商品标题
        /// </summary>
        public string ProductTitle { get; set; }
        /// <summary>
        /// 关联的商品单位
        /// </summary>
        public string ProductUnitDisplayName { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal ProductPrice { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>
        public decimal ProductOldPrice { get; set; }
        /// <summary>
        /// 商品积分
        /// </summary>
        public decimal ProductIntegral { get; set; }
        /// <summary>
        /// skuId，如果有的话
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// sku动态属性1属性名
        /// </summary>
        public string SkuDynamicProperty1DisplayName { get; set; }
        /// <summary>
        /// sku动态属性1值
        /// </summary>
        public string SkuDynamicProperty1Text { get; set; }
        /// <summary>
        /// sku动态属性2属性名
        /// </summary>
        public string SkuDynamicProperty2DisplayName { get; set; }
        /// <summary>
        /// sku动态属性2
        /// </summary>
        public string SkuDynamicProperty2Text { get; set; }
        /// <summary>
        /// sku动态属性3属性名
        /// </summary>
        public string SkuDynamicProperty3DisplayName { get; set; }
        /// <summary>
        /// sku动态属性3
        /// </summary>
        public string SkuDynamicProperty3Text { get; set; }
        /// <summary>
        /// sku动态属性4属性名
        /// </summary>
        public string SkuDynamicProperty4DisplayName { get; set; }
        /// <summary>
        /// sku动态属性4
        /// </summary>
        public string SkuDynamicProperty4Text { get; set; }
        /// <summary>
        /// sku动态属性5属性名
        /// </summary>
        public string SkuDynamicProperty5DisplayName { get; set; }
        /// <summary>
        /// sku动态属性5
        /// </summary>
        public string SkuDynamicProperty5Text { get; set; }
        /// <summary>
        /// Sku单价
        /// </summary>
        public decimal SkuPrice { get; set; }
        /// <summary>
        /// Sku原价
        /// </summary>
        public decimal SkuOldPrice { get; set; }
        /// <summary>
        /// Sku积分
        /// </summary>
        public decimal SkuIntegral { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; } = 1;
        /// <summary>
        /// 金额小计
        /// </summary>
        public decimal Amount { get;  set; }
        /// <summary>
        /// 购物车中包含的商品明细的可得积分总额
        /// </summary>
        public int IntegralTotal { get;  set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtensionData { get; set; }
    }
    #endregion

    #region 向购物车添加商品使用的dto
    /// <summary>
    /// 顾客将商品加入购物车时的出入模型
    /// </summary>
    public class AddItemInput
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public long ProductId { get; set; }
        /// <summary>
        /// skuId，如果有的话
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; } = 1;
        /// <summary>
        /// 扩展数据
        /// </summary>
        public Dictionary<string, object> ExtensionData { get; set; }
    }
    /// <summary>
    /// 顾客将商品加入购物车时的输出模型
    /// </summary>
    public class AddItemOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    #endregion

    #region 调整指定明细的数量的输入输出dto

    /// <summary>
    /// 调整购物车明细数量的输入模型
    /// </summary>
    public class ChangeItemQuantityInput : EntityDto<long>
    {
        /// <summary>
        /// 在原有数量基础上改变的数量，正数为加、负数为减
        /// </summary>
        public decimal Quantity { get; set; }
    }
    /// <summary>
    /// 调整购物车明细数量的输出模型
    /// </summary>
    public class ChangeItemQuantityOutput
    {

    }
    #endregion

    #region 从购物车中移除明细时的输入/输出模型
    /// <summary>
    /// 从购物车中移除明细时的输入模型
    /// </summary>
    public class RemoveItemInput : BatchOperationInputLong
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    /// <summary>
    /// 从购物车中移除明细时的输出模型
    /// </summary>
    public class RemoveItemOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    #endregion

    #region 清空购物车的输入/输出模型
    /// <summary>
    /// 清空购物车的输入模型
    /// </summary>
    public class ClearInput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    /// <summary>
    /// 清空购物车的输出模型
    /// </summary>
    public class ClearOutput
    {
        //为啥为空？因为将来可以随意调整返回数据，而不必修改接口定义
    }
    #endregion
}
