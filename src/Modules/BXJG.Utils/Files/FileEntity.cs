
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Utils.Share;
using BXJG.Utils.Share.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 文件实体
    /// </summary>
    public class FileEntity : FullAuditedEntity<Guid>, IExtendableObject, IMayHaveTenant
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 真实的文件名 c#高级编程
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// .jpg
        /// </summary>
        public string Ext { get; set; }
        /// <summary>
        /// c#高级编程.jpg
        /// </summary>
        public string RealFullName => $"{RealName}{Ext}";
        /// <summary>
        /// 响应的文件类型，mime
        /// </summary>
        public string ResponseContentType { get; set; }
        /// <summary>
        /// 大小字节
        /// </summary>
        public long Size { get; set; } = 0;
        /// <summary>
        /// 相对于文件存储目录的 相对路径
        /// 别用web根，因为文件下载需要权限
        /// 别用内容根，因为文件存储在任何目录更灵活
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 缩略图相对路径
        /// </summary>
        public string? RelativePathThumbnail { get; set; }
        /// <summary>
        /// 通用的文件权限，统一文件访问接口判断此属性，
        /// 具体业务独立的文件访问接口葫芦此属性
        /// 由于这是通用权限，所以定义在这里而不是附件实体上
        /// </summary>
        public FilePermission Permission { get; set; } = FilePermission.Further;
        /// <summary>
        /// 扩展数据
        /// </summary>
        public string? ExtensionData { get; set; }
        ///// <summary>
        ///// 文件状态
        ///// </summary>
        //public FileStatus Status { get; set; }
        ///// <summary>
        ///// 访问文件时，当前用户必须拥有这里这定义权限中的任意一个
        ///// 为空则不限制
        ///// </summary>
        //public virtual ICollection<AttachmentPermissionEntity> Permissions { get; set; }
    }

    //public static class FileExt
    //{
    //    Func<string> 
    //}
}