using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo.GoodsInfo
{
    /// <summary>
    /// 物品实体接口
    /// </summary>
    public interface IGoodsInfoExtensionEntity : IEntity<long>, IExtendableObject, IMayHaveTenant 
    {
        /// <summary>
        /// 关联的物品信息id
        /// </summary>
        long GoodsInfoId { get; set; }
        /// <summary>
        /// 关联的物品信息
        /// </summary>
        GoodsInfoEntity GoodsInfo { get; set; }
    }

    public abstract class GoodsInfoExtensionEntity : Entity<long>, IGoodsInfoExtensionEntity
    {
        /// <summary>
        /// 关联的物品信息id
        /// </summary>
        public virtual long GoodsInfoId { get; set; }
        /// <summary>
        /// 关联的物品信息
        /// </summary>
        public virtual GoodsInfoEntity GoodsInfo { get; set; }
        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ExtensionData { get; set; }
        /// <summary>
        /// 租户id，由于是抽象的，不确定将来调用方是否需要多租户，所以可空
        /// 虽然GoodsInfo里已经有了，但是考虑将来直接处理数据时更容易，这里也存储下租户id
        /// </summary>
        public int? TenantId { get; set; }
    }
}
