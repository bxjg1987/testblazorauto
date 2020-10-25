using Abp.Domain.Entities;
using Abp.DynamicEntityProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品的sku信息，= 动态属性组合 + 价格...
    /// </summary>
    public class SkuEntity : Entity<long>//, IPassivable
    {
        //sku属性由abp动态属性提供

        /// <summary>
        /// 原价
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 现价(销售价)
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        /// <summary>
        /// 所属产品(spu)的Id
        /// </summary>
        public long ProductId { get; set; }
        /// <summary>
        /// 所属产品(spu)
        /// </summary>
        public virtual ProductEntity Product { get; set; }
        ///// <summary>
        ///// 是否启用
        ///// </summary>
        //public bool IsActive { get; set; }
        #region sku动态属性组合

        /// <summary>
        /// 第1个动态属性id
        /// </summary>
        public int DynamicEntityProperty1Id { get; set; }
        /// <summary>
        /// 第1个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty1Name { get; set; }
        /// <summary>
        /// 第1个动态属性
        /// </summary>
        public virtual DynamicEntityProperty DynamicEntityProperty1 { get; set; }
        /// <summary>
        /// 第1个动态属性值
        /// </summary>
        public string DynamicEntityProperty1Value { get; set; }
        /// <summary>
        /// 第1个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicEntityProperty1Text { get; set; }
        /// <summary>
        /// 第2个动态属性id
        /// </summary>
        public int? DynamicEntityProperty2Id { get; set; }
        /// <summary>
        /// 第2个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty2Name { get; set; }
        /// <summary>
        /// 第2个动态属性
        /// </summary>
        public virtual DynamicEntityProperty DynamicEntityProperty2 { get; set; }
        /// <summary>
        /// 第2个动态属性值
        /// </summary>
        public string DynamicEntityProperty2Value { get; set; }
        /// <summary>
        /// 第2个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicEntityProperty2Text { get; set; }
        /// <summary>
        /// 第3个动态属性id
        /// </summary>
        public int? DynamicEntityProperty3Id { get; set; }
        /// <summary>
        /// 第3个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty3Name { get; set; }
        /// <summary>
        /// 第3个动态属性
        /// </summary>
        public virtual DynamicEntityProperty DynamicEntityProperty3 { get; set; }
        /// <summary>
        /// 第3个动态属性值
        /// </summary>
        public string DynamicEntityProperty3Value { get; set; }
        /// <summary>
        /// 第3个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicEntityProperty3Text { get; set; }
        /// <summary>
        /// 第4个动态属性id
        /// </summary>
        public int? DynamicEntityProperty4Id { get; set; }
        /// <summary>
        /// 第4个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty4Name { get; set; }
        /// <summary>
        /// 第4个动态属性
        /// </summary>
        public virtual DynamicEntityProperty DynamicEntityProperty4 { get; set; }
        /// <summary>
        /// 第4个动态属性值
        /// </summary>
        public string DynamicEntityProperty4Value { get; set; }
        /// <summary>
        /// 第4个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicEntityProperty4Text { get; set; }
        /// <summary>
        /// 第5个动态属性Id
        /// </summary>
        public int? DynamicEntityProperty5Id { get; set; }
        /// <summary>
        /// 第5个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty5Name { get; set; }
        /// <summary>
        /// 第5个动态属性
        /// </summary>
        public virtual DynamicEntityProperty DynamicEntityProperty5 { get; set; }
        /// <summary>
        /// 第5个动态属性值
        /// </summary>
        public string DynamicEntityProperty5Value { get; set; }
        /// <summary>
        /// 第5个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicEntityProperty5Text { get; set; }
        #endregion

    }
}
