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
    /// 物品类型接口
    /// 通常建议继承<see cref="GoodsInfoEntity"/>，特殊需求时才直接实现此接口。
    /// </summary>
    public interface IGoodsInfoEntity : IAggregateRoot<long>, IFullAudited, IMayHaveTenant
    {
        ///// <summary>
        ///// 物品类型名称
        ///// </summary>
        //string TypeName { get; }
        /// <summary>
        /// 物品名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 助记码
        /// </summary>
        string MnemonicCode { get; set; }
        /// <summary>
        /// 所属分类id
        /// </summary>
        long CategoryId { get; set; }
        /// <summary>
        /// 所属分类实体
        /// </summary>
        GoodsInfoCategoryEntity Category { get; set; }
        /// <summary>
        /// 单位id
        /// </summary>
        string UnitId { get; set; }
        /// <summary>
        /// 所属品牌id
        /// </summary>
        string BrandId { get; set; }
    }
}