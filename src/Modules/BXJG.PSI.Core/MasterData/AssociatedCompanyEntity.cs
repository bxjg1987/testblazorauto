using Abp.Domain.Entities.Auditing;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BXJG.PSI.MasterData
{
    /// <summary>
    /// 往来单位实体
    /// </summary>
    [Comment("往来单位实体")]
    [Table("psi_AssociatedCompany")]
    public class AssociatedCompanyEntity : FullAuditedEntity<Guid>, IMustHaveTenant, IExtendableObject, IPassivable
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
        /// 公司名称
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.AssociatedCompanyNameMaxLength)]
        [Required]
        [Comment("公司名称")]
        public virtual string Name { get; set; }
        /// <summary>
        /// 拼音简码
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.AssociatedCompanyPinyinMaxLength)]
        [Required]
        [Comment("拼音简码")]
        public virtual string Pinyin { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        [Comment("是否启用")]
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 税号
        /// </summary>
        [MaxLength(BXJGPSICoreConsts.AssociatedCompanyTaxNoMaxLength)]
        [Comment("税号")]
        public virtual string? TaxNo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PersionNameMaxLength)]
        [Comment("联系人")]
        public virtual string? LinkMan { get; set; }
        /// <summary>
        /// 联系人拼音
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PersionNameMaxLength)]
        [Comment("联系人拼音")]
        public virtual string? LinkManPinyin { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PhoneMaxLength)]
        [Comment("联系电话")]
        public virtual string? LinkPhone { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [MaxLength(BXJGUtilsConsts.AddressMaxLength)]
        [Comment("详细地址")]
        public virtual string? Address { get; set; }
        /// <summary>
        /// 地址拼音
        /// </summary>
        [MaxLength(BXJGUtilsConsts.AddressMaxLength)]
        [Comment("地址拼音")]
        public virtual string? AddressPinyin { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Precision(18, 15)]
        [Comment("经度")]
        public virtual decimal? Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Precision(18, 15)]
        [Comment("纬度")]
        public virtual decimal? Lat { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        [Comment("所属区域")]
        public virtual long? AreaId { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        [MaxLength(BXJGUtilsConsts.AreaNameMaxLength)]
        [Comment("所属区域名称")]
        public virtual string? AreaName { get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        [Comment("负责人id")]
        public virtual long? ManagerId { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PersionNameMaxLength)]
        [Comment("负责人姓名")]
        public virtual string? ManagerName { get; set; }
        /// <summary>
        /// 客户等级Id
        /// </summary>
        [Comment("客户等级Id")]
        public virtual long? LevelId { get; set; }
        /// <summary>
        /// 客户等级名称
        /// </summary>
        [MaxLength(BXJGUtilsConsts.MaxDisplayNameLength)]
        [Comment("客户等级名称")]
        public virtual string? LevelName { get; set; }
        /// <summary>
        /// 客户等级实体
        /// </summary>
        public virtual DataDictionaryEntity Level { get; set; }
    }
}