using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Share.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Files
{
    /// <summary>
    /// 通用文件dto
    /// 目前考虑是后续的独立文件管理模块使用的dto
    /// </summary>
    public class FileDto : FullAuditedEntityDto<Guid>, IExtendableObj
    {
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
        public string RealFullName { get; set; }
        /// <summary>
        /// 响应的文件类型，mime
        /// </summary>
        public string ResponseContentType { get; set; }
        /// <summary>
        /// 大小字节
        /// </summary>
        public long Size { get; set; }
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
        ///// <summary>
        ///// 扩展数据
        ///// </summary>
        //public string? ExtensionData { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public dynamic? ExtensionData { get; set; }
    }
}
