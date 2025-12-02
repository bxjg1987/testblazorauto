using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.PSI.MasterData.Product
{
    /// <summary>
    /// 商品档案实体
    /// </summary>
    public  class ProductEntity : FullAuditedEntity<string>, IMustHaveTenant, IExtendableObject, IPassivable
    {
        /// <summary>
        /// 唯一id，主键
        /// </summary>
        public override string Id { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public virtual string? ExtensionData { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 商品名称拼音简码
        /// </summary>
        public virtual string? Pinyin { get; set; }
        /// <summary>
        /// 品牌Id
        /// </summary>
        public virtual long? BrandId { get; set; }
        /// <summary>
        /// 品牌导航属性
        /// </summary>
        public virtual DataDictionaryEntity Brand { get; set; }
       
        ///// <summary>
        ///// 品牌名称
        ///// </summary>
        //public virtual string? BrandName { get; set; }
        
        /// <summary>
        /// 商品规格型号
        /// </summary>
        public virtual string? Model { get; set; }
        /// <summary>
        /// 是否是虚拟产品
        /// </summary>
        public virtual bool IsVirtual { get; set; } = false;
        /// <summary>
        /// 商品类别id
        /// </summary>
        public virtual long? CategoryId { get; set; }
        /// <summary>
        /// 商品类别
        /// </summary>
        public virtual ProductCategoryEntity Category { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        public virtual string Unit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 所属组织机构id
        /// </summary>
        public long? OrganizationUnitId { get; set; }
        /// <summary>
        /// 所属组织机构
        /// </summary>
        public virtual OrganizationUnit? OrganizationUnit { get; set; }
    }
}