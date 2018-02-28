using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace PublicLibrary
{
    /// <summary>
    /// StringEx
    /// 功能：string辅助类。
    /// 作者：raoaro
    /// 时间：2018年2月8日
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 作用：去掉字符串中最后包含的子字符串
        /// 如果字符串source的最后是子字符串value，则去掉最后的子字符串value；否则原样返回source。
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="value">需要被去掉的子字符串</param>
        /// <returns>返回的结果字符串</returns>
        public static string RemoveEndsWith(string source, string value)
        {
            string result;
            if (source.EndsWith(value))
            {
                int pos = source.LastIndexOf(value);
                result = source.Substring(0, pos);
            }
            else
                result = source;
            return result;
        }

        /// <summary>
        /// 作用：去掉字符串中开始包含的子字符串
        /// 如果字符串source的开始是子字符串value，则去掉开始的子字符串value；否则原样返回source。
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="value">需要被去掉的子字符串</param>
        /// <returns>返回的结果字符串</returns>
        public static string RemoveStartsWith(string source, string value)
        {
            string result;
            if (source.StartsWith(value))
            {
                int length = source.Length - value.Length;
                result = source.Substring(value.Length, length);
            }
            else
                result = source;
            return result;
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="length">截取长度</param>
        /// <returns>返回截取之后的字符串</returns>
        public static string Cut(string source, int length)
        {
            string result = source;
            if (result.Length > length)
            {
                if (length > 3)
                    result = result.Substring(0, length - 3) + "...";
                else
                    result = result.Substring(0, length);
            }
            return result;
        }

        /// <summary>
        /// 作用：重复指定的字符串若干次，并返回结果。例如 RepeatString("a",3) 返回"aaa"
        /// </summary>
        /// <param name="source">需要被重复的字符串</param>
        /// <param name="repeatCount">重复次数</param>
        /// <returns>返回结果字符串</returns>
        public static string RepeatString(string source, int repeatCount)
        {
            StringBuilder sbResult = new StringBuilder();
            for (int i = 0; i < repeatCount; i++)
            {
                sbResult.Append(source);
            }
            return sbResult.ToString();
        }


        /// <summary>
        /// 清除HTML标签的多余
        /// </summary>
        /// <param name="el">标签名</param>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        private static string repElement(string el, string str)
        {
            string pat = @"<" + el + "[^>]+>";
            string rep = "";
            str = Regex.Replace(str.ToString(), pat, rep);
            return str;
        }


        /// <summary>
        /// 里面全是要清理的html标签
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string rep(string str)
        {
            string[] el = new string[] { "p", "span", "strong", "FONT", "P", "SPAN", "STRONG" };
            foreach (string s in el)
            {
                str = repElement(s, str);
            }
            return str;
        }


        ///   <summary>   
        ///   返回一个数组长度为3的字符串数组   
        ///   </summary>   
        ///   <returns>GetWeekDay[0]=周次;GetWeekDay[1]=该周第一天;GetWeekDay[2]=该周最后一天</returns>   
        public static string[] GetWeekDay(DateTime rq)
        {
            string[] inti = new string[3];
            DateTime day = DateTime.Parse(rq.Year + "-1-1");
            System.DayOfWeek dateTime = day.DayOfWeek;
            int DayCount = DateTime.Today.DayOfYear;
            int i = (DayCount + Convert.ToInt32(dateTime) - 2) / 7 + 1;
            inti[0] = i.ToString();

            inti[1] = day.AddDays(DayCount - 1).ToString("yyyy-MM-dd");
            inti[2] = day.AddDays(DayCount + 5).ToString("yyyy-MM-dd");

            return inti;
        }

        /// <summary>
        /// 去除HTML代码
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string LostHTML(string Str)
        {
            string Re_STR = "";
            if (Str != null)
            {
                if (Str != string.Empty)
                {
                    string pattern = "<\v*[^<>]*>";
                    Re_STR = Regex.Replace(Str, pattern, "");

                }
            }

            return (Re_STR.Replace("\\r\\n", "")).Replace("\\r", "");
        }
        /// <summary>
        /// json格式字符串转对应对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T JsonStrToModel<T>(this string str) where T : new()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Deserialize<T>(str);
        }
        /// <summary>
        /// 实体对象转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ModelToJsonStr(object model)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            return json.Serialize(model);
        }
    }
}
