using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
    /*
     * 通用树模块的核心逻辑目前并不依赖此接口，而是依赖的抽象类
     * 后续也许会让核心逻辑依赖此接口，而不是抽象类。
     * 
     * 目前这个接口用于扩展根树相关的功能，让实体和dto能有些通用功能
     */

    /// <summary>
    /// 树
    /// </summary>
    public interface IGeneralTree<TChild> where TChild: IGeneralTree<TChild>
    {
        /// <summary>
        /// 主键、唯一id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public TChild? Parent { get; set; }
        /// <summary>
        /// 父节点id
        /// </summary>
        public long? ParentId { get; set; }
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

    /// <summary>
    /// 实现树的一些通用操作
    /// </summary>
    public static class GeneralTreeExtensions
    {
        /// <summary>
        /// 递归向下查找节点
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="id">目标节点id</param>
        /// <returns></returns>
        public static TChild FindRecursiveDown<TChild>( this TChild node, long? id) where TChild : IGeneralTree<TChild>
        { 
            if(node.Id==id)
                return node;

            if (node.Children != null && node.Children.Any())
            {
                foreach (var item in node.Children)
                {
                    var r = FindRecursiveDown(item,id);
                    if (r!=null) return r;
                }
            }
            return default;
        }

        /// <summary>
        /// 递归向下查找节点
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="id">目标节点id</param>
        /// <returns></returns>
        public static TChild FindRecursiveDown<TChild>(this IEnumerable<TChild> nodes, long? id) where TChild : IGeneralTree<TChild>
        {
            foreach (var item in nodes)
            {
                var r = item.FindRecursiveDown(id);
                if (r!=null) return r;
            }
            return default;
        }
    }
}