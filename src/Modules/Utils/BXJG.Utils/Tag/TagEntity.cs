using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Tag
{
    /// <summary>
    /// 实体标签
    /// </summary>
    public class TagEntity : FullAuditedEntity<Guid>, IMayHaveTenant, IExtendableObject, IOrderIndex
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int? TenantId { get; set; }
        /// <summary>
        /// 关联实体类型，可以是任意唯一字符串，通常是实体类型.FullTypeName
        /// </summary>
        public string EntityType { get; set; }
        /// <summary>
        /// 关联实体id
        /// </summary>
        public string EntityId { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }
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
        /// <summary>
        /// json格式的扩展数据，配合abp的扩展管理器
        /// </summary>
        public string? ExtensionData { get; set; }
        ///// <summary>
        ///// 普通的扩展字段1
        ///// </summary>
        //public string? ExtField1 { get; set; }
        //// <summary>
        ///// 普通的扩展字段2
        ///// </summary>
        //public string? ExtField2 { get; set; }
    }
}