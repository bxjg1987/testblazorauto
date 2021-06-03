using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 上传文件的返回数据模型
    /// </summary>
    public class FileDto
    {
        [Obsolete]
        public string FilePath { get; set; }
        [Obsolete]
        public string ThumPath { get; set; }
        /// <summary>
        /// 可访问的文件绝对url
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 可访问的缩略图url
        /// </summary>
        public string ThumUrl { get; set; }
    }
}
