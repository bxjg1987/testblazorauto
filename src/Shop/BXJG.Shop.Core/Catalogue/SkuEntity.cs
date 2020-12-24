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
        public int DynamicProperty1Id { get; set; }
        /// <summary>
        /// 第1个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// 再则考虑动态属性的定义可能需要修改，订单明细是引用sku的，所以最好sku复制一份数据
        /// </summary>
        public string DynamicProperty1Name { get; set; }
        /// <summary>
        /// 多语言环境有用
        /// </summary>
        public string DynamicProperty1DisplayName { get; set; }
        ///// <summary>
        ///// 第1个动态属性
        ///// </summary>
        //public virtual DynamicEntityProperty DynamicEntityProperty1 { get; set; }
        /// <summary>
        /// 第1个动态属性值
        /// </summary>
        public string DynamicProperty1Value { get; set; }
        /// <summary>
        /// 第1个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicProperty1Text { get; set; }
        /// <summary>
        /// 第2个动态属性id
        /// </summary>
        public int? DynamicProperty2Id { get; set; }
        /// <summary>
        /// 第2个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty2Name { get; set; }
        /// <summary>
        /// 多语言环境有用
        /// </summary>
        public string DynamicProperty2DisplayName { get; set; }
        ///// <summary>
        ///// 第2个动态属性
        ///// </summary>
        //public virtual DynamicEntityProperty DynamicEntityProperty2 { get; set; }
        /// <summary>
        /// 第2个动态属性值
        /// </summary>
        public string DynamicProperty2Value { get; set; }
        /// <summary>
        /// 第2个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicProperty2Text { get; set; }
        /// <summary>
        /// 第3个动态属性id
        /// </summary>
        public int? DynamicProperty3Id { get; set; }
        /// <summary>
        /// 第3个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty3Name { get; set; }
        /// <summary>
        /// 多语言环境有用
        /// </summary>
        public string DynamicProperty3DisplayName { get; set; }
        ///// <summary>
        ///// 第3个动态属性
        ///// </summary>
        //public virtual DynamicEntityProperty DynamicEntityProperty3 { get; set; }
        /// <summary>
        /// 第3个动态属性值
        /// </summary>
        public string DynamicProperty3Value { get; set; }
        /// <summary>
        /// 第3个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicProperty3Text { get; set; }
        /// <summary>
        /// 第4个动态属性id
        /// </summary>
        public int? DynamicProperty4Id { get; set; }
        /// <summary>
        /// 第4个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty4Name { get; set; }
        /// <summary>
        /// 多语言环境有用
        /// </summary>
        public string DynamicProperty4DisplayName { get; set; }
        ///// <summary>
        ///// 第4个动态属性
        ///// </summary>
        //public virtual DynamicEntityProperty DynamicEntityProperty4 { get; set; }
        /// <summary>
        /// 第4个动态属性值
        /// </summary>
        public string DynamicProperty4Value { get; set; }
        /// <summary>
        /// 第4个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicProperty4Text { get; set; }
        /// <summary>
        /// 第5个动态属性Id
        /// </summary>
        public int? DynamicProperty5Id { get; set; }
        /// <summary>
        /// 第5个动态属性名
        /// 虽然从导航属性可以关联查询，但是这里存储一份冗余字段查询速度更快。毕竟目前考虑sku的动态属性组合不允许修改
        /// </summary>
        public string DynamicProperty5Name { get; set; }
        /// <summary>
        /// 多语言环境有用
        /// </summary>
        public string DynamicProperty5DisplayName { get; set; }
        ///// <summary>
        ///// 第5个动态属性
        ///// </summary>
        //public virtual DynamicEntityProperty DynamicEntityProperty5 { get; set; }
        /// <summary>
        /// 第5个动态属性值
        /// </summary>
        public string DynamicProperty5Value { get; set; }
        /// <summary>
        /// 第5个动态属性值对应的文本（若是选择的）
        /// </summary>
        public string DynamicProperty5Text { get; set; }
        #endregion
    }
}
