using Abp.Application.Services.Dto;
using BXJG.Common.Contracts;
using BXJG.Utils.Share.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Application.Share.Tag
{
    public class TagDto : FullAuditedEntityDto<Guid>, IExtendableObj
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
        /// 属性名，可空 比如工单：字段A表示要处理的问题相关tag，字段B表示处理完成时拍摄的tag，它们都使用tag表，当通过此字段来表示关联的不同的属性
        /// </summary>
        public string? PropertyName { get; set; }
        /// <summary>
        /// 属性显示名，在存储时若为空则复制PropertyName
        /// </summary>
        public string? PropertyDisplayName { get; set; }
        /// <summary>
        /// 标签名称、同一个实体的同一个属性下必须唯一
        /// </summary>
        public string TagName { get; set; }
        /// <summary>
        /// 标签显示名
        /// </summary>
        public string? TagDisplayName { get; set; }
        /// <summary>
        /// 顺序索引
        /// 注意TagDto中的排序是为了可选tag列表用的，这里的排序是实体已经设置多个tag时的排序，这里的排序其实没多大意义
        /// </summary>
        public int OrderIndex { get; set; }
        public dynamic ExtensionData { get; set; }
        /// <summary>
        /// 是否已选择
        /// </summary>
        public bool IsSelected { get; set; }

        public static TagDto Map(string entityId, string entityType, string propertyName, SelectableTagDto x, string propertyDisplayName=default)
        {
            return new TagDto
            {
                EntityId = entityId, 
                TagName = x.TagName,
                TagDisplayName = x.TagDisplayName,
                PropertyName = propertyName,
                EntityType = entityType,
                OrderIndex = x.OrderIndex, 
                //PropertyDisplayName = x.PropertyDisplayName
            };
        }
    }
}
