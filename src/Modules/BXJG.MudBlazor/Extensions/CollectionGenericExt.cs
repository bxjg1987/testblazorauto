using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class CollectionGenericExt
    {
        /// <summary>
        /// 转换为System.Linq.Dynamic.Core的排序字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sortDefinitions"></param>
        /// <returns></returns>
        public static string ToLinqDynamicCore<T>(this ICollection<SortDefinition<T>> sortDefinitions)
        {
            if (sortDefinitions == default || !sortDefinitions.Any())
                return "Id";

            return string.Join(",", sortDefinitions.Select(c => $"{c.SortBy} {(c.Descending ? "desc" : "asc")}"));
        }
    }
}
