using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 附件编辑模型
    /// </summary>
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
