using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 判断两个集合是否有变化
        /// 项使用Eq
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true有变化 false无变化</returns>
        public static bool HasChange<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a == null && b != null)
                return true;

            if (a != null && b == null)
                return true;

            if (a.Count() != b.Count())
                return true;

            foreach (var item in a)
            {
                var item2 = b.Single(c=>c.GetHashCode()==item.GetHashCode());
                if (!item2.Equals(item))
                    return true;
            }
            return false;
            //return a.Any(c => b.Any(d => !d.Equals(c)));
        }

        //public static Func<IEnumerable<T>, IEnumerable<T>, bool> CreateComparer<T>()
        //{
        //    return HasChange;
        //}
    }
}
