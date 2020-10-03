using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXJG.Shop.Catalogue
{


    /// <summary>
    /// 后台管理商品列表页模型
    /// </summary>
    public class ProductDto : FullAuditedEntityDto<long>
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
        public string[] Images { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string Image { get { return Images?.First(); } }
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
