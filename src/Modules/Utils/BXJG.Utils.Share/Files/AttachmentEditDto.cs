using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 附件编辑模型
    /// </summary>
    [Obsolete("看样子好像是没用了，如果后续发现有用就去掉这个标签")]
    public class AttachmentEditDto
    {
        /// <summary>
        /// 文件绝对url
        /// </summary>
        [Required]
        public string AbsoluteFileUrl { get; set; }
        /// <summary>
        /// 附件扩展属性
        /// </summary>
        public IDictionary<string, object> ExtensionData { get; set; }
        /// <summary>
        /// 排序索引，若赋值则按此顺序，否则按提交顺序
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
