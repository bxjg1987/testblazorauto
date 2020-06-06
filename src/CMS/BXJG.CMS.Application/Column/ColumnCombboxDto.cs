using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 扁平化的栏目下拉框数据模型
    /// </summary>
    public class ColumnCombboxDto : GeneralTreeComboboxDto
    {
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType ColumnType { get; set; }
        /// <summary>
        /// 内容类型Id
        /// </summary>
        public long ContentTypeId { get; set; }
    }
}
