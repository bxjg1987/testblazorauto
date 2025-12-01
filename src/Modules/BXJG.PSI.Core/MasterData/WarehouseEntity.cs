using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BXJG.PSI.MasterData
{
    /// <summary>
    /// 仓库档案实体
    /// </summary>
    [Comment("仓库档案实体")]
    [Table("psi_Warehouse")]
    public class WarehouseEntity : FullAuditedEntity<Guid>, IMustHaveTenant, IExtendableObject, IPassivable
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
        //[Unicode(true)]//ef默认就是Unicode
        [Comment("扩展字段")]
        public virtual string? ExtensionData { get; set; }
        /// <summary>
    /// 仓库名称
    /// </summary>
    [MaxLength(BXJGPSICoreConsts.WarehouseNameMaxLength)]
    [Required]
    [Comment("仓库名称")]
    public virtual string Name { get; set; }
    /// <summary>
    /// 拼音简码
    /// </summary>
    [MaxLength(BXJGUtilsConsts.MaxCodeLength)]
    [Comment("拼音简码")]
    public virtual string? PinYin { get; set; }
    /// <summary>
    /// 是否是虚拟仓库
    /// </summary>
    [Comment("是否是虚拟仓库")]
    public virtual bool IsVirtual { get; set; }
    /// <summary>
    /// 是否是个人仓库
    /// </summary>
    [Comment("是否是个人仓库")]
    public virtual bool IsPersonal { get; set; }
    /// <summary>
    /// 所属省市区县id
    /// </summary>
    [Comment("所属省市区县id")]
    public long? AreaId { get; set; }
        /// <summary>
        /// 省市区县名称
        /// </summary>
        [MaxLength(BXJGUtilsConsts.AreaNameMaxLength)]
        [Comment("省市区县名称")]
        public virtual string? AreaName { get; set; }
        /// <summary>
    /// 仓库地址
    /// </summary>
    [MaxLength(BXJGUtilsConsts.AddressMaxLength)]
    //[Unicode(true)]
    [Comment("仓库地址")]
    public virtual string? Address { get; set; }
    /// <summary>
    /// 地址拼音简码
    /// </summary>
    [MaxLength(BXJGUtilsConsts.MaxCodeLength)]
    [Comment("地址拼音简码")]
    public virtual string? AddressPinYin { get; set; }
    /// <summary>
    /// 面积，㎡
    /// </summary>
    [Comment("面积，㎡")]
    public virtual int SquareMeasure { get; set; }
        /// <summary>
        /// 体积 m³
        /// </summary>
        [Comment("体积 m³")]
        public virtual int Volume { get; set; }
        /// <summary>
        /// 仓库类型
        /// 便于调用方灵活规定，这里就用int类型，而不是枚举
        /// 可能当前模块会规定一些预设的仓库类型，为了避免冲突，预设的数值要用一些不常用的数值，如：2545621
        /// 使用long类型 可以尽量避免冲突，还可以考虑使用位运算的值
        /// 或者预设的仓库用单独的字段，这样此模块调用方更灵活
        /// </summary>
        [Comment("仓库类型")]
        public virtual long WarehouseType { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        [Comment("负责人id")]
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 冗余存储，因为无法使用导航属性，因为abpuser是泛型的
        /// 子类可以定义这个导航属性
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PersionNameMaxLength)]
        [Comment("负责人姓名")]
        public virtual string? UserName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(BXJGUtilsConsts.PhoneMaxLength)]
        [Comment("联系电话")]
        public virtual string? Phone { get; set; }
        /// <summary>
        /// 纬度，用于地理位置定位
        /// </summary>
        [Precision(18, 15)]
        [Comment("纬度，用于地理位置定位")]
        public virtual decimal? Latitude { get; set; }
        /// <summary>
        /// 经度，用于地理位置定位
        /// </summary>
        [Precision(18, 15)]
        [Comment("经度，用于地理位置定位")]
        public virtual decimal? Longitude { get; set; }
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
        /// 属于哪个组织机构id
        /// </summary>
        [Comment("所属组织机构id")]
        public long? OrganizationUnitId { get; set; }
        /// <summary>
        /// 属于哪个组织机构
        /// </summary>
        [Comment("所属组织机构")]
        public virtual OrganizationUnit OrganizationUnit { get; set; }
    }
}
