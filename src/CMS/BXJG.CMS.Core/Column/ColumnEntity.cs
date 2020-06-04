using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 栏目实体类
    /// </summary>
    public class ColumnEntity : GeneralTreeEntity<ColumnEntity>
    {
        public const int TitleMaxLength = 50;
        public const int IconMaxLength = 200;
        public const int SeoTitleMaxLength = 2000;
        public const int SeoDescriptionMaxLength = 5000;
        public const int SeoKeywordMaxLength = 1000;
        ///// <summary>
        ///// 标题
        ///// </summary>
        //public string Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType ColumnType { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public ContentType ContentType { get; set; }
        /// <summary>
        /// 是否是系统预定的栏目，这些栏目不允许被删除
        /// </summary>
        public bool SystemDefine { get; set; }
        /// <summary>
        /// SEO标题
        /// </summary>
        public string SeoTitle { get; set; }
        /// <summary>
        /// SEO描述信息
        /// </summary>
        public string SeoDescription { get; set; }
        /// <summary>
        /// seo关键字
        /// </summary>
        public string SeoKeyword { get; set; }
        /// <summary>
        /// 栏目描述
        /// </summary>
        public string Description { get; set; }
    }
}
