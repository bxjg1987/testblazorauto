using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 通用的附件实体
    /// 任何实体都可以通过它来实现附件、图片功能，它与实体之间是弱引用关系
    /// </summary>
    public class AttachmentEntity : Entity<Guid>, IMustHaveTenant, IExtendableObject
    {
        /// <summary>
        /// 关联实体类型.FullTypeName
        /// </summary>
        public string EntityType { get; set; }
        /// <summary>
        /// 关联实体id
        /// </summary>
        public string EntityId { get; set; }
        /// <summary>
        /// 文件url，相对url
        /// </summary>
        public string RelativeFileUrl { get; set; }
        /// <summary>
        /// 缩略图url，相对url
        /// </summary>
        public string RelativeThumUrl { get; set; }
        /// <summary>
        /// 文件url，可访问的url
        /// </summary>
        public string AbsoluteFileUrl { get; set; }
        /// <summary>
        /// 缩略图url，可访问的url
        /// </summary>
        public string AbsoluteThumUrl { get; set; }
        /// <summary>
        /// 顺序索引
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public string ExtensionData { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
    }
}
