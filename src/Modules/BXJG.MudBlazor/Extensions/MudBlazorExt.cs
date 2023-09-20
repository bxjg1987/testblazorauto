using BXJG.AbpMudBlazor;
using BXJG.Common.Dto;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MudBlazor
{
    public static class MudBlazorExt
    {
        /// <summary>
        /// 转换为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ConditionFieldDefine MapToDynamicCondition<T>(this IFilterDefinition<T> c)
        {
            return new ConditionFieldDefine
            {
                CompareType = GloableStatic.FilterOperatorMapToCompareType[c.Operator],
                Name = c.Column.PropertyName,
                Value = c.Value?.ToString()
            };
        }
    }
}