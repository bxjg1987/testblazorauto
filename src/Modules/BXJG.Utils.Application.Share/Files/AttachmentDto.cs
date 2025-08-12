using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Files
{
    /// <summary>
    /// 通用的获取实体附件的模型
    /// 后台管理端和其它引用的地方目前统一使用这个模型
    /// </summary>
    public class AttachmentDto : FullAuditedEntityDto<Guid>, IExtendableObj
    {
        /// <summary>
        /// 关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName
        /// </summary>
        public required string EntityType { get; set; }
        /// <summary>
        /// 关联实体id
        /// </summary>
        public required string EntityId { get; set; }
        /// <summary>
        /// 属性名，可空
        /// 比如工单：字段A表示要处理的问题相关图片，字段B表示处理完成时拍摄的图片，它们都使用附件表，当通过此字段来表示关联的不同的属性
        /// </summary>
        public string? PropertyName { get; set; }
        ///// <summary>
        ///// 文件url，可访问路径
        ///// 例：http://xxx.xxx/upload/xxx.xx
        ///// </summary>
        //public string AbsoluteFileUrl { get; set; }
        ///// <summary>
        ///// 缩略图url，可访问路径
        ///// 例：http://xxx.xxx/upload/xxxthum.xx
        ///// </summary>
        //public string AbsoluteThumUrl { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public dynamic ExtensionData { get; set; }
        /// <summary>
        /// 文件id
        /// </summary>
        public Guid FileId { get; set; }
        public FileDto File { get; set; }
    }
}
