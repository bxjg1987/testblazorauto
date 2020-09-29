using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品分类实体（树形）
    /// </summary>
    public class ProductCategoryEntity : GeneralTreeEntity<ProductCategoryEntity>
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon{ get; set; }
        /// <summary>
        /// 图片1
        /// </summary>
        public string Image1 { get; set; }
        /// <summary>
        /// 图片2
        /// </summary>
        public string Image2 { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool ShowInHome { get; set; }
    }
}
