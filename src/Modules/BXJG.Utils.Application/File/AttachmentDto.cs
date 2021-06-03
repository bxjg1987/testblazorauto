using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.File
{
    /// <summary>
    /// 通用显示模型
    /// </summary>
    public class AttachmentDto : EntityDto<Guid>
    {
        /// <summary>
        /// 关联实体id
        /// </summary>
        public string EntityId { get; set; }
        /// <summary>
        /// 文件url，可访问路径
        /// 例：http://xxx.xxx/upload/xxx.xx
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 缩略图url，可访问路径
        /// 例：http://xxx.xxx/upload/xxxthum.xx
        /// </summary>
        public string ThumUrl { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string,object> ExtensionData { get; set; }
    }
}
