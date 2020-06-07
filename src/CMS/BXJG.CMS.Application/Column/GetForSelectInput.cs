using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 获取下拉框（无论是树形的还是扁平化的）栏目时的输入模型
    /// </summary>
    public class GetForSelectInput : GeneralTreeGetForSelectInput
    { 
        /// <summary>
      /// 栏目类型
      /// </summary>
        public ColumnType? ColumnType { get; set; }
        /// <summary>
        /// 内容类型Id
        /// </summary>
        public long? ContentTypeId { get; set; }
    }
}
