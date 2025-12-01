using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using BXJG.Utils.Share;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.PSI.MasterData.Warehouse
{
    /// <summary>
    /// 仓库档案实体
    /// </summary>
    public class WarehouseEntity : FullAuditedEntity<Guid>, IMustHaveTenant, IExtendableObject, IPassivable
    {
        /// <summary>
        /// 唯一id，主键
        /// </summary>
        public override Guid Id { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public virtual string? ExtensionData { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 拼音简码
        /// </summary>
        public virtual string? Pinyin { get; set; }
        /// <summary>
        /// 是否是虚拟仓库
        /// </summary>
        public virtual bool IsVirtual { get; set; }
        /// <summary>
        /// 是否是个人仓库
        /// </summary>
        public virtual bool IsPersonal { get; set; }
        /// <summary>
        /// 所属省市区县id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 省市区县名称
        /// </summary>
        public virtual string? AreaName { get; set; }
        /// <summary>
        /// 仓库地址
        /// </summary>
        public virtual string? Address { get; set; }
        /// <summary>
        /// 地址拼音简码
        /// </summary>
        public virtual string? AddressPinyin { get; set; }
        /// <summary>
        /// 面积，㎡
        /// </summary>
        public virtual int SquareMeasure { get; set; }
        /// <summary>
        /// 体积 m³
        /// </summary>
        public virtual int Volume { get; set; }
        /// <summary>
        /// 仓库类型
        /// </summary>
        public virtual long WarehouseType { get; set; }

        /// <summary>
        /// 负责人id
        /// </summary>
        public virtual long? UserId { get; set; }
        /// <summary>
        /// 冗余存储，因为无法使用导航属性，因为abpuser是泛型的
        /// 子类可以定义这个导航属性
        /// </summary>
        public virtual string? UserName { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string? Phone { get; set; }
        /// <summary>
        /// 纬度，用于地理位置定位
        /// </summary>
        public virtual decimal? Latitude { get; set; }
        /// <summary>
        /// 经度，用于地理位置定位
        /// </summary>
        public virtual decimal? Longitude { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string? Remark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// 属于哪个组织机构id
        /// </summary>
        public long? OrganizationUnitId { get; set; }
        /// <summary>
        /// 属于哪个组织机构
        /// </summary>
        public virtual OrganizationUnit? OrganizationUnit { get; set; }
    }
}