using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 面向顾客的商品查询时的输入模型
    /// </summary>
    public class GetAllFrontItemInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// 所属商品类别id
        /// </summary>
        public long? CategoryId { get; set; }
        /// <summary>
        /// 分类Code
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public long? BrandId { get; set; }
        /// <summary>
        /// 关键字
        /// Title/DescriptionShort/SKU等..模糊查询
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 价格范围下限
        /// </summary>
        public decimal? PriceMin { get; set; }
        /// <summary>
        /// 价格范围上限
        /// </summary>
        public decimal? PriceMax { get; set; }
        /// <summary>
        /// 是否热卖
        /// </summary>
        public bool? Hot { get; set; }
        /// <summary>
        /// 是否新品
        /// </summary>
        public bool? New { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool? Home { get; set; }
        /// <summary>
        /// 是否显示在轮播图片中
        /// </summary>
        public bool? Focus { get; set; }
        /// <summary>
        /// 模型绑定后，abp会调用此方法来进一步初始化
        /// </summary>
        public void Normalize()
        {
            if (this.Sorting.IsNullOrEmpty())
                this.Sorting = "creationtime desc";
        }
    }
}
