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
        public string RelativePath { get; set; }
        /// <summary>
        /// 缩略图的物理路径(如果有)
        /// </summary>
        public string? RelativePathThumbnail {  get; set; }
        /// <summary>
        /// 上传时真实的文件名
        /// </summary>
        public string RealFullName { get; set; }
        /// <summary>
        /// 真实的文件名 c#高级编程
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 响应时的content-type
        /// </summary>
        public string ResponseContentType { get; set; }
        /// <summary>
        /// 访问权限
        /// </summary>
        public FilePermission Permission { get; set; }
        /// <summary>
        /// 若Permission为指定权限字符串时，PermissionNames为指定权限字符串
        /// </summary>
        public string? PermissionNames { get; set; }
        //
        // 摘要:
        //     Creation time of this entity.
        public virtual DateTime CreationTime { get; set; }

        //
        // 摘要:
        //     Creator of this entity.
        public virtual long? CreatorUserId { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public int? TenantId { get; set; }
    }
}
