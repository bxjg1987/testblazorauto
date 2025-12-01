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
    /// 附件编辑模型
    /// </summary>
    public class AttachmentEditDto//: EntityDto<Guid>
    {
        /// <summary>
        /// 文件url，相对路径或可访问路径
        /// 例：/upload/xxx.xx或http://xx.xx/upload/xxx.xx
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, object> ExtensionData { get; set; }
        /// <summary>
        /// 排序索引
        /// </summary>
        public int OrderIndex { get; set; }
    }
}
