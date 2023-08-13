using BXJG.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExt
    {
        /// <summary>
        /// 应用动态条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="define"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDynamicCondtion<T>(this IQueryable<T> q, ConditionFieldDefine define)
        {
            return q;
            //属性类型不同，linq写法不同，太难实现了。还要考虑导航属性
            //switch (define.CompareType)
            //{
            //    case BXJG.Common.CompareType.Dayu:
            //        return q.Where($"{define.Name}>{define.Value}");
            //    case BXJG.Common.CompareType.Dengyu:
            //        return q.Where($"{define.Name}=={define.Value}");
            //    case BXJG.Common.CompareType.Xiaoyu:
            //        return q.Where($"{define.Name}<{define.Value}");
            //    case BXJG.Common.CompareType.DayuDengyu:
            //        return q.Where($"{define.Name}>={define.Value}");
            //    case BXJG.Common.CompareType.XiaoyuDengyu:
            //        return q.Where($"{define.Name}<={define.Value}");
            //    case BXJG.Common.CompareType.BuDengyu:
            //        return q.Where($"{define.Name}!={define.Value}");
            //    case BXJG.Common.CompareType.Baohan:
            //        return q.Where($"{define.Name}.Contains(\"{define.Value}\")");
            //    case BXJG.Common.CompareType.BuBaohan:
            //        return q.Where($"!{define.Name}.Contains(\"{define.Value}\")");
            //    case BXJG.Common.CompareType.StartWith:
            //        return q.Where($"{define.Name}.StartsWith(\"{define.Value}\")");
            //    case BXJG.Common.CompareType.EndWith:
            //        return q.Where($"{define.Name}.EndsWith(\"{define.Value}\")");
            //    case BXJG.Common.CompareType.Kong:
            //        return q.Where($"{define.Name}==null");
            //    case BXJG.Common.CompareType.Feikong:
            //        return q.Where($"{define.Name}!=null");
            //    default:
            //        return q;
            //}
        }
        /// <summary>
        /// 应用动态条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="defines"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDynamicCondtion<T>(this IQueryable<T> q, IEnumerable<ConditionFieldDefine> defines)
        {
            foreach (var item in defines)
            {
                q = q.ApplyDynamicCondtion(item);
            }
            return q;
        }
        /// <summary>
        /// 应用动态条件
        /// </summary>
        /// <param name="q"></param>
        /// <param name="defines"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDynamicCondtion<T>(this IQueryable<T> q, object defines)
        {
            if (defines is IEnumerable<ConditionFieldDefine>)
            {
                q = q.ApplyDynamicCondtion(defines as IEnumerable<ConditionFieldDefine>);
            }
            return q;
        }
    }
}