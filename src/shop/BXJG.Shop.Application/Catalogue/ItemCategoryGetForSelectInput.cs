using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 获取下拉框（无论是树形的还是扁平化的）栏目时的输入模型
    /// </summary>
    public class ItemCategoryGetForSelectInput : GeneralTreeGetForSelectInput
    {
        /// <summary>
        /// 是否显示在首页
        /// </summary>
        public bool? ShowInHome { get; set; }
    }
}
