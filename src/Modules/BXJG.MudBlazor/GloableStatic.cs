using BXJG.Common;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXJG.MudBlazor
{
    public class GloableStatic
    {
        /// <summary>
        /// mudblazor中的动态条件比较符与BXJG.Common中的比较符的映射
        /// </summary>
        public static readonly IReadOnlyDictionary<string, CompareType> FilterOperatorMapToCompareType = new Dictionary<string, CompareType>
        {
            { FilterOperator.String.Contains, CompareType.Baohan },
            { FilterOperator.String.NotContains, CompareType.BuBaohan },
            { FilterOperator.String.StartsWith, CompareType.StartWith },
            { FilterOperator.String.EndsWith, CompareType.EndWith },
            { FilterOperator.String.Equal, CompareType.Dengyu },
            { FilterOperator.String.NotEqual, CompareType.BuDengyu },
            { FilterOperator.String.Empty, CompareType.Kong },
            { FilterOperator.String.NotEmpty, CompareType.Feikong },

            { FilterOperator.Number.Equal, CompareType.Dengyu },
            { FilterOperator.Number.NotEqual, CompareType.BuDengyu },
            { FilterOperator.Number.LessThanOrEqual, CompareType.XiaoyuDengyu },
            { FilterOperator.Number.LessThan, CompareType.Xiaoyu },
            { FilterOperator.Number.GreaterThan, CompareType.Dayu },
            { FilterOperator.Number.GreaterThanOrEqual, CompareType.DayuDengyu },
            { FilterOperator.Number.NotEmpty, CompareType.Feikong },
            { FilterOperator.Number.Empty, CompareType.Kong },

            { FilterOperator.Enum.IsNot, CompareType.BuDengyu },
            { FilterOperator.Enum.Is, CompareType.Dengyu },

            { FilterOperator.Boolean.Is, CompareType.Dengyu },

            { FilterOperator.DateTime.Is, CompareType.Dengyu },
            { FilterOperator.DateTime.IsNot, CompareType.BuDengyu },
            { FilterOperator.DateTime.After, CompareType.Dayu },
            { FilterOperator.DateTime.OnOrAfter, CompareType.DayuDengyu },
            { FilterOperator.DateTime.Before, CompareType.Xiaoyu },
            { FilterOperator.DateTime.OnOrBefore, CompareType.XiaoyuDengyu },
            { FilterOperator.DateTime.NotEmpty, CompareType.Feikong },
            { FilterOperator.DateTime.Empty, CompareType.Kong },

            { FilterOperator.Guid.Equal, CompareType.Dengyu },
            { FilterOperator.Guid.NotEqual, CompareType.BuDengyu },
        };
    }
}
