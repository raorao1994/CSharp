using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools
{
    public static class RegexHelper
    {

        #region 正则匹配
        /// <summary>
        /// 返回单个正则匹配值
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="regex">正则表达式字符串</param>
        /// <returns>字符串类型</returns>
        public static string ToRegexString(this string value, string regex)
        {

            if (value != null)
            {
                Regex re = new Regex(regex);
                Match m = re.Match(value);
                if (m.Success)
                {
                    return m.Value;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// 返回正则匹配字符串数组
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="regex">正刚表达式</param>
        /// <returns>字符串数组</returns>
        public static String[] ToRegexStringArray(this string value, string regex)
        {
            String[] array = { };
            if (value != null)
            {
                //兼容多行匹配模式
                Regex rg = new Regex(regex, RegexOptions.Multiline);
                MatchCollection mc = rg.Matches(value);
                if (mc.Count > 0)
                {
                    int group = mc.Count;
                    array = new String[group];
                    for (int i = 0; i < group; i++)
                    {
                        array[i] = mc[i].Value;
                    }
                }

            }
            return array;
        }
        /// <summary>
        /// 判断是否匹配
        /// </summary>
        /// <param name="value">字符串</param>
        /// <param name="regex">正则表达式</param>
        /// <returns>bool</returns>
        public static bool IsRegex(this string value, string regex)
        {
            if (value != null)
            {
                Regex reg = new Regex(regex);
                return reg.IsMatch(value);
            }
            else
            {
                return false;
            }

        } 
        #endregion

        #region 验证输入字符串是否与模式字符串匹配
         /// <summary>
         /// 验证输入字符串是否与模式字符串匹配，匹配返回true
         /// </summary>
         /// <param name="input">输入字符串</param>
         /// <param name="pattern">模式字符串</param>        
         public static bool IsMatch(string input, string pattern)
         {
             return IsMatch(input, pattern, RegexOptions.IgnoreCase);
         }
 
         /// <summary>
         /// 验证输入字符串是否与模式字符串匹配，匹配返回true
         /// </summary>
         /// <param name="input">输入的字符串</param>
         /// <param name="pattern">模式字符串</param>
         /// <param name="options">筛选条件</param>
         public static bool IsMatch(string input, string pattern, RegexOptions options)
         {
             return Regex.IsMatch(input, pattern, options);
         }
         #endregion
    }
}
