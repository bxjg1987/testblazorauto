using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品时的新增或编辑模型
    /// </summary>
    public class GoodsInfoEditDto : EntityDto<long>
    {
        /// <summary>
        /// 所属分类id
        /// </summary>
        [Required]
        public virtual long CategoryId { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        [Required]
        [StringLength(BXJGGoodsInfoCoreConsts.GoodsInfoNameMaxLength)]
        public virtual string Name { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        [StringLength(BXJGGoodsInfoCoreConsts.GoodsInfoMnemonicCodeMaxLength)]
        public virtual string MnemonicCode { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        [StringLength(BXJGGoodsInfoCoreConsts.GoodsInfoUnitIdMaxLength)]
        public virtual string UnitId { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        [StringLength(BXJGGoodsInfoCoreConsts.GoodsInfoBrandIdMaxLength)]
        public virtual string BrandId { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual Dictionary<string, object> ExtensionData { get; set; }
    }
}
