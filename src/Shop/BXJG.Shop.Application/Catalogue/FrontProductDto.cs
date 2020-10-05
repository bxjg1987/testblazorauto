using Abp.Application.Services.Dto;
using BXJG.Utils.Extensions;
using BXJG.Utils.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 显示给顾客的商品信息
    /// </summary>
    public class FrontProductDto : EntityDto<long>
    {
        #region 基本信息
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        public string DescriptionShort { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public string DescriptionFull { get; set; }
        /// <summary>
        /// 轮播图片集合，多个用英文逗号,分割
        /// </summary>
        public List<FileDto> Images { get; set; }
        /// <summary>
        /// 封面原始图片
        /// </summary>
        public string Image { get { return Images?.First().FilePath; } }
        /// <summary>
        /// 封面缩略图图片
        /// </summary>
        public string ImageThum { get { return Images?.First().ThumPath; } }
        /// <summary>
        /// 所属类别id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryDisplayName { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        public long? BrandId { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandDisplayName { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitDisplayName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public long? UnitId { get; set; }
        #endregion

        #region 价格信息
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
        ///// <summary>
        ///// 是否已发布
        ///// </summary>
        //public bool Published { get; set; }
        ///// <summary>
        ///// 上架时间
        ///// </summary>
        //public DateTimeOffset? AvailableStart { get; set; }
        ///// <summary>
        ///// 下架时间
        ///// </summary>
        //public DateTimeOffset? AvailableEnd { get; set; }
        #endregion

    }
}