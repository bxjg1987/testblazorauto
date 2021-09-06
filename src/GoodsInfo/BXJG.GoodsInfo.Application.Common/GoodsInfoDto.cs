using Abp.Application.Services.Dto;
using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.Application.Common
{
    /// <summary>
    /// 后台管理物品的显示模型
    /// </summary>
    public class GoodsInfoDto : EntityDto<long>, IExtendableDto
    {
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
        /// 扩展数据
        /// </summary>
        public dynamic ExtensionData { get; set; }
    }
}
