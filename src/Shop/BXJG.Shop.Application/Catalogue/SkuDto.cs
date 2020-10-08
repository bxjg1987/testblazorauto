using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 新增需改商品时相关的sku信息
    /// </summary>
    public class SkuDto : EntityDto<long>
    {
        /// <summary>
        /// 动态属性
        /// key：动态实体属性id；value：值
        /// </summary>
        public Dictionary<int, string> DynamicEntityPropertyValues { get; set; }
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
    }
}
