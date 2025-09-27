using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// 判断两个字典是否相等
        /// key数量相同，且每个值相等
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="dic2"></param>
        /// <returns></returns>
        public static bool Xiangdeng<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> dic2)
        {
            if (dic.Count != dic2.Count)
                return false;

            return dic.All(c =>
            {
                if (dic2.TryGetValue(c.Key, out var p))
                    return p.Equals(c.Value);
                return false;
            });
        }
        /// <summary>
        /// 移除并返回所有满足条件的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> RemoveAll<T>(this ICollection<T> collection, Func<T, bool> where)
        {
            var ary = collection.Where(c => where(c)).ToList();
            foreach (var item in ary)
            {
                collection.Remove(item);
            }
            return ary;
        }
    }
}
