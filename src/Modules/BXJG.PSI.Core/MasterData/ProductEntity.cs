using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BXJG.PSI.MasterData
{
    /// <summary>
    /// 商品档案实体
    /// </summary>
    [Comment("商品档案实体")]
    [Table("psi_Product")]
    public abstract class ProductEntity : FullAuditedEntity<Guid>, IMustHaveTenant, IExtendableObject, IPassivable
    {
        /// <summary>
        /// 唯一id，主键
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("唯一id，主键")]
        public override Guid Id { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        [Comment("租户id")]
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        [MaxLength(BXJGUtilsConsts.ExtDataMaxLength)]
        [Comment("扩展字段")]
        public virtual string? ExtensionData { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.ProductNameMaxLength)]
        [Required]
        [Comment("商品名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        [MaxLength(BXJGUtilsConsts.MaxCodeLength)]
        [Required]
        [Comment("商品编码")]
        public virtual string Code { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.ProductSpecMaxLength)]
        [Comment("商品规格")]
        public virtual string? Spec { get; set; }
        /// <summary>
        /// 商品型号
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.ProductModelMaxLength)]
        [Comment("商品型号")]
        public virtual string? Model { get; set; }
        /// <summary>
        /// 商品类别id
        /// </summary>
        [Comment("商品类别id")]
        public long? CategoryId { get; set; }
        /// <summary>
        /// 商品类别名称
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.ProductCategoryNameMaxLength)]
        [Comment("商品类别名称")]
        public virtual string? CategoryName { get; set; }
        /// <summary>
        /// 计量单位
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.ProductUnitMaxLength)]
        [Required]
        [Comment("计量单位")]
        public virtual string Unit { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>
        [Precision(18, 4)]
        [Comment("成本价")]
        public virtual decimal? CostPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        [Precision(18, 4)]
        [Comment("售价")]
        public virtual decimal? SalePrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(BXJGUtilsConsts.RemarkMaxLength)]
        [Comment("备注")]
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Comment("是否启用")]
        public bool IsActive { get; set; }
        /// <summary>
        /// 所属组织机构id
        /// </summary>
        [Comment("所属组织机构id")]
        public long? OrganizationUnitId { get; set; }
        /// <summary>
        /// 所属组织机构
        /// </summary>
        [Comment("所属组织机构")]
        public virtual OrganizationUnit OrganizationUnit { get; set; }
    }
}