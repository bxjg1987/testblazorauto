using BXJG.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BXJG.Common.Contracts
{
    /// <summary>
    /// 对ITree的扩展，通常树实体和dto实现ITree，从而都自动有这些扩展能力
    /// </summary>
    public static class TreeExtensions
    {
        #region 字符串自增
        /// <summary>
        /// 字符串最后部分自增
        /// 00.01.01  ->  00.01.02
        /// </summary>
        /// <param name="str">原始保护数字的字符串，如：00.01.01 </param>
        /// <param name="jgf">间隔符，如：.</param>
        /// <returns></returns>
        public static string Increment(this string str, string jgf)
        {
            var ary = str.Split(jgf).ToList();
            var last = ary.Last();
            var lastLength = last.Length;
            var lastVal = int.Parse(last);
            lastVal++;
            last = lastVal.ToString().PadLeft(lastLength, '0');
            ary.RemoveAt(ary.Count - 1);
            ary.Add(last);
            return string.Join(jgf, ary);
        }
        /// <summary>
        /// 字符串最后部分自增
        /// 000101  ->  000102
        /// </summary>
        /// <param name="str">原始保护数字的字符串，如：000101 </param>
        /// <param name="c">每层长度，如：2</param>
        /// <returns></returns>
        public static string Increment(this string str, int c)
        {
            var ary = str.SplitByLength(c).ToList();
            var last = ary.Last();
            var lastVal = int.Parse(last);
            lastVal++;
            last = lastVal.ToString().PadLeft(c, '0');
            ary.RemoveAt(ary.Count - 1);
            ary.Add(last);
            return string.Join('0', ary);
        }

        /// <summary>
        /// 生成新的code
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="lasttCode"></param>
        /// <param name="CodeUnitLength"></param>
        /// <returns></returns>
        public static string BuildCode(string parentCode, string lasttCode, int CodeUnitLength = 5)
        {
            if (parentCode.IsNullOrWhiteSpaceBXJG())
            {
                if (lasttCode.IsNullOrWhiteSpaceBXJG())
                    return "1".PadLeft(CodeUnitLength, '0');
                else
                {
                    var lastBlock1 = lasttCode.Split('.').Last();
                    var temp1 = Convert.ToInt32(lastBlock1) + 1;
                    return temp1.ToString().PadLeft(CodeUnitLength, '0');
                }
            }
            else
            {
                if (lasttCode.IsNullOrWhiteSpaceBXJG())
                    return parentCode + "." + "1".PadLeft(CodeUnitLength, '0');
                else
                {
                    var lastBlock = lasttCode.Split('.').Last();
                    var temp = Convert.ToInt32(lastBlock) + 1;
                    return parentCode + "." + temp.ToString().PadLeft(CodeUnitLength, '0');
                }
            }
        }
        /// <summary>
        /// 生成新的code
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="lasttCode"></param>
        /// <param name="CodeUnitLength"></param>
        /// <returns></returns>
        public static string BuildCode(string parentCode, int lasttCode, int CodeUnitLength = 5)
        {
            return BuildCode(parentCode, lasttCode.ToString());
        }
        #endregion 

        #region 递归
        ///// <summary>
        ///// 递归向上
        ///// </summary>
        ///// <typeparam name="TTreeNode"></typeparam>
        ///// <param name="tree"></param>
        ///// <param name="act">true停止递归 false继续</param>
        //public static void RecursiveUp<TTreeNode>(this ITreeNode<TTreeNode> tree, Func<TTreeNode, bool> act)
        //    where TTreeNode : ITreeNode
        //{
        //    tree.RecursiveUp(x => act((TTreeNode)x));
        //}
        /// <summary>
        /// 递归向上
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="act">true停止递归 false继续</param>
        public static void RecursiveUp<TTreeNode>(this TTreeNode tree, Func<TTreeNode, bool> act)
            where TTreeNode : IGeneralTree<TTreeNode>

        {
            var r = act(tree);
            if (r)
                return;

            var p = tree.Parent;
            if (p != null)
                p.RecursiveUp(act);
        }
        /// <summary>
        /// 递归向下
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="act">true停止递归 false继续</param>
        /// <param name="isReturn">true停止递归 false继续</param>
        public static void RecursiveDown<TTreeNode>(this IList<TTreeNode> tree, Func<TTreeNode, bool> act, ref bool isReturn)
              where TTreeNode : IGeneralTree<TTreeNode>
        {
            foreach (var node in tree)
            {
                if (isReturn)
                    return;

                isReturn = act(node);

                if (node.Children != default && node.Children.Count > 0)
                    node.Children.RecursiveDown(act, ref isReturn);
            }
        }
        ///// <summary>
        ///// 递归向下
        ///// </summary>
        ///// <typeparam name="TTreeNode"></typeparam>
        ///// <param name="tree"></param>
        ///// <param name="act">true停止递归 false继续</param>
        ///// <param name="isReturn">true停止递归 false继续</param>
        //public static void RecursiveDown<TTreeNode>(this IList<TTreeNode> tree, Func<TTreeNode, bool> act, ref bool isReturn)
        //    where TTreeNode : ITreeNode
        //{
        //    tree.Cast<ITreeNode>().ToList().RecursiveDown(x => act((TTreeNode)x), ref isReturn);
        //}

        /// <summary>
        /// 递归向下查找节点
        /// </summary>
        /// <typeparam name="TChild"></typeparam>
        /// <param name="id">目标节点id</param>
        /// <returns></returns>
        [Obsolete]
        public static TChild FindRecursiveDown<TChild>(this TChild node, long id) where TChild : IGeneralTree<TChild>
        {
            if (node.Id == id)
                return node;

            if (node.Children != null && node.Children.Any())
            {
                foreach (var item in node.Children)
                {
                    var r = FindRecursiveDown(item, id);
                    if (r != null) return r;
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
        [Obsolete]
        public static TChild FindRecursiveDown<TChild>(this IEnumerable<TChild> nodes, long id) where TChild : IGeneralTree<TChild>
        {
            foreach (var item in nodes)
            {
                var r = item.FindRecursiveDown(id);
                if (r != null) return r;
            }
            return default;
        }




        #endregion

        #region 重置code
        /// <summary>
        /// 递归向下重置code
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="list"></param>
        /// <param name="index"></param>
        public static void ResetCode<TTreeNode>(this IList<TTreeNode> list, string parentCode, int index, ref CodeRules cr)
             where TTreeNode : IGeneralTree<TTreeNode>
        {
            for (; index < list.Count; index++)
            {
                var item = list[index];

                if (parentCode.IsNullOrWhiteSpaceBXJG())
                    item.Code = "";
                else
                    item.Code = $"{parentCode}{cr.Spacer}";

                item.Code = $"{item.Code}{index.ToString().PadLeft(cr.Length, '0')}";

                if (item.Children != default && item.Children.Count > 0)
                    item.Children.ResetCode(item.Code, 0, ref cr);
            }
        }



        /// <summary>
        /// 递归重新设置code
        /// </summary>
        /// <param name="parentCode">父节点code</param>
        /// <param name="children">子节点集合</param>
        /// <param name="startIndex">从children中指定索引节点开始（跳过前面的）</param>
        [Obsolete]
        public static void ResetCode<TEntity>(string parentCode, IList<TEntity> children, int startIndex = 0) where TEntity : IGeneralTree<TEntity>
        {
            for (int i = startIndex; i < children.Count; i++)
            {
                var item = children[i];
                item.Code = BuildCode(parentCode, i);
                if (item.Children != null)
                    ResetCode(item.Code, item.Children);
            }
        }




        ///// <summary>
        ///// 递归向下重置code
        ///// </summary>
        ///// <typeparam name="TTreeNode"></typeparam>
        ///// <param name="parentCode"></param>
        ///// <param name="list"></param>
        ///// <param name="index"></param>
        //public static void ResetCode<TTreeNode>(this IList<TTreeNode> list, string parentCode, int index, ref CodeRules cr)
        //    where TTreeNode : ITreeNode
        //{
        //    ResetCode(list.Cast<ITreeNode>().ToList(), parentCode, index, ref cr);
        //}
        #endregion

        /// <summary>
        /// 移动节点
        /// </summary>
        /// <param name="shareTrunk">目标和源处于的共同主干</param>
        /// <param name="source">源节点</param>
        /// <param name="sourceBrothers">源节点的兄弟节点集合</param>
        /// <param name="target">目标节点</param>
        /// <param name="targetBrothers">目标节点的兄弟节点集合</param>
        /// <param name="moveType"></param>
        /// <param name="cr">code规则</param>
        /// <returns></returns>
        public static void Move<TTreeNode>(IList<TTreeNode> shareTrunk,
                                TTreeNode source,
                                IList<TTreeNode> sourceBrothers,
                                TTreeNode target,
                                IList<TTreeNode> targetBrothers,
                                GeneralTreeMoveType moveType,
                                CodeRules cr = default)
               where TTreeNode : IGeneralTree<TTreeNode>
        {
            if (cr.Length == 0)
            {
                cr = new CodeRules();
            }
            //源节点的根处于topChildren的索引
            int resetIndex1 = 0;
            source.RecursiveUp(x =>
            {
                resetIndex1 = shareTrunk.IndexOf(x);
                return resetIndex1 > -1;
            });

            //将源节点从sourceBrothers移除
            sourceBrothers.Remove(source);

            if (moveType != GeneralTreeMoveType.Append)
            {
                //将源节点插入目标targetBrothers
                var targetIndex = targetBrothers.IndexOf(target);

                if (moveType == GeneralTreeMoveType.Front)
                    targetIndex--;
                else
                    targetIndex++;

                if (targetIndex < 0)
                    targetIndex = 0;
                else if (targetIndex >= targetBrothers.Count)
                    targetIndex = targetBrothers.Count - 1;

                targetBrothers.Insert(targetIndex, source);
                source.Parent = target.Parent;
                source.ParentId = target.ParentId;
            }
            else
            {
                target.Children.Add(source);
                source.Parent = target;
                source.ParentId = target.Id;
            }

            //目标节点处于根topChildren的索引
            var resetIndex2 = 0;
            target.RecursiveUp(x =>
            {
                resetIndex2 = shareTrunk.IndexOf(x);
                return resetIndex2 > -1;
            });

            var resetIndex = resetIndex1 < resetIndex2 ? resetIndex1 : resetIndex2;

            ResetCode(shareTrunk, shareTrunk.First().Parent?.Code, resetIndex, ref cr);
        }

        //public static void Move<TTreeNode>(IList<TTreeNode> shareTrunk,
        //                                   TTreeNode source,
        //                                   IList<TTreeNode> sourceBrothers,
        //                                   TTreeNode target,
        //                                   IList<TTreeNode> targetBrothers,
        //                                   MoveType moveType,
        //                                   CodeRules cr = default)
        //    where TTreeNode : ITreeNode
        //{
        //    Move(shareTrunk.Cast<ITreeNode>().ToList(), source, sourceBrothers.Cast<ITreeNode>().ToList(), target, targetBrothers.Cast<ITreeNode>().ToList(), moveType, cr);
        //}

        /// <summary>
        /// 按指定长度分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <param name="padleter"></param>
        /// <returns></returns>
        public static string[] SplitByLength(this string str, int maxLength, char? padleter = default)
        {
            return Enumerable.Range(0, str.Length / maxLength)
                             .Select(i =>
                             {
                                 var temp = str.Substring(i * maxLength, Math.Min(maxLength, str.Length - i * maxLength));
                                 return padleter == default ? temp : temp.PadLeft(maxLength, padleter.Value);
                             })
                             .ToArray();
        }
        /// <summary>
        /// 获取父节点code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetParentCode(this string code, CodeRules cr = default)
        {
            if (cr.Length == 0)
                cr = new CodeRules();

            if (code.Length == cr.Length)
                return default;

            return code.Substring(0, code.Length - cr.Length - cr.Spacer.Length);
        }
        /// <summary>
        /// 建立父子关系
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sdf"></param>
        /// <param name="setParent"></param>
        public static void Link<T>(this IEnumerable<T> sdf, bool setParent = false) where T : IGeneralTree<T>
        {
            foreach (var item in sdf)
            {
                item.Children = sdf.Where(x => x.ParentId.Equals(item.Id)).OrderBy(x => x.Code).ToList();
                if (!setParent)
                    item.Parent = sdf.SingleOrDefault(x => x.Id.Equals(item.ParentId));
            }
        }

        //public static ValueTask<List<TTreeNode>> WhereStartWithParent<TTreeNode>(this IEnumerable<TTreeNode> q, BaseGetTreeNodeInput input)
        //  where TTreeNode : class, ITreeNode<TTreeNode>
        //{
        //    return q.WhereStartWithParent(input.ParentId, input.ParentCode);
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TTreeNode"></typeparam>
        ///// <param name="q"></param>
        ///// <param name="parentId"></param>
        ///// <param name="code"></param>
        ///// <param name="isOnlyLoadChildren"></param>
        ///// <returns></returns>
        //public static async ValueTask<List<TTreeNode>> WhereStartWithParent<TTreeNode>(this IEnumerable<TTreeNode> q, object? parentId = default, string? code = default, bool isOnlyLoadChildren = false)
        //    where TTreeNode : class, ITreeNode<TTreeNode>
        //{

        //}

    }
}
