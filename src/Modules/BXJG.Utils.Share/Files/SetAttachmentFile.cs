using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 设置附件的模型
    /// </summary>
    public class SetAttachmentFile
    {
        /// <summary>
        /// 文件id
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
        /// <summary>
        /// 前端忽略此参数
        /// 具体应用将对此字段赋值
        /// </summary>
        public FilePermission Permission { get; set; } = FilePermission.Further;
    }
}
