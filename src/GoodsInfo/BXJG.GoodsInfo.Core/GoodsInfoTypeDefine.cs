using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.GoodsInfo
{
    /// <summary>
    /// 具体的物品类型描述类
    /// </summary>
    public class GoodsInfoTypeDefine
    {
        /// <summary>
        /// 实例化具体的物品类型描述类
        /// </summary>
        /// <param name="entityType">具体物品类型</param>
        /// <param name="displayName">具体物品类型的显示名</param>
        /// <param name="repositoryType">仓储类型</param>
        public GoodsInfoTypeDefine(Type entityType, ILocalizableString displayName, Type repositoryType)
        {
            EntityType = entityType;
            DisplayName = displayName;
            RepositoryType = repositoryType;
        }
        /// <summary>
        /// 获取具体物品类型的完全限定名
        /// </summary>
        public string EntityTypeFullName => EntityType.FullName;
        /// <summary>
        /// 获取具体物品类型
        /// </summary>
        public virtual Type EntityType { get; private set; }
        /// <summary>
        /// 获取具体物品类型的显示名
        /// </summary>
        public ILocalizableString DisplayName { get; private set; }
        /// <summary>
        /// 获取仓储类型
        /// </summary>
        public virtual Type RepositoryType { get; private set; }
    }
    /// <summary>
    /// 具体的物品类型描述类
    /// </summary>
    /// <typeparam name="TEntityType">具体物品类型</typeparam>
    /// <typeparam name="TRepositoryType">仓储类型</typeparam>
    public class GoodsInfoTypeDefine<TEntityType, TRepositoryType> : GoodsInfoTypeDefine
    {
        /// <summary>
        /// 实例化具体的物品类型描述类
        /// </summary>
        /// <param name="displayName">具体物品类型的显示名</param>
        public GoodsInfoTypeDefine(ILocalizableString displayName) : base(typeof(TEntityType), displayName, typeof(TRepositoryType))
        {
        }
    }
}
