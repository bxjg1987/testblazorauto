using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Entities
{
    /*
     * abp提供的IExtendableObject/ExtensionData是以json格式存储的，不便于直接对数据做查询，适合只扩展用来简单显示的字段
     * ExtFieldX也是简单的扩展方式，且支持直接对数据做查询的情况
     * 以上两种方式结合能支持大部分简单扩展需求，更复杂的扩展需求可以使用abp提供的动态属性
     * 
     */

    public class ExtensibleFullAuditedAggregateRoot<TKey> : FullAuditedAggregateRoot<TKey>, IExtendableObject
    {
        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string ExtField1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string ExtField2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string ExtField3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string ExtField4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string ExtField5 { get; set; }
        /// <summary>
        /// abp方式的扩展字段
        /// </summary>
        public string ExtensionData { get; set; }
    }
    public class ExtensibleFullAuditedEntity<TKey> : FullAuditedEntity<TKey>, IExtendableObject
    {
        /// <summary>
        /// 扩展字段1
        /// </summary>
        public string ExtField1 { get; set; }
        /// <summary>
        /// 扩展字段2
        /// </summary>
        public string ExtField2 { get; set; }
        /// <summary>
        /// 扩展字段3
        /// </summary>
        public string ExtField3 { get; set; }
        /// <summary>
        /// 扩展字段4
        /// </summary>
        public string ExtField4 { get; set; }
        /// <summary>
        /// 扩展字段5
        /// </summary>
        public string ExtField5 { get; set; }
        /// <summary>
        /// abp方式的扩展字段
        /// </summary>
        public string ExtensionData { get; set; }
    }
}
