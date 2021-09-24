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
        /// 属性名
        /// 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性
        /// 此字段是新加的，以前的功能此字段可能为空。现在你使用时必须添加此字段
        /// </summary>
        public string PropertyName { get; set; }
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
