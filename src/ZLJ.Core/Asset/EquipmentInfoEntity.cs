
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using BXJG.GeneralTree;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using ZLJ.ABPFile;

namespace ZLJ.Asset
{
    /// <summary>
    /// 设备档案
    /// </summary>
    [Table("ZLJEquipmentInfos")]
    public class EquipmentInfoEntity : FullAuditedEntity<long>, IMustHaveTenant
    {
        public const int CodeMaxLength = 32;
        public const int SizeMaxLength = 200;
        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        //[Column(TypeName ="varchar")]
        [StringLength(CodeMaxLength)]
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// 分类Id
        /// </summary>
        public long AreaId { get; set; }
        /// <summary>
        /// 设备分类导航属性
        /// </summary>
        public virtual GeneralTreeEntity Area { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [StringLength(SizeMaxLength)]
        public string Size { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        [NotMapped]
        public double Distance { get; set; }
    }
}
