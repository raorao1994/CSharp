using System;
using System.Text;

namespace PublicLibrary
{
    /// <summary>
    /// DateTimeEx
    /// 功能：操作DateTime的一些静态方法。
    /// 作者：raoaro
    /// 时间：2018年2月8日
    /// </summary>
    public static class DateTimeEx
    {
        /// <summary>
        /// 得到中文形式的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChineseDate(DateTime dt)
        {
            return dt.Year.ToString() + "年" + dt.Month.ToString() + "月" + dt.Day.ToString() + "日";
        }

        /// <summary>
        /// 得到中文形式的时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChineseTime(DateTime dt)
        {
            return dt.Hour.ToString() + "点" + dt.Minute.ToString() + "分" + dt.Second.ToString() + "秒";
        }

        /// <summary>
        /// 得到中文形式的日期时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChineseDateTime(DateTime dt)
        {
            return GetChineseDate(dt) + GetChineseTime(dt);
        }

        /// <summary>
        /// 得到某月的最后一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回最后一天的日期</returns>
        public static DateTime GetLastDateOfMonth(DateTime dt)
        {
            return GetLastDateOfMonth(dt.Year, dt.Month);
        }

        /// <summary>
        /// 得到某月的最后一天
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>返回最后一天的日期</returns>
        public static DateTime GetLastDateOfMonth(int year, int month)
        {
            if (month < 1) month = 1;
            if (month > 12) month = 12;
            return (new DateTime(year, month, 1)).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 得到某月的最后一豪秒
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回最后一豪秒的日期</returns>
        public static DateTime GetLastDateTimeOfMonth(DateTime dt)
        {
            return GetLastDateTimeOfMonth(dt.Year, dt.Month);
        }

        /// <summary>
        /// 得到某月的最后一豪秒
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>返回最后一豪秒的日期</returns>
        public static DateTime GetLastDateTimeOfMonth(int year, int month)
        {
            if (month < 1) month = 1;
            if (month > 12) month = 12;
            return (new DateTime(year, month, 1)).AddMonths(1).AddSeconds(-1);
        }

        /// <summary>
        /// 得到某月的第一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回第一天的日期</returns>
        public static DateTime GetFirstDateOfMonth(DateTime dt)
        {
            return GetFirstDateOfMonth(dt.Year, dt.Month);
        }

        /// <summary>
        /// 得到某月的第一天
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns>返回第一天的日期</returns>
        public static DateTime GetFirstDateOfMonth(int year, int month)
        {
            if (month < 1) month = 1;
            if (month > 12) month = 12;
            return new DateTime(year, month, 1);
        }

        /// <summary>
        /// 得到某年的第一天
        /// </summary>
        /// <param name="year">年</param>
        /// <returns>返回第一天的日期</returns>
        public static DateTime GetFirstDateOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        /// <summary>
        /// 得到某年的第一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回第一天的日期</returns>
        public static DateTime GetFirstDateOfYear(DateTime dt)
        {
            return GetFirstDateOfYear(dt.Year);
        }

        /// <summary>
        /// 得到某年的最后一天
        /// </summary>
        /// <param name="year">年</param>
        /// <returns>返回最后一天的日期</returns>
        public static DateTime GetLastDateOfYear(int year)
        {
            return new DateTime(year, 12, 31);
        }

        /// <summary>
        /// 得到某年的最后一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回最后一天的日期</returns>
        public static DateTime GetLastDateOfYear(DateTime dt)
        {
            return GetLastDateOfYear(dt.Year);
        }

        /// <summary>
        /// 得到某年的最后一毫秒
        /// </summary>
        /// <param name="year">年</param>
        /// <returns>返回最后一毫秒的日期时间对象</returns>
        public static DateTime GetLastDateTimeOfYear(int year)
        {
            return new DateTime(year + 1, 1, 1).AddSeconds(-1);
        }

        /// <summary>
        /// 得到某年的最后一毫秒
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回最后一毫秒的日期时间对象</returns>
        public static DateTime GetLastDateTimeOfYear(DateTime dt)
        {
            return GetLastDateTimeOfYear(dt.Year);
        }

        /// <summary>
        /// 计算某段时间内，每月的天数
        /// </summary>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回天数</returns>
        public static float GetDaysOfMonth(DateTime beginTime, DateTime endTime)
        {
            float days;
            if (beginTime.Year == endTime.Year && beginTime.Month == endTime.Month)
            {
                //如果是同一个月，返回这个月的天数
                DateTime t1 = new DateTime(beginTime.Year, beginTime.Month, 1);
                DateTime t2 = t1.AddMonths(1);
                TimeSpan interval = t2.Subtract(t1);
                days = interval.Days;
            }
            else if (beginTime.Year == endTime.Year)
            {
                //如果是同一年，返回这一年的平均月天数
                DateTime t1 = new DateTime(beginTime.Year, 1, 1);
                DateTime t2 = new DateTime(beginTime.Year + 1, 1, 1);
                TimeSpan interval = t2.Subtract(t1);
                days = (float)(1.0 * interval.Days / 12);
            }
            else
            {
                //否则返回每4年的平均月天数
                days = (float)((365.0 * 3 + 366) / 48);
            }
            return days;
        }

        /// <summary>
        /// 得到某日所在周的第一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回某日所在周的第一天</returns>
        public static DateTime GetFirstDateOfWeek(DateTime dt)
        {
            int days = (int)dt.DayOfWeek;
            TimeSpan ts = new TimeSpan(days, 0, 0, 0);
            return dt.Date - ts;
        }

        /// <summary>
        /// 得到某日所在周的最后一天
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回某日所在周的最后一天</returns>
        public static DateTime GetLastDateOfWeek(DateTime dt)
        {
            int days = 6 - (int)dt.DayOfWeek;
            TimeSpan ts = new TimeSpan(days, 0, 0, 0);
            return dt.Date + ts;
        }

        /// <summary>
        /// 得到某日所在周的最后一豪秒
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns>返回某日所在周的最后一毫秒</returns>
        public static DateTime GetLastDateTimeOfWeek(DateTime dt)
        {
            int days = 6 - (int)dt.DayOfWeek;
            TimeSpan ts = new TimeSpan(days, 0, 0, 0);
            DateTime lastDate = dt.Date + ts;
            return new DateTime(lastDate.Year, lastDate.Month, lastDate.Day, 23, 59, 59, 59);
        }

        /// <summary>
        /// 得到本月已经过去的天数
        /// </summary>
        /// <returns>返回本月已经过去的天数</returns>
        public static int GetThisMonthPastDays()
        {
            int days;
            DateTime today = DateTime.Today;
            DateTime firstDayOfCurMonth = DateTimeEx.GetFirstDateOfMonth(today);
            TimeSpan ts = today - firstDayOfCurMonth;
            days = (int)ts.TotalDays;
            if (days < 1)
                days = 1;
            return days;
        }

        /// <summary>
        /// 得到本年已经过去的天数
        /// </summary>
        /// <returns>返回本年已经过去的天数</returns>
        public static int GetThisYearPastDays()
        {
            int days;
            DateTime today = DateTime.Today;
            DateTime firstDayOfCurYear = DateTimeEx.GetFirstDateOfYear(today);
            TimeSpan ts = today - firstDayOfCurYear;
            days = (int)ts.TotalDays;
            if (days < 1) days = 1;
            return days;
        }

        /// <summary>
        /// 得到本周已经过去的天数
        /// </summary>
        /// <returns>返回本周已经过去的天数</returns>
        public static int GetThisWeekPastDays()
        {
            int days;
            DateTime today = DateTime.Today;
            DateTime firstDayOfCurWeek = DateTimeEx.GetFirstDateOfWeek(today);
            TimeSpan ts = today - firstDayOfCurWeek;
            days = (int)ts.TotalDays;
            if (days < 1) days = 1;
            return days;
        }

        /// <summary>
        /// 得到某日所在季度的第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetFirstDateOfQuarter(DateTime dt)
        {
            int year = dt.Year;
            int month;
            int dtMonth = dt.Month;
            if (dtMonth == 1 || dtMonth == 2 || dtMonth == 3)
                month = 1;
            else if (dtMonth == 4 || dtMonth == 5 || dtMonth == 6)
                month = 4;
            else if (dtMonth == 7 || dtMonth == 8 || dtMonth == 9)
                month = 7;
            else
                month = 10;
            return new DateTime(year, month, 1);
        }

        /// <summary>
        /// 得到某日所在季度的最后一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDateOfQuarter(DateTime dt)
        {
            return GetFirstDateOfQuarter(dt).AddMonths(3).AddDays(-1);	//即下一季度第1天减1天
        }

        /// <summary>
        /// 得到某日所在季度的最后一天的最后一秒
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetLastDateTimeOfQuarter(DateTime dt)
        {
            return GetFirstDateOfQuarter(dt).AddMonths(3).AddSeconds(-1);	//即下一季度第1天减1秒
        }

        /// <summary>
        /// 转换成中文形式的时间间隔“xx天xx时xx分xx秒”
        /// </summary>
        /// <param name="ts">时间间隔</param>
        /// <param name="showSeconds">是否显示秒</param>
        /// <returns>返回中文形式的时间间隔</returns>
        public static string GetChineseTimeSpan(TimeSpan ts, bool showSeconds)
        {
            int days, hours, minutes, seconds;
            StringBuilder sbTs = new StringBuilder();
            if (ts.Ticks == 0)
            {
                days = 0;
                hours = 0;
                minutes = 0;
                seconds = 0;
            }
            else
            {
                //如果时间间隔为负，将其转成正数
                if (ts.Ticks < 0)
                {
                    sbTs.Append("-");
                    ts = ts.Negate();
                }
                //计算天、小时、分钟和秒
                days = ts.Days;
                if (days != 0)
                    ts = ts.Subtract(new TimeSpan(days, 0, 0, 0));
                hours = ts.Hours;
                if (hours != 0)
                    ts = ts.Subtract(new TimeSpan(0, hours, 0, 0));
                minutes = ts.Minutes;
                if (minutes != 0)
                    ts = ts.Subtract(new TimeSpan(0, 0, minutes, 0));
                seconds = ts.Seconds;
            }
            sbTs.AppendFormat("{0}天{1}时{2}分{3}", days, hours, minutes, showSeconds ? (seconds.ToString() + "秒") : "");
            return sbTs.ToString();
        }

        /// <summary>
        /// 获取日期所在年的周数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            DateTime yearD = new DateTime(dt.Year, 1, 1);
            int diff = Convert.ToInt32(yearD.DayOfWeek);
            int dayOfYear = dt.DayOfYear + diff;
            return (int)(Math.Ceiling(dayOfYear / 7.0));
        }
    }
}
