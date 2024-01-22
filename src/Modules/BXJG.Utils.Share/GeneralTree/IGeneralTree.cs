using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.Share.GeneralTree
{
    /*
     * 通用树模块的核心逻辑目前并不依赖此接口，而是依赖的抽象类
     * 后续也许会让核心逻辑依赖此接口，而不是抽象类。
     * 
     * 目前这个接口用于扩展跟树相关的功能，让实体和dto能有些通用功能
     */

    /// <summary>
    /// 通常新增dto实现此接口，因为它仅仅需要提供父节点
    /// </summary>
    public interface IHaveParentId
    {
        /// <summary>
        /// 
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object? ParentId { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHaveParentId<T> : IHaveParentId  /*, IEntity<T>, IEntityDto<T>*/ where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        public new T Id
        {
            get { return (T)(this as IHaveParentId).Id; }
            set { (this as IHaveParentId).Id = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public new T? ParentId
        {
            get { return (T?)(this as IHaveParentId).ParentId; }
            set { (this as IHaveParentId).ParentId = value; }
        }
    }

    /// <summary>
    /// 树
    /// </summary>
    public interface IGeneralTree<TChild> : IHaveParentId<long>
        where TChild : IGeneralTree<TChild>
    {
        ///// <summary>
        ///// 主键、唯一id
        ///// </summary>
        //public new long Id { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public TChild? Parent { get; set; }
        ///// <summary>
        ///// 父节点id
        ///// </summary>
        //public long? ParentId { get; set; }
        ///// <summary>
        ///// code中，每一段的长度
        ///// </summary>
        //public const int CodeUnitLength = 5;
        /// <summary>
        /// 有层次结构的代码，如00001.00002.
        /// GeneralTreeEntity.CodeUnitLength定义了长度
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 子集
        /// </summary>
        public IList<TChild> Children { get; set; }
        /// <summary>
        /// 子节点数量
        /// 有些时候可能不会加载子节点，仅仅想获取子节点的数量
        /// </summary>
        public int ChildrenCount { get; set; }
    }
}