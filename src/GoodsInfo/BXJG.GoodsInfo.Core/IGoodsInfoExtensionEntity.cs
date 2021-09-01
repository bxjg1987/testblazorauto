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
    /// 具体物品实体接口
    /// 比如：整机、配件、耗材、办公用品
    /// 它与GoodsInfoEntity双向一对一关联，而GoodsInfoEntity是物品基本信息表
    /// 如果你有一种新的物品类型，需要实现此接口，更简单的方式是继承<see cref="GoodsInfoExtensionEntity"/>，它已实现此接口并提供基本属性
    /// </summary>
    public interface IGoodsInfoExtensionEntity : IEntity<long>, IExtendableObject, IMayHaveTenant 
    {
        /// <summary>
        /// 关联的基本物品信息id
        /// </summary>
        long GoodsInfoId { get; set; }
        /// <summary>
        /// 关联的基本物品信息
        /// </summary>
        GoodsInfoEntity GoodsInfo { get; set; }
    }
}
