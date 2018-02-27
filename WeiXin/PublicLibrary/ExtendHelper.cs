using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicLibrary
{
    public static class ExtendHelper
    {
        /// <summary>
        /// string判断字符串是否为Null或者“”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 将guid转为string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            return new Guid(str);
        }
        /// <summary>
        /// 将guid转为string无横杆
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GuidToStringN(this Guid guid)
        {
            return guid.ToString("N");
        }
        /// <summary>
        /// 不分大小写对比
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEquals(this string str1, string str)
        {
            return str1.Equals(str, StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// ToInt32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(str);
            }
        }
        /// <summary>
        /// 判断是否为null为null时输出“”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IsNULL(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            else
            {
                return str;
            }
        }
    }
}
