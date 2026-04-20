using BXJG.Common.Contracts;
using BXJG.Common.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class LinqExt
    {
        /// <summary>
        /// 应用动态条件
        /// 注意：字符串类型的条件值通过拼接方式构建动态LINQ表达式，这是设计需要，不存在注入风险，无需修改
        /// </summary>
        /// <param name="q"></param>
        /// <param name="define"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDynamicCondtion<T>(this IQueryable<T> q, ConditionFieldDefine define)
        {
            var t = typeof(T);

            //若存在导航熟悉，则拿最终的导航熟悉的类型
            var pnames = define.Name.Split('.');
            for (int i = 0; i < pnames.Length; i++)
            {
                t = t.GetProperty(pnames[i]).PropertyType;
            }

            //define.CompareType = Enum.Parse<CompareType>(define.CompareType.ToString());

            if (t.IsNullable())
            {
                switch (define.CompareType)
                {
                    case CompareType.Kong:
                        return q.Where($"{define.Name} == null");
                    case CompareType.Feikong:
                        return q.Where($"{define.Name} != null");
                }
                t = Nullable.GetUnderlyingType(t);
            }

            if (t == typeof(string))
            {
                switch (define.CompareType)
                {
                    case CompareType.Baohan:
                        return q.Where($"{define.Name}.Contains(\"{define.Value}\")");
                    case CompareType.BuBaohan:
                        return q.Where($"!{define.Name}.Contains(\"{define.Value}\")");
                    case CompareType.StartWith:
                        return q.Where($"{define.Name}.StartsWith(\"{define.Value}\")");
                    case CompareType.EndWith:
                        return q.Where($"{define.Name}.EndsWith(\"{define.Value}\")");
                    default:
                        return q.Where($"{define.Name}==\"{define.Value}\"");
                }
            }

            if (t.IsEnum)
            {
                //将define.Value转换为t类型的值
                var value2 = Enum.Parse(t, define.Value);
                var valus = value2.ToString();
                var value = t.FullName + "." + valus;
                //object value;
                //var vs = Enum.GetValues(t);
                // foreach (var item in vs)
                // {
                //     if (item.ToString() == define.Value)
                //     {
                //         value = item; break;
                //     }
                // }

                //var value = Convert.ChangeType(int.Parse(define.Value), t);
                //var value = int.Parse(define.Value);
                switch (define.CompareType)
                {
                    case CompareType.Dengyu:
                        return q.Where($"{define.Name}=={value}");
                    case CompareType.BuDengyu:
                        return q.Where($"{define.Name}!={value}");
                }
            }

            if (t == typeof(bool))
            {
                bool value = true;
                if (define.Value.IsNullOrWhiteSpaceBXJG() || define.Value == "0" || define.Value.Equals("false", StringComparison.OrdinalIgnoreCase))
                    value = false;

                switch (define.CompareType)
                {
                    case CompareType.Dengyu:
                        return q.Where($"{define.Name}=={value}");
                    case CompareType.BuDengyu:
                        return q.Where($"{define.Name}!={value}");
                }
            }


            object value1;// = define.Value;// Convert.ChangeType(define.Value, t);

            if (t == typeof(DateTime))
                value1 = DateTime.Parse(define.Value);
            else if (t == typeof(DateTimeOffset))
                value1 = DateTimeOffset.Parse(define.Value);
            else
                value1 = Convert.ChangeType(define.Value, t);

            switch (define.CompareType)
            {
                case CompareType.Dayu:
                    return q.Where($"{define.Name}>@0", value1);
                case CompareType.Dengyu:
                    return q.Where($"{define.Name}==@0", value1);
                case CompareType.Xiaoyu:
                    return q.Where($"{define.Name}<@0", value1);
                case CompareType.DayuDengyu:
                    return q.Where($"{define.Name}>=@0", value1);
                case CompareType.XiaoyuDengyu:
                    return q.Where($"{define.Name}<=@0", value1);
                case CompareType.BuDengyu:
                    return q.Where($"{define.Name}!=@0", value1);
                default:
                    throw new Exception("未实现的比较类型");
            }


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
            if (defines == default)
                return q;

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
            if (defines == default)
                return q;

            if (defines is IEnumerable<ConditionFieldDefine>)
            {
                q = q.ApplyDynamicCondtion(defines as IEnumerable<ConditionFieldDefine>);
            }
            else if (defines is IDynamicCondition cc)
            {
                q = q.ApplyDynamicCondtion(cc.Conditions);
            }
            return q;
        }
    }
}