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
    /// 你的物品类型实体应继承它
    /// </summary>
    public abstract class GoodsInfoEntity : FullAuditedAggregateRoot<long>, IGoodsInfoEntity
    {
        /// <summary>
        /// 所属分类id
        /// </summary>
        public long CategoryId { get; set; }
        /// <summary>
        /// 所属分类实体
        /// </summary>
        public virtual GoodsInfoCategoryEntity Category { get; set; }
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
        /// 所属品牌id
        /// </summary>
        public virtual string BrandId { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public virtual string ExtensionData { get; set; }
        /// <summary>
        /// 租户id，由于是抽象的，不确定将来调用方是否需要多租户，所以可空
        /// </summary>
        public virtual int? TenantId { get; set; }
    }
}
