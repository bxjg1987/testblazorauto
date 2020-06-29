using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 创建上架模型时前端提供的数据模型
    /// </summary>
    //[AutoMapTo(typeof(ItemEntity))]
    public class ItemCreateDto
    {
        #region 基本信息
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(BXJGShopConsts.ItemTitleMaxLength)]
        public string Title { get; set; }
        /// <summary>
        /// sku
        /// </summary>
        [StringLength(BXJGShopConsts.ItemSkuMaxLength)]
        public string Sku { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        [StringLength(BXJGShopConsts.ItemDescriptionShortMaxLength)]
        public string DescriptionShort { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string DescriptionFull { get; set; }
        /// <summary>
        /// 图片集合
        /// </summary>
        [Required]
        [StringLength(BXJGShopConsts.ItemImagesMaxLength)]
        public string[] Images { get; set; }
        /// <summary>
        /// 所属类别id
        /// </summary>
        [Required]
        public long CategoryId { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public long? BrandId { get; set; }
        #endregion

        #region 价格信息
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 现价(销售价)
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public int Integral { get; set; }
        #endregion

        #region 上架信息
        /// <summary>
        /// 是否热卖
        /// </summary>
        public bool Hot { get; set; }
        /// <summary>
        /// 是否新品
        /// </summary>
        public bool New { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool Home { get; set; }
        /// <summary>
        /// 是否显示在轮播图片中
        /// </summary>
        public bool Focus { get; set; }
        /// <summary>
        /// 是否已发布
        /// </summary>
        public bool Published { get; set; }
        /// <summary>
        /// 上架时间
        /// </summary>
        public DateTimeOffset? AvailableStart { get; set; }
        /// <summary>
        /// 下架时间
        /// </summary>
        public DateTimeOffset? AvailableEnd { get; set; }
        #endregion
    }
}
