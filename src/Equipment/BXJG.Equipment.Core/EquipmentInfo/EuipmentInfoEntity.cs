using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;
using ZLJ.BaseInfo.Administrative;

namespace BXJG.Equipment.EquipmentInfo
{
    /// <summary>
    /// 设备档案实体类
    /// </summary>
    public class EquipmentInfoEntity : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 硬件码
        /// </summary>
        public string HardwareCode { get; set; }
        /// <summary>
        /// 所属区域Id
        /// </summary>
        public long? AreaId { get; set; }
        /// <summary>
        /// 所属区域实体
        /// </summary>
        public virtual AdministrativeEntity Area { get; set; }
        ///// <summary>
        ///// 助记码
        ///// </summary>
        //public string MnemonicCode { get; set; }
        ///// <summary>
        ///// 分类Id
        ///// </summary>
        //public long CategoryId { get; set; }
        ///// <summary>
        ///// 设备分类导航属性
        ///// </summary>
        //public virtual BaseInfo.DataDictionaryEntity Category { get; set; }
        ///// <summary>
        ///// 单位Id
        ///// </summary>
        //public long? UnitId { get; set; }
        ///// <summary>
        ///// 单位导航属性
        ///// </summary>
        //public virtual BaseInfo.DataDictionaryEntity Unit { get; set; }
        ///// <summary>
        ///// 品牌Id
        ///// </summary>
        //public long? BrandId { get; set; }
        ///// <summary>
        ///// 品牌导航属性
        ///// </summary>
        //public virtual BaseInfo.DataDictionaryEntity Brand { get; set; }
        ///// <summary>
        ///// 规格型号
        ///// </summary>
        //[Column(TypeName = "nvarchar")]
        //[StringLength(SizeMaxLength)]
        //public string Size { get; set; }
        //附件是一个通用功能，与实体是弱引用关系
        ///// <summary>
        ///// 附件列表
        ///// </summary>
        //public virtual IList<AttachmentEntity> Attachments { get; set; }
        // /// <summary>
        // /// 默认供应商Id
        // /// </summary>
        // public long? SupplierId { get; set; }
        //// [ForeignKey("BtypeId")]
        // public virtual BaseInfo.BtypeEntity Supplier { get; set; }
        ///// <summary>
        ///// 默认进货价格
        ///// </summary>
        //public decimal PurchasePrice { get; set; }
        ///// <summary>
        ///// 默认售价
        ///// </summary>
        //public decimal SellPrice { get; set; }
    }
}
