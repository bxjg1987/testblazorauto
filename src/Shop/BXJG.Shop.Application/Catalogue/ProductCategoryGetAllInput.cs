using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 后台管理商品分类时，获取列表页面数据时的输入模型
    /// </summary>
    public class ProductCategoryGetAllInput : GeneralTreeGetTreeInput
    {
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool? ShowInHome { get; set; }
    }
}
