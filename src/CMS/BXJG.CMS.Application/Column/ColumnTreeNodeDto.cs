using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 获取栏目树形下拉框的数据模型
    /// </summary>
    public class ColumnTreeNodeDto : GeneralTreeNodeDto<ColumnTreeNodeDto>
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
