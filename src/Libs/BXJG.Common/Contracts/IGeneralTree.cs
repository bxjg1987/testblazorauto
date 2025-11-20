using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Contracts
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
        /// 唯一id
        /// </summary>
        public object Id { get; set; }
        /// <summary>
        /// 父节点id
        /// </summary>
        public object ParentId { get; set; }
    }
  /// <summary>
  /// 
  /// </summary>
    public interface IGeneralTree: IHaveParentId
    {

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
        /// 子节点数量
        /// 有些时候可能不会加载子节点，仅仅想获取子节点的数量
        /// </summary>
        public int ChildrenCount { get; set; }
        /*
        * 有个品牌，它是固定节点，系统预设的，
        * 它有多个子节点，代表不同的具体品牌
        * 前端下拉框需要绑定品牌这个节点的id
        * 不能用code，因为移动节点后，code会变，前端硬编码绑定id是没有问题的
        * 但在 单数据库，多租户场景中，绑定id也行不通了。 所以需要单独定义个标记，要跟租户无关 的
        * 
        * 这个不是通用数的问题，因为其它通用树的子类往往是特定数据，而数据字典里包含各种乱七八糟的数据，品牌、客户级别等，下拉框往往需要不同的节点
        * 但通用树提供这个字段，至少给其它数据一个机会，可以通过此字段来获取跟租户无关的功能
        */
        /// <summary>
        /// 节点标识，不同租户下同类型的节点，此字段一样
        /// 如：品牌  表示品牌节点，不同租户下此字段值一样
        /// 使用场景：在数据字典功能中，前端下拉框绑定时可以通过此字段绑定指定节点类型
        /// 不能用DisplayName，因为它可能变
        /// 不能用id，因为相同数据库中的不同租户id不同
        /// 不能用code，因为节点移动后，code也会变
        /// 用不到此字段时，请忽略。此字段通常不允许修改
        /// </summary>
        public string? Name { get; set; }
    }
    /// <summary>
    /// 通常新增dto实现此接口，因为它仅仅需要提供父节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHaveParentId<T> :  IHaveParentId  /*, IEntity<T>, IEntityDto<T>*/ where T : struct
    {
        /// <summary>
        /// 
        /// </summary>
        public new T Id{ get; set; }

        object IHaveParentId.Id
        {
            get { 
                return Id;
            }
            set { Id = (T)value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public new T? ParentId { get; set; }
        object IHaveParentId.ParentId
        {
            get
            {
                return ParentId;
            }
            set { ParentId = (T?)value; }
        }
    }
  
    
    /// <summary>
    /// 树
    /// </summary>
    public interface IGeneralTree<TChild> : IHaveParentId<long>, IGeneralTree
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

        /// <summary>
        /// 子集
        /// </summary>
        public List<TChild> Children { get; set; }
       
    }
}