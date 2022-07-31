using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.Common.Extensions
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
        /// <returns></returns>
        public static bool HasChange<T>(this IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a == null && b != null)
                return true;

            if (a != null && b == null)
                return true;

            if (a.Count() != b.Count())
                return true;

            return a.Any(c => b.Any(d => !d.Equals(c)));
        }

        //public static Func<IEnumerable<T>, IEnumerable<T>, bool> CreateComparer<T>()
        //{
        //    return HasChange;
        //}
    }
}
