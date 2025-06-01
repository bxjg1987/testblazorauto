using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeExtensions
    {
        #region 生日转年龄相关
        /// <summary>
        /// 获得年龄字符串：某个日期点到今天的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        public static string GetAgeString(this DateTime p_FirstDateTime)
        {
            return CalculateAgeString(p_FirstDateTime, DateTime.Now, null);
        }

        /// <summary>
        /// 获得年龄字符串：某个日期点到今天的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        /// <param name="p_ReturnFormat">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, string p_ReturnFormat)
        {
            return CalculateAgeString(p_FirstDateTime, DateTime.Now, p_ReturnFormat);
        }

        /// <summary>
        /// 获得年龄字符串：两个日期点之间的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        /// <param name="p_SecondDateTime">目标日期</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime)
        {
            return CalculateAgeString(p_FirstDateTime, p_SecondDateTime, null);
        }

        /// <summary>
        /// 获得年龄字符串：两个日期点之间的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        /// <param name="p_SecondDateTime">目标日期</param>
        /// <param name="p_ReturnFormat">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime, string p_ReturnFormat)
        {
            return CalculateAgeString(p_FirstDateTime, p_SecondDateTime, p_ReturnFormat);
        }

        /// <summary>
        /// 计算年龄字符串
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        /// <param name="p_SecondDateTime">目标日期</param>
        /// <param name="p_ReturnFormat">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string CalculateAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime, string p_ReturnFormat)
        {
            if (string.IsNullOrWhiteSpace(p_ReturnFormat))
                p_ReturnFormat = "{0}岁{1}月{2}天";

            var r = CalculateAge(p_FirstDateTime, p_SecondDateTime);
            return string.Format(p_ReturnFormat, r.years, r.months, r.days);
        }

        /// <summary>
        /// 计算两个日期之间的年龄（年/月/日）
        /// </summary>
        /// <param name="p_FirstDateTime">出生日期</param>
        /// <param name="p_SecondDateTime">目标日期</param>
        /// <returns>(years, months, days) 元组</returns>
        public static (int years, int months, int days) CalculateAge(this DateTime p_FirstDateTime, DateTime p_SecondDateTime)
        {
            // 确保出生日期早于目标日期
            if (p_FirstDateTime > p_SecondDateTime)
            {
                // 交换日期并递归调用（确保结果为正数）
                var result = CalculateAge(p_SecondDateTime, p_FirstDateTime);
                return result;
            }

            // 计算年份差（基础值）
            int years = p_SecondDateTime.Year - p_FirstDateTime.Year;
            int months = 0;
            int days = 0;

            // 检查月份和日是否需要调整
            if (p_SecondDateTime.Month < p_FirstDateTime.Month ||
                (p_SecondDateTime.Month == p_FirstDateTime.Month && p_SecondDateTime.Day < p_FirstDateTime.Day))
            {
                // 未满一整年，年份减1
                years--;

                // 计算临时月份差（借位计算）
                months = p_SecondDateTime.Month + 12 - p_FirstDateTime.Month;
            }
            else
            {
                // 直接计算月份差
                months = p_SecondDateTime.Month - p_FirstDateTime.Month;
            }

            // 计算天数差
            if (p_SecondDateTime.Day < p_FirstDateTime.Day)
            {
                // 获取目标日期上一个月的天数
                int daysInPreviousMonth = DateTime.DaysInMonth(
                    p_SecondDateTime.Year,
                    p_SecondDateTime.Month == 1 ? 12 : p_SecondDateTime.Month - 1
                );

                // 计算借位后的天数
                days = p_SecondDateTime.Day + daysInPreviousMonth - p_FirstDateTime.Day;

                // 月份借位后需减1
                if (months == 0)
                {
                    months = 11;
                    years--;  // 年份借位
                }
                else
                {
                    months--;
                }
            }
            else
            {
                // 直接计算天数差
                days = p_SecondDateTime.Day - p_FirstDateTime.Day;
            }

            return (years, months, days);
        }
        #endregion

        /// <summary>
        /// 获取指定日期所在月的起始时间（00:00:00）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当月第一天的零点</returns>
        public static DateTimeOffset MonthStart(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, 1, 0, 0, 0, dt.Offset);
        }

        /// <summary>
        /// 获取指定日期所在月的起始时间（00:00:00）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当月第一天的零点</returns>
        public static DateTime MonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind);
        }

        /// <summary>
        /// 获取指定日期所在月的结束时间（23:59:59.999）
        /// </summary>
        public static DateTime MonthEnd(this DateTime dt)
        {
            int days = DateTime.DaysInMonth(dt.Year, dt.Month);
            return new DateTime(dt.Year, dt.Month, days, 23, 59, 59, 999, dt.Kind);
        }

        /// <summary>
        /// 获取指定日期所在月的结束时间（23:59:59.999）
        /// </summary>
        public static DateTimeOffset MonthEnd(this DateTimeOffset dt)
        {
            int days = DateTime.DaysInMonth(dt.Year, dt.Month);
            return new DateTimeOffset(dt.Year, dt.Month, days, 23, 59, 59, 999, dt.Offset);
        }

        /// <summary>
        /// 清除月份以下的时间部分（保留到年）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>年初时间（yyyy-01-01 00:00:00）</returns>
        public static DateTime EffaceMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1, 0, 0, 0, dt.Kind);
        }

        /// <summary>
        /// 清除日期以下的时间部分（保留到月）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>月初时间（yyyy-MM-01 00:00:00）</returns>
        public static DateTime EffaceDay(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, dt.Kind);
        }

        /// <summary>
        /// 清除小时以下的时间部分（保留到日）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当日零点（yyyy-MM-dd 00:00:00）</returns>
        public static DateTime EffaceHour(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Kind);
        }

        /// <summary>
        /// 清除分钟以下的时间部分（保留到小时）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当前小时起始时间（yyyy-MM-dd HH:00:00）</returns>
        public static DateTime EffaceMinute(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, dt.Kind);
        }

        /// <summary>
        /// 清除秒以下的时间部分（保留到分钟）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当前分钟起始时间（yyyy-MM-dd HH:mm:00）</returns>
        public static DateTime EffaceSecond(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Kind);
        }

        /// <summary>
        /// 清除毫秒部分（保留到秒）
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <returns>当前秒起始时间（yyyy-MM-dd HH:mm:ss.000）</returns>
        public static DateTime EffaceMillisecond(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Kind);
        }

        /// <summary>
        /// 根据指定精度清除时间部分
        /// </summary>
        /// <param name="dt">目标日期</param>
        /// <param name="type">清除精度类型</param>
        /// <returns>按精度清除后的时间</returns>
        public static DateTime Efface(this DateTime dt, DateTimeEffaceType type)
        {
            return type switch
            {
                DateTimeEffaceType.Month => dt.EffaceMonth(),
                DateTimeEffaceType.Day => dt.EffaceDay(),
                DateTimeEffaceType.Hour => dt.EffaceHour(),
                DateTimeEffaceType.Minute => dt.EffaceMinute(),
                DateTimeEffaceType.Second => dt.EffaceSecond(),
                DateTimeEffaceType.Millisecond => dt.EffaceMillisecond(),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        // DateTimeOffset 版本的 Efface 方法（实现逻辑相同）
        /// <summary>
        /// 清除月份以下的时间部分（保留到年）
        /// </summary>
        public static DateTimeOffset EffaceMonth(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, 1, 1, 0, 0, 0, dt.Offset);
        }

        /// <summary>
        /// 清除日期以下的时间部分（保留到月）
        /// </summary>
        public static DateTimeOffset EffaceDay(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, 1, 0, 0, 0, dt.Offset);
        }

        /// <summary>
        /// 清除小时以下的时间部分（保留到日）
        /// </summary>
        public static DateTimeOffset EffaceHour(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, dt.Day, 0, 0, 0, dt.Offset);
        }

        /// <summary>
        /// 清除分钟以下的时间部分（保留到小时）
        /// </summary>
        public static DateTimeOffset EffaceMinute(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0, dt.Offset);
        }

        /// <summary>
        /// 清除秒以下的时间部分（保留到分钟）
        /// </summary>
        public static DateTimeOffset EffaceSecond(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0, dt.Offset);
        }

        /// <summary>
        /// 清除毫秒部分（保留到秒）
        /// </summary>
        public static DateTimeOffset EffaceMillisecond(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Offset);
        }

        /// <summary>
        /// 根据指定精度清除时间部分
        /// </summary>
        public static DateTimeOffset Efface(this DateTimeOffset dt, DateTimeEffaceType type)
        {
            return type switch
            {
                DateTimeEffaceType.Month => dt.EffaceMonth(),
                DateTimeEffaceType.Day => dt.EffaceDay(),
                DateTimeEffaceType.Hour => dt.EffaceHour(),
                DateTimeEffaceType.Minute => dt.EffaceMinute(),
                DateTimeEffaceType.Second => dt.EffaceSecond(),
                DateTimeEffaceType.Millisecond => dt.EffaceMillisecond(),
                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }

        /// <summary>
        /// 获取指定日期所在周的起始时间（周一 00:00:00）
        /// </summary>
        /// <param name="nowTime">目标日期</param>
        /// <returns>本周一的零点</returns>
        public static DateTime WeekStart(this DateTime nowTime)
        {
            int delta = (7 + (nowTime.DayOfWeek - DayOfWeek.Monday)) % 7;
            return nowTime.AddDays(-delta).Date;
        }

        /// <summary>
        /// 获取指定日期所在周的结束时间（周日 23:59:59.999）
        /// </summary>
        /// <param name="nowTime">目标日期</param>
        /// <returns>本周日的最后一毫秒</returns>
        public static DateTime WeekEnd(this DateTime nowTime)
        {
            return nowTime.WeekStart().AddDays(7).AddMilliseconds(-1);
        }
    }

    /// <summary>
    /// 时间清除精度枚举
    /// </summary>
    public enum DateTimeEffaceType
    {
        /// <summary> 保留到年（yyyy-01-01 00:00:00） </summary>
        Month,
        /// <summary> 保留到月（yyyy-MM-01 00:00:00） </summary>
        Day,
        /// <summary> 保留到日（yyyy-MM-dd 00:00:00） </summary>
        Hour,
        /// <summary> 保留到小时（yyyy-MM-dd HH:00:00） </summary>
        Minute,
        /// <summary> 保留到分钟（yyyy-MM-dd HH:mm:00） </summary>
        Second,
        /// <summary> 保留到秒（yyyy-MM-dd HH:mm:ss.000） </summary>
        Millisecond
    }
}