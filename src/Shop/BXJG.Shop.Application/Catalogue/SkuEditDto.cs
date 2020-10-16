using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    public class SkuEditDto : EntityDto<long>
    {
        ///// <summary>
        ///// 动态实体属性值
        ///// key：动态实体属性id；value：值
        ///// </summary>
        //public Dictionary<int, string> DynamicEntityPropertyValues { get; set; }
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
        #region sku动态属性组合
        /// <summary>
        /// 第1个动态属性id
        /// </summary>
        public int DynamicEntityProperty1Id { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicEntityPropertyValue1 { get; set; }
        /// <summary>
        /// 第2个动态属性id
        /// </summary>
        public int? DynamicEntityProperty2Id { get; set; }
        /// <summary>
        /// 第2个动态属性值
        /// </summary>
        public string DynamicEntityPropertyValue2 { get; set; }
        /// <summary>
        /// 第3个动态属性id
        /// </summary>
        public int? DynamicEntityProperty3Id { get; set; }
        /// <summary>
        /// 第3个动态属性值
        /// </summary>
        public string DynamicEntityPropertyValue3 { get; set; }
        /// <summary>
        /// 第4个动态属性id
        /// </summary>
        public int? DynamicEntityProperty4Id { get; set; }
        /// <summary>
        /// 第4个动态属性值
        /// </summary>
        public string DynamicEntityPropertyValue4 { get; set; }
        /// <summary>
        /// 第5个动态属性Id
        /// </summary>
        public int? DynamicEntityProperty5Id { get; set; }
        /// <summary>
        /// 第5个动态属性值
        /// </summary>
        public string DynamicEntityPropertyValue5 { get; set; }
        #endregion
    }
}
