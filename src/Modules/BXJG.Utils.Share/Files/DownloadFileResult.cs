using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.Files
{
    /// <summary>
    /// 下载文件时的返回模型
    /// </summary>
    public class DownloadFileResult
    {
        /// <summary>
        /// 物理路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 上传时真实的文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 响应时的content-type
        /// </summary>
        public string ContentType { get; set; }
    }
}
