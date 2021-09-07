using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 物品类型描述类
    /// </summary>
    public class GoodsInfoTypeDefine
    {
        /// <summary>
        /// 实例化具体的物品类型描述类
        /// </summary>
        /// <param name="entityTypeName">具体物品类型名</param>
        /// <param name="displayName">具体物品类型的显示名</param>
        /// <param name="orderIndex">顺序</param>
        public GoodsInfoTypeDefine(string entityTypeName, ILocalizableString displayName, int orderIndex = int.MinValue)
        {
            EntityTypeName = entityTypeName;
            DisplayName = displayName;
        }
        /// <summary>
        /// 获取具体物品类型名
        /// </summary>
        public string EntityTypeName { get; private set; }
        /// <summary>
        /// 获取具体物品类型的显示名
        /// </summary>
        public ILocalizableString DisplayName { get; private set; }
        /// <summary>
        /// 顺序
        /// </summary>
        public int OrderIndex { get; private set; }
    }
}
