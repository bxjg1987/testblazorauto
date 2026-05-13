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
        /// 项使用Equals进行比较，顺序无关
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

            if (a == null && b == null)
                return false;

            var listA = a as IList<T> ?? a.ToList();
            var listB = b as IList<T> ?? b.ToList();

            if (listA.Count != listB.Count)
                return true;

            var remaining = listB.ToList();
            foreach (var item in listA)
            {
                if (!remaining.Remove(item))
                    return true;
            }
            return false;
        }

        //public static Func<IEnumerable<T>, IEnumerable<T>, bool> CreateComparer<T>()
        //{
        //    return HasChange;
        //}
    }
}
