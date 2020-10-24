using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Sale
{
    /// <summary>
    /// 前端、后端的查询订单的明细都使用这个类型
    /// </summary>
    public class OrderItemDto : EntityDto<long>
    {
        /// <summary>
        /// 关联的订单Id
        /// </summary>
        public long OrderId { get; set; }
        /// <summary>
        /// 关联的商品上架信息Id
        /// </summary>
        public long ProductId { get; set; }
        #region sku
        /// <summary>
        /// skuId
        /// </summary>
        public long? SkuId { get; set; }
        /// <summary>
        /// 第1个动态属性id
        /// </summary>
        public int DynamicEntityProperty1Id { get; set; }
        /// <summary>
        /// 第1个动态属性名称
        /// </summary>
        public string DynamicEntityProperty1DynamicPropertyPropertyName { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty1Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty1Text { get; set; }
        /// <summary>
        /// 第2个动态属性id
        /// </summary>
        public int? DynamicEntityProperty2Id { get; set; }
        /// <summary>
        /// 第2个动态属性名称
        /// </summary>
        public string DynamicEntityProperty2DynamicPropertyPropertyName { get; set; }
        /// <summary>
        /// 第2个动态属性值
        /// </summary>
        public string DynamicEntityProperty2Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty2Text { get; set; }
        /// <summary>
        /// 第3个动态属性id
        /// </summary>
        public int? DynamicEntityProperty3Id { get; set; }
        /// <summary>
        /// 第3个动态属性名称
        /// </summary>
        public string DynamicEntityProperty3DynamicPropertyPropertyName { get; set; }
        /// <summary>
        /// 第3个动态属性值
        /// </summary>
        public string DynamicEntityProperty3Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty3Text { get; set; }
        /// <summary>
        /// 第4个动态属性id
        /// </summary>
        public int? DynamicEntityProperty4Id { get; set; }
        /// <summary>
        /// 第4个动态属性名称
        /// </summary>
        public string DynamicEntityProperty4DynamicPropertyPropertyName { get; set; }
        /// <summary>
        /// 第4个动态属性值
        /// </summary>
        public string DynamicEntityProperty4Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty4Text { get; set; }
        /// <summary>
        /// 第5个动态属性Id
        /// </summary>
        public int? DynamicEntityProperty5Id { get; set; }
        /// <summary>
        /// 第5个动态属性名称
        /// </summary>
        public string DynamicEntityProperty5DynamicPropertyPropertyName { get; set; }
        /// <summary>
        /// 第5个动态属性值
        /// </summary>
        public string DynamicEntityProperty5Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityProperty5Text { get; set; }
        #endregion
        /// <summary>
        /// 产品标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 产品图片
        /// 与商品上架信息不同，这里只需要单张图片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public int TotalIntegral { get; set; }
    }
}
