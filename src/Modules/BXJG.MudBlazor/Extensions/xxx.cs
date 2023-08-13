using BXJG.Common;
using BXJG.Common.Dto;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor.Extensions
{
    public static class xxx
    {
        public static readonly IReadOnlyDictionary<string, CompareType> FilterOperatorMapToCompareType = new Dictionary<string, CompareType>{
            { FilterOperator.String.Contains, CompareType.Baohan },
            { FilterOperator.String.NotContains, CompareType.BuBaohan },
            { FilterOperator.String.StartsWith, CompareType.StartWith },
            { FilterOperator.String.EndsWith, CompareType.EndWith },
            { FilterOperator.String.Equal, CompareType.Dengyu },
            { FilterOperator.String.NotEqual, CompareType.BuDengyu },
            { FilterOperator.String.Empty, CompareType.Kong },
            { FilterOperator.String.NotEmpty, CompareType.Feikong },
        };

        public static IEnumerable<ConditionFieldDefine> sss<T>(this GridState<T> state)
        {
            return state.FilterDefinitions.Select(c => new ConditionFieldDefine
            {
                CompareType = FilterOperatorMapToCompareType[c.Operator],
                Name = c.Column.PropertyName,
                Value = c.Value?.ToString()
            });
        }
    }
}