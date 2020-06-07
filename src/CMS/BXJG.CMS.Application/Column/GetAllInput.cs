using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.CMS.Column
{
    /// <summary>
    /// 后台管理栏目时，获取列表页面数据时的输入模型
    /// </summary>
    public class GetAllInput : GeneralTreeGetTreeInput
    {
        /// <summary>
        /// 栏目类型
        /// </summary>
        public ColumnType? ColumnType { get; set; }
        /// <summary>
        /// 内容类型Id
        /// </summary>
        public long? ContentTypeId { get; set; }
        /// <summary>
        /// 时候是系统预定义的栏目
        /// </summary>
        public bool? IsSysDefine { get; set; }
        /// <summary>
        /// 关键字 标题、seo信息，模板名等...
        /// </summary>
        public string Keywords { get; set; }
    }
}
