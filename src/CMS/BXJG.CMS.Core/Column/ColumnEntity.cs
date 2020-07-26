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
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType ColumnType { get; set; }

        //将来可能需要做一个dll实现新的内容类型，然后注册到主模块中，因此内容类型如果用枚举就不太方便了

        /// <summary>
        /// 内容类型Id
        /// </summary>
        public long ContentTypeId { get; set; }
        /// <summary>
        /// 内容类型
        /// </summary>
        public virtual GeneralTreeEntity ContentType { get; set; }
        /// <summary>
        /// 是否是系统预定的栏目，这些栏目不允许被删除
        /// </summary>
        public bool IsSysDefine { get; set; }
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
        /// <summary>
        /// 列表页模板
        /// </summary>
        public string ListTemplate { get; set; }
        /// <summary>
        /// 详情页模板
        /// </summary>
        public string DetailTemplate { get; set; }
    }
}
