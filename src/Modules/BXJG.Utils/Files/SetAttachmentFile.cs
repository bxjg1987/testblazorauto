using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 设置附件的模型
    /// </summary>
    public class SetAttachmentFile
    {
        /// <summary>
        /// 文件id（附件id与文件id相同）
        /// </summary>
        public Guid? FileId { get; set; }
        /// <summary>
        /// 临时目录的相对路径
        /// </summary>
        public string? TempPath { get; set; }
        /// <summary>
        /// 文件真实名称
        /// </summary>
        public string? FileName { get; set; }
    }
}
