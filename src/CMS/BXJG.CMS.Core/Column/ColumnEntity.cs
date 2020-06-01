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

        public string Title { get; set; }

        public string Icon { get; set; }

        public string SeoTitle { get; set; }

        public string SeoDescription { get; set; }

        public string SeoKeyword { get; set; }

        public string Description { get; set; }
    }
}
