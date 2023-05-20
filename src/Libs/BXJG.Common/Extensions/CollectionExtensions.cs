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

            return  dic.All(c =>
            {
                if (dic2.TryGetValue(c.Key, out var p))
                    return p.Equals(c.Value);
                return false;
            });
        }
    }
}
