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
        public string Icon { get; set; }
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
        ///// <summary>
        ///// 为何注释掉，请看ProductCategoryDynamicPropertyEntity的注释
        ///// 允许当前商品分类下的商品 有哪些sku的动态属性，如：手机类别下允许有 入网类型、版本(4g*64g)、颜色等属性
        ///// </summary>
        //public virtual List<ProductCategoryDynamicPropertyEntity> DynamicProperties { get; set; }
    }
}
