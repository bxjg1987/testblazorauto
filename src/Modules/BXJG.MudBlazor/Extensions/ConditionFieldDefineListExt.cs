using BXJG.Common;
using BXJG.Common.Dto;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class ConditionFieldDefineListExt
    {
        /// <summary>
        /// 转换为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        public static IEnumerable<ConditionFieldDefine> MapToDynamicCondition<T>(this ICollection<IFilterDefinition<T>> state)
        {
            return state.Select(c => c.MapToDynamicCondition());
        }
    }
}