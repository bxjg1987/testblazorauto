using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 文件上传的返回值
    /// </summary>
    public class FileResult
    {
        /// <summary>
        /// 文件的相对路径
        /// \temp\guid.jpg
        /// </summary>
        public string FileRelativePath { get; set; }
        /// <summary>
        /// 文件名称，如：c#高级编程
        /// </summary>
        public string FileName { get; set; }
        ///// <summary>
        ///// 文件的绝对路径
        ///// d:\app\wwwroot\upload\20202115\a.jpg
        ///// </summary>
        //public string FileAbsolutePath { get; set; }
        ///// <summary>
        ///// 缩略图的相对路径
        ///// \upload\20202115\athum.jpg
        ///// </summary>
        //public string ThumRelativePath { get; set; }
        ///// <summary>
        ///// 缩略图的绝对路径
        ///// d:\app\wwwroot\upload\20202115\athum.jpg
        ///// </summary>
        //public string ThumAbsolutePath { get; set; }
        ///// <summary>
        ///// 可访问的文件url，http://xx.xx/upload/temp/xx.jpg
        ///// </summary>
        //public string FileUrl { get; set; }
        ///// <summary>
        ///// 可访问的缩略图url，http://xx.xx/upload/temp/xxthum.jpg
        ///// </summary>
        //public string ThumUrl { get; set; }
    }
}
