using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using System.Collections.Generic;

namespace BXJG.Shop.Catalogue
{
    /// <summary>
    /// 商品定义
    /// </summary>
    public class ItemEntity<TTree> : FullAuditedEntity<long>, IMustHaveTenant
        where TTree: GeneralTreeEntity
    {
        public int TenantId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 简短描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 轮播图片集合，多个用英文逗号,分割
        /// </summary>
        public string Images { get; set; }

    }
}
