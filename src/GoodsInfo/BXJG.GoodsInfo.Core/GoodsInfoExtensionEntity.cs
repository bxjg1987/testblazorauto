using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 具体物品实体
    /// 比如：整机、配件、耗材、办公用品
    /// 它与GoodsInfoEntity双向一对一关联，而GoodsInfoEntity是物品基本信息表
    /// 如果你有一种新的物品类型，需要实现此接口，更简单的方式是继承<see cref="GoodsInfoExtensionEntity"/>，它已实现此接口并提供基本属性
    /// </summary>
    public abstract class GoodsInfoExtensionEntity : Entity<long>, IGoodsInfoExtensionEntity
    {
        /// <summary>
        /// 关联的物品信息id
        /// </summary>
        public virtual long GoodsInfoId { get; set; }
        /// <summary>
        /// 关联的基本物品信息
        /// </summary>
        public virtual GoodsInfoEntity GoodsInfo { get; set; }
        /// <summary>
        /// 扩展属性
        /// 虽然关联的GoodsInfo已提供这样的属性，但当前类是抽象，给调用方预留更多的扩展点
        /// </summary>
        public virtual string ExtensionData { get; set; }
        /// <summary>
        /// 租户id，由于是抽象的，不确定将来调用方是否需要多租户，所以可空
        /// 虽然GoodsInfo里已经有了，但是考虑将来直接处理数据时更容易，这里也存储下租户id
        /// </summary>
        public virtual int? TenantId { get; set; }
    }
}
