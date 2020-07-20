using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BXJG.Equipment.EquipmentInfo
{
    public class EquipmentInfoEntity<TDataDictionary> : FullAuditedEntity<long>, IMustHaveTenant
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
    /// <summary>
    /// 设备信息定义的实体，
    /// 关联的数据字典类型为GeneralTreeEntity，若需要关联到模块调用方自定义的字典时请自定义类并继承此类的泛型版本
    /// </summary>
    public class EquipmentInfoEntity : EquipmentInfoEntity<GeneralTreeEntity> { }
}
