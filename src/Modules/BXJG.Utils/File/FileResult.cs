using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 文件上传、移动等操作的返回值
    /// </summary>
    public class FileResult
    {
        /// <summary>
        /// 文件的相对路径
        /// \upload\20202115\a.jpg
        /// </summary>
        public string FileRelativePath { get; set; }
        /// <summary>
        /// 文件的绝对路径
        /// d:\app\wwwroot\upload\20202115\a.jpg
        /// </summary>
        public string FileAbsolutePath { get; set; }
        /// <summary>
        /// 缩略图的相对路径
        /// \upload\20202115\athum.jpg
        /// </summary>
        public string ThumRelativePath { get; set; }
        /// <summary>
        /// 缩略图的绝对路径
        /// d:\app\wwwroot\upload\20202115\athum.jpg
        /// </summary>
        public string ThumAbsolutePath { get; set; }
    }
}
