using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 基本物品实体
    /// 它定义物品信息公共属性
    /// 它与具体的物品类型<see cref="IGoodsInfoExtensionEntity"/>双向一对一关联
    /// </summary>
    public class GoodsInfoEntity : FullAuditedAggregateRoot<long>, IExtendableObject, IMayHaveTenant
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        public string MnemonicCode { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        public string UnitId { get; set; }
        /// <summary>
        /// 关联物品类型名称，可空
        /// 比如：设备、耗材、办公用品等类型
        /// </summary>
        public string GoodsInfoExtensionType { get; set; }
        /// <summary>
        /// 关联物品id，可空
        /// 如：id为3的设备，id为10的耗材
        /// </summary>
        public long GoodsInfoExtensionId { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtensionData { get; set; }
        /// <summary>
        /// 租户id，由于是抽象的，不确定将来调用方是否需要多租户，所以可空
        /// </summary>
        public int? TenantId { get; set; }
    }
}
