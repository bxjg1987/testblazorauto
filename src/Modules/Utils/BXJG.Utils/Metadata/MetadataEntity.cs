using BXJG.Utils.GeneralTree;
using BXJG.Utils.Share;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Metadata
{
    /// <summary>
    /// 元数据
    /// </summary>
    [Table("BXJGUtilsMetadata")]
    public class MetadataEntity : GeneralTreeNoTenantEntity<MetadataEntity>
    {
        //默认都是系统预设的


        //实现这个需求，用Name属性也行
        ///// <summary>
        ///// 业务类型
        ///// 比如采购退货出库和采购订单 他们对应的业务类型都是 采购
        ///// 之所以用不同的显示名称，是为了用户友好
        ///// 界面上可以根据这个业务类型来决定显示意义的ui 比如 采购订单选择
        ///// 与Name不同，Name是用来做数据过滤的
        ///// </summary>
        //[MaxLength(BXJGUtilsConsts.BusinessTypeMaxLength)]
        //[Unicode(false)]
        //public string? BusinessType { get; set; }


        /// <summary>
        /// 指定的实体类型才会做数据权限控制
        /// </summary>
        [MaxLength(BXJGUtilsConsts.EntityTypeMaxLength)]
        [Unicode(false)]
        [Required]
        [Comment("指定的实体类型才会做数据权限控制")]
        public string? EntityTypeFullName { get; set; }
    }
}