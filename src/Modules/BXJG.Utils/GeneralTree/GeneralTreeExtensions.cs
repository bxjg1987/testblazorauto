using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq;
using BXJG.Utils.Share.GeneralTree;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Utils.GeneralTree
{
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
        public static TChild FindRecursiveDown<TChild>(this IEnumerable<TChild> nodes, long id) where TChild : IGeneralTree<TChild>
        {
            foreach (var item in nodes)
            {
                var r = item.FindRecursiveDown(id);
                if (r != null) return r;
            }
            return default;
        }

        /// <summary>
        /// 生成新的code
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="lasttCode"></param>
        /// <returns></returns>
        public static string BuildCode(string parentCode, string lasttCode)
        {
            if (parentCode.IsNullOrEmpty())
            {
                if (lasttCode.IsNullOrEmpty())
                    return "1".PadLeft(GeneralTreeEntity.CodeUnitLength, '0');
                else
                {
                    var lastBlock1 = lasttCode.Split('.').Last();
                    var temp1 = Convert.ToInt32(lastBlock1) + 1;
                    return temp1.ToString().PadLeft(GeneralTreeEntity.CodeUnitLength, '0');
                }
            }
            else
            {
                if (lasttCode.IsNullOrEmpty())
                    return parentCode + "." + "1".PadLeft(GeneralTreeEntity.CodeUnitLength, '0');
                else
                {
                    var lastBlock = lasttCode.Split('.').Last();
                    var temp = Convert.ToInt32(lastBlock) + 1;
                    return parentCode + "." + temp.ToString().PadLeft(GeneralTreeEntity.CodeUnitLength, '0');
                }
            }
        }
        /// <summary>
        /// 生成新的code
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="lasttCode"></param>
        /// <returns></returns>
        public static string BuildCode(string parentCode, int lasttCode)
        {
            return BuildCode(parentCode, lasttCode.ToString());
        }

        /// <summary>
        /// 递归重新设置code
        /// </summary>
        /// <param name="parentCode">父节点code</param>
        /// <param name="children">子节点集合</param>
        /// <param name="startIndex">从children中指定索引节点开始（跳过前面的）</param>
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

        /// <summary>
        /// 根据code获取所有后代节点，并以平铺结构的集合返回
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static async Task<IList<TEntity>> GetFlattenOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, string code = null) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var query = repository.GetAll();
            if (!code.IsNullOrWhiteSpace())
                query = query.Where(c => c.Code.StartsWith(code));
            query = query.OrderBy(c => c.Code);
            return await query.ToListAsync();// AsyncQueryableExecuter.ToListAsync(query);
        }

        public static string GetParentCode(this string code)
        {
            if (code.Length == GeneralTreeEntity.CodeUnitLength)
            {
                return default;
            }
            else
            {
                return code.Substring(0, code.Length - GeneralTreeEntity.CodeUnitLength).TrimEnd('.');
            }
        }

        /// <summary>
        /// 获取指定节点的兄弟节点、父节点(可能为空)
        /// 均包含其后代节点
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Item1父节点，Item2兄弟节点</returns>
        public static async Task<Tuple<TEntity, IList<TEntity>>> GetBrotherWithOffspringAsync<TEntity>(this IRepository<TEntity, long> repository, TEntity entity) where TEntity : Entity<long>, IGeneralTree<TEntity>
        {
            var parentCode = entity.Code.GetParentCode();

            TEntity parent = null;
            IList<TEntity> children;

            if (!Abp.Extensions.StringExtensions.IsNullOrWhiteSpace(parentCode))
            {
                children = await repository.GetFlattenOffspringAsync<TEntity>(parentCode);
                parent = children[0];
            }
            else
            {
                //本来也可以用上面的StartsWith，但是直接getAll性能更好
                children = await repository.GetFlattenOffspringAsync();
            }
            children = children.Where(c => c.ParentId.Equals(entity.ParentId)).ToList();
            return new Tuple<TEntity, IList<TEntity>>(parent, children);
        }
    }
}
