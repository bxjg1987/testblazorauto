using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    //public class SkuDto : EntityDto<long>
    //{
    //    /// <summary>
    //    /// 动态实体属性值
    //    /// </summary>
    //    public List<DynamicEntityPropertyValueDto> DynamicEntityPropertyValues { get; set; }
    //    /// <summary>
    //    /// 原价
    //    /// </summary>
    //    public decimal OldPrice { get; set; }
    //    /// <summary>
    //    /// 现价(销售价)
    //    /// </summary>
    //    public decimal Price { get; set; }
    //    /// <summary>
    //    /// 积分
    //    /// </summary>
    //    public int Integral { get; set; }
    //}

    public class SkuDto : EntityDto<long>
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
        public int? DynamicProperty1Id { get; set; }
        /// <summary>
        /// 第1个动态属性名称
        /// </summary>
        public string DynamicProperty1Name { get; set; }
        public string DynamicProperty1DisplayName { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty1Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty1Text { get; set; }
        /// <summary>
        /// 第2个动态属性id
        /// </summary>
        public int? DynamicProperty2Id { get; set; }
        /// <summary>
        /// 第2个动态属性名称
        /// </summary>
        public string DynamicProperty2Name { get; set; }
        public string DynamicProperty2DisplayName { get; set; }
        /// <summary>
        /// 第2个动态属性值
        /// </summary>
        public string DynamicProperty2Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty2Text { get; set; }
        /// <summary>
        /// 第3个动态属性id
        /// </summary>
        public int? DynamicProperty3Id { get; set; }
        /// <summary>
        /// 第3个动态属性名称
        /// </summary>
        public string DynamicProperty3Name { get; set; }
        public string DynamicProperty3DisplayName { get; set; }
        /// <summary>
        /// 第3个动态属性值
        /// </summary>
        public string DynamicProperty3Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty3Text { get; set; }
        /// <summary>
        /// 第4个动态属性id
        /// </summary>
        public int? DynamicProperty4Id { get; set; }
        /// <summary>
        /// 第4个动态属性名称
        /// </summary>
        public string DynamicProperty4Name { get; set; }
        public string DynamicProperty4DisplayName { get; set; }
        /// <summary>
        /// 第4个动态属性值
        /// </summary>
        public string DynamicProperty4Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty4Text { get; set; }
        /// <summary>
        /// 第5个动态属性Id
        /// </summary>
        public int? DynamicProperty5Id { get; set; }
        /// <summary>
        /// 第5个动态属性名称
        /// </summary>
        public string DynamicProperty5Name { get; set; }
        public string DynamicProperty5DisplayName { get; set; }
        /// <summary>
        /// 第5个动态属性值
        /// </summary>
        public string DynamicProperty5Value { get; set; }
        /// <summary>
        /// 第一个动态属性值
        /// </summary>
        public string DynamicProperty5Text { get; set; }
        #endregion
    }

}
