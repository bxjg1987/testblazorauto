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
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        public static string GetAgeString(this DateTime p_FirstDateTime)
        {
            return CalculateAgeString(p_FirstDateTime, DateTime.Now, null);
        }
        /// <summary>
        /// 获得年龄字符串：某个日期点到今天的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        /// <param name="p_Format">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, string p_ReturnFormat)
        {
            return CalculateAgeString(p_FirstDateTime, DateTime.Now, p_ReturnFormat);
        }
        /// <summary>
        /// 获得年龄字符串：两个日期点之间的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        /// <param name="p_SecondDateTime">第2个日期参数</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime)
        {
            return CalculateAgeString(p_FirstDateTime, p_SecondDateTime, null);
        }
        /// <summary>
        /// 获得年龄字符串：两个日期点之间的年龄
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        /// <param name="p_SecondDateTime">第2个日期参数</param>
        /// <param name="p_Format">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string GetAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime, string p_ReturnFormat)
        {
            return CalculateAgeString(p_FirstDateTime, p_SecondDateTime, p_ReturnFormat);
        }
        /// <summary>
        /// 计算年龄字符串
        /// 默认返回：xx岁xx月xx天
        /// </summary>
        /// <param name="p_FirstDateTime">第1个日期参数</param>
        /// <param name="p_SecondDateTime">第2个日期参数</param>
        /// <param name="p_Format">返回字符串的格式，默认为：{0}岁{1}月{2}天</param>
        public static string CalculateAgeString(this DateTime p_FirstDateTime, DateTime p_SecondDateTime, string p_ReturnFormat)
        {
            //判断返回字符串的格式。若为空，则给默认值：{0}岁{1}月{2}天
            if (string.IsNullOrWhiteSpace(p_ReturnFormat)) p_ReturnFormat = "{0}岁{1}月{2}天";
            var r = p_FirstDateTime.CalculateAge(p_SecondDateTime);
            return string.Format(p_ReturnFormat, r.years, r.months, r.days);
        }

        public static (int years, int months, int days) CalculateAge(this DateTime p_FirstDateTime, DateTime p_SecondDateTime)
        {
            //判断时间段是否为正。若为负，调换两个时间点的位置。
            if (DateTime.Compare(p_FirstDateTime, p_SecondDateTime) > 0)
            {
                DateTime stmpDateTime = p_FirstDateTime;
                p_FirstDateTime = p_SecondDateTime;
                p_SecondDateTime = stmpDateTime;
            }

            //判断返回字符串的格式。若为空，则给默认值：{0}岁{1}月{2}天
            // if (string.IsNullOrEmpty(p_ReturnFormat)) p_ReturnFormat = "{0}岁{1}月{2}天";

            //定义：年、月、日
            int year, month, day;

            //计算：天
            day = p_SecondDateTime.Day - p_FirstDateTime.Day;
            if (day < 0)
            {
                day += DateTime.DaysInMonth(p_FirstDateTime.Year, p_FirstDateTime.Month);
                p_FirstDateTime = p_FirstDateTime.AddMonths(1);
            }
            //计算：月
            month = p_SecondDateTime.Month - p_FirstDateTime.Month;
            if (month < 0)
            {
                month += 12;
                p_FirstDateTime = p_FirstDateTime.AddYears(1);
            }
            //计算：年
            year = p_SecondDateTime.Year - p_FirstDateTime.Year;

            //返回格式化后的结果
            return (year, month, day);
            //return string.Format(p_ReturnFormat, year, month, day);
        }

        #endregion

        /// <summary>
        /// 月初
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTimeOffset MonthStart(this DateTimeOffset dt)
        {
            return new DateTimeOffset(dt.Year, dt.Month, 1, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, dt.Offset);
        }

        //Abp.Extensions.DateTimeExtensions有时间相关的扩展方法

        /// <summary>
        /// 月初
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime MonthStart(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }
        /// <summary>
        /// 月末
        /// </summary>
        /// <param name="startMonth"></param>
        /// <returns></returns>
        public static DateTime MonthEnd(this DateTime startMonth)
        {
            return startMonth.MonthStart().AddMonths(1).AddDays(-1).AddMilliseconds(-1);  //本月月末
        }
        /// <summary>
        /// 月末
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTimeOffset MonthEnd(this DateTimeOffset dt)
        {
            // 获取该月的第一天
            DateTimeOffset monthStart = dt.MonthStart();

            // 将日期增加一个月并减少一天
            DateTimeOffset monthEnd = monthStart.AddMonths(1).AddDays(-1);

            // 设置时间为当天的 23:59:59.999
            monthEnd = monthEnd.Date.AddMilliseconds(-1);

            // 返回月末的时间
            return monthEnd;
        }
        public static DateTime EffaceMonth(this DateTime dt)
        {
            return dt.AddMonths(-dt.Month);
        }
        public static DateTime EffaceDay(this DateTime dt)
        {
            return dt.AddDays(-dt.Day);
        }
        public static DateTime EffaceHour(this DateTime dt)
        {
            return dt.AddHours(-dt.Hour);
        }
        public static DateTime EffaceMinute(this DateTime dt)
        {
            return dt.AddMinutes(-dt.Minute);
        }
        public static DateTime EffaceSecond(this DateTime dt)
        {
            return dt.AddSeconds(-dt.Second);
        }
        public static DateTime EffaceMillisecond(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }
        /// <summary>
        /// 抹去时间的后面部分
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime Efface(this DateTime dt, DateTimeEffaceType type)
        {
            if (type <= DateTimeEffaceType.Millisecond)
                dt = dt.EffaceMillisecond();
            if (type <= DateTimeEffaceType.Second)
                dt = dt.EffaceSecond();
            if (type <= DateTimeEffaceType.Minute)
                dt = dt.EffaceMinute();
            if (type <= DateTimeEffaceType.Hour)
                dt = dt.EffaceHour();
            if (type <= DateTimeEffaceType.Day)
                dt = dt.EffaceDay();
            if (type <= DateTimeEffaceType.Month)
                dt = dt.EffaceMonth();
            return dt;
        }

        public static DateTimeOffset EffaceMonth(this DateTimeOffset dt)
        {
            return dt.AddMonths(-dt.Month);
        }
        public static DateTimeOffset EffaceDay(this DateTimeOffset dt)
        {
            return dt.AddDays(-dt.Day);
        }
        public static DateTimeOffset EffaceHour(this DateTimeOffset dt)
        {
            return dt.AddHours(-dt.Hour);
        }
        public static DateTimeOffset EffaceMinute(this DateTimeOffset dt)
        {
            return dt.AddMinutes(-dt.Minute);
        }
        public static DateTimeOffset EffaceSecond(this DateTimeOffset dt)
        {
            return dt.AddSeconds(-dt.Second);
        }
        public static DateTimeOffset EffaceMillisecond(this DateTimeOffset dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }
        /// <summary>
        /// 抹去时间的后面部分
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTimeOffset Efface(this DateTimeOffset dt, DateTimeEffaceType type)
        {
            if (type <= DateTimeEffaceType.Millisecond)
                dt = dt.EffaceMillisecond();
            if (type <= DateTimeEffaceType.Second)
                dt = dt.EffaceSecond();
            if (type <= DateTimeEffaceType.Minute)
                dt = dt.EffaceMinute();
            if (type <= DateTimeEffaceType.Hour)
                dt = dt.EffaceHour();
            if (type <= DateTimeEffaceType.Day)
                dt = dt.EffaceDay();
            if (type <= DateTimeEffaceType.Month)
                dt = dt.EffaceMonth();
            return dt;
        }

        /// <summary>
        /// 本周一
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static DateTime WeekStart(this DateTime nowTime)
        {
            //return dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));
            #region 获取本周第一天
            //星期一为第一天  
            int weeknow = Convert.ToInt32(nowTime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            return nowTime.Date.AddDays(daydiff);
            #endregion
        }
        /// <summary>
        /// 本周最后一天
        /// </summary>
        /// <param name="nowTime"></param>
        /// <returns></returns>
        public static DateTime WeekEnd(this DateTime nowTime)
        {
            //星期天为最后一天  
            int lastWeekDay = Convert.ToInt32(nowTime.DayOfWeek);
            lastWeekDay = lastWeekDay == 0 ? (7 - lastWeekDay) : lastWeekDay;
            int lastWeekDiff = (7 - lastWeekDay);

            //本周最后一天  
            return nowTime.Date.AddDays(lastWeekDiff + 1).AddMilliseconds(-1);
        }
    }

    public enum DateTimeEffaceType
    {
        Month,
        Day,
        Hour,
        Minute,
        Second,
        Millisecond
    }
}
