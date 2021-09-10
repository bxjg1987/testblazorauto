using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Admin
{
    /// <summary>
    /// 后台管理物品的显示模型
    /// </summary>
    public class GoodsInfoDto : FullAuditedEntityDto<long>, IExtendableDto
    {
        /// <summary>
        /// 所属分类id
        /// </summary>
        public virtual long CategoryId { get; set; }
        /// <summary>
        /// 所属分类名称
        /// </summary>
        public virtual long CategoryDisplayName { get; set; }
        /// <summary>
        /// 物品名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        public virtual string MnemonicCode { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        public virtual string UnitId { get; set; }
        /// <summary>
        /// 品牌id
        /// </summary>
        public virtual string BrandId { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        public virtual dynamic ExtensionData { get; set; }
    }
}
