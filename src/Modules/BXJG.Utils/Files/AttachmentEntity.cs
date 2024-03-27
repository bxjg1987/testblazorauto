using Abp.Domain.Entities;
using BXJG.Common.Contracts;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Files
{
    /// <summary>
    /// 通用的附件实体
    /// 任何实体都可以通过它来实现附件、图片功能，它与实体之间是弱引用关系
    /// 使用冗余数据方式关联单个文件，避免join查询
    /// 它与附件权限是弱关系
    /// </summary>
    public class AttachmentEntity : Entity<Guid>, IMayHaveTenant/*, IExtendableObject*/,IOrderIndex
    {
        /// <summary>
        /// 关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName
        /// </summary>
        public string EntityType { get; set; }
        /// <summary>
        /// 关联实体id
        /// </summary>
        public string EntityId { get; set; }
        /// <summary>
        /// 属性名，可空
        /// 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性
        /// </summary>
        public string? PropertyName { get; set; }
        ///// <summary>
        ///// 关联的文件id，直接用文件id
        ///// </summary>
        //public Guid FileId { get; set; }
        /// <summary>
        /// 关联的文件实体
        /// </summary>
        public virtual FileEntity File { get; set; }
        ///// <summary>
        ///// 文件url，相对url
        ///// </summary>
        //public string RelativeFileUrl { get; set; }
        ///// <summary>
        ///// 缩略图url，相对url
        ///// </summary>
        //public string RelativeThumUrl { get; set; }
        ///// <summary>
        ///// 文件url，可访问的url
        ///// </summary>
        //public string AbsoluteFileUrl { get; set; }
        ///// <summary>
        ///// 缩略图url，可访问的url
        ///// </summary>
        //public string AbsoluteThumUrl { get; set; }
        /// <summary>
        /// 顺序索引
        /// </summary>
        public int OrderIndex { get; set; }
        ///// <summary>
        ///// 扩展数据
        ///// 附件跟文件是一对一关系，所以扩展属性这里没必要，直接放文件实体上即可
        ///// </summary>
        //public string ExtensionData { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public int? TenantId { get; set; }
    }
}
