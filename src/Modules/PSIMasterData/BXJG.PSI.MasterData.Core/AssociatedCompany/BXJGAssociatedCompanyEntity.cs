using Abp.Domain.Entities.Auditing;
using BXJG.Utils.GeneralTree;
using BXJG.Utils.Share;
using System;

namespace BXJG.PSI.MasterData.AssociatedCompany
{
    /// <summary>
    /// 往来单位实体
    /// </summary>
    public class BXJGAssociatedCompanyEntity : FullAuditedEntity<long>, IMustHaveTenant, IExtendableObject, IPassivable
    {
        /// <summary>
        /// 唯一id，主键
        /// </summary>
        public override long Id { get; set; }
        /// <summary>
        /// 租户id
        /// </summary>
        public virtual int TenantId { get; set; }
        /// <summary>
        /// 扩展字段
        /// </summary>
        public virtual string? ExtensionData { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 拼音简码
        /// </summary>
        public virtual string? Pinyin { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsActive { get; set; } = true;
        /// <summary>
        /// 税号
        /// </summary>
        public virtual string? TaxNo { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string? LinkMan { get; set; }
        /// <summary>
        /// 联系人拼音
        /// </summary>
        public virtual string? LinkManPinyin { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string? LinkPhone { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public virtual string? Address { get; set; }
        /// <summary>
        /// 地址拼音
        /// </summary>
        public virtual string? AddressPinyin { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public virtual decimal? Lng { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public virtual decimal? Lat { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public virtual long? AreaId { get; set; }
        /// <summary>
        /// 所属区域名称
        /// </summary>
        public virtual string? AreaName { get; set; }
        /// <summary>
        /// 负责人id
        /// </summary>
        public virtual long? ManagerId { get; set; }
        /// <summary>
        /// 负责人姓名
        /// </summary>
        public virtual string? ManagerName { get; set; }
        /// <summary>
        /// 客户等级Id
        /// </summary>
        public virtual long? LevelId { get; set; }
        /// <summary>
        /// 客户等级名称
        /// </summary>
        public virtual string? LevelName { get; set; }
        /// <summary>
        /// 客户等级实体
        /// </summary>
        public virtual DataDictionaryEntity? Level { get; set; }
    }
}