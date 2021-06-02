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
    public class EntityFileEntity : Entity<Guid>, IMustHaveTenant, IExtendableObject
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
        /// 文件url，相对路径
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 缩略图url，相对路径
        /// </summary>
        public string ThumUrl { get; set; }


        public string ExtensionData { get; set; }
        public int TenantId { get; set; }
    }
}
