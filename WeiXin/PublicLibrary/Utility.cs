using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace PublicLibrary
{
    /// <summary>
    /// 帮助类
    /// </summary>
    public class Utility
    {
        #region 其他
        //静态成员
        //“显示信息”页的url
        public static string ShowMessageUrl = "~/ShowMessage.aspx";
        /// <summary>
        /// 生成”显示信息“页的含参数地址
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <returns>返回生成的url</returns>
        public static string GenerateShowMessageUrl(string title, string description)
        {
            HttpServerUtility server = HttpContext.Current.Server;
            return ShowMessageUrl + "?Title=" + server.UrlEncode(title) + "&Description=" + server.UrlEncode(description);
        }

        /// <summary>
        /// 显示客户端提示对话框信息
        /// </summary>
        /// <param name="scriptKey">脚本的键-用于避免重复注册客户端脚本</param>
        /// <param name="alertText">提示文本</param>
        public static void ShowClientAlert(Page page, string scriptKey, string alertText)
        {
            alertText = alertText.Replace("\"", "\\\"");
            ClientScriptManager cs = page.ClientScript;
            if (!cs.IsStartupScriptRegistered(cs.GetType(), scriptKey))
                cs.RegisterStartupScript(cs.GetType(), scriptKey, string.Format("<script language=\"javascript\" type=\"text/javascript\">alert(\"{0}\");</script>\r\n", alertText));
        }

        /// <summary>
        /// 显示客户端是与否的对话框
        /// </summary>
        /// <param name="scriptKey">脚本的键-用于避免重复注册客户端脚本</param>
        /// <param name="alertText">提示文本</param>
        public static void ShowConfirm(Page page, string scriptKey, string alertText, string doText)
        {
            alertText = alertText.Replace("\"", "\\\"");
            ClientScriptManager cs = page.ClientScript;
            if (!cs.IsStartupScriptRegistered(cs.GetType(), scriptKey))
                cs.RegisterStartupScript(cs.GetType(), scriptKey, string.Format("<script>if(window.confirm('{0}')){1}</script>", alertText, doText));
        }

        #endregion

        #region 字符串操作
        /// <summary>
        /// 得到GUID字符串，去掉“-”号，转换成大写字母
        /// </summary>
        /// <param name="guid">guid对象</param>
        /// <returns>返回GUID字符串</returns>
        public static string GetGuidString(Guid guid)
        {
            return guid.ToString().Replace("-", "").ToUpper();
        }

        /// <summary>
        /// 得到一个新的GUID字符串，去掉“-”号，转换成大写字母
        /// </summary>
        /// <returns>返回GUID字符串</returns>
        public static string GetNewGuidString()
        {
            return GetGuidString(Guid.NewGuid());
        }

        /// <summary>
        /// 从Oracle数据库的BLOB类型中获取字符串
        /// </summary>
        /// <param name="blob">BLOB类型的值</param>
        /// <returns>返回字符串</returns>
        public static string GetStringFromOracleBlob(object blob)
        {
            return (blob == null || Convert.IsDBNull(blob)) ? "" : UTF8Encoding.UTF8.GetString((byte[])blob);
        }

        /// <summary>
        /// 从字符串中获取Oracle数据库的BLOB类型
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回字节数组</returns>
        public static byte[] GetOracleBlobFromString(string str)
        {
            return string.IsNullOrEmpty(str) ? null : UTF8Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// 判断列表中是否存在重复的元素
        /// </summary>
        /// <typeparam name="T">列表的类型，必须为实现了IComparable接口的可比较类型</typeparam>
        /// <param name="list">列表</param>
        /// <returns>返回是否包含重复元素</returns>
        public static bool IsRepeated<T>(List<T> list) where T : IComparable<T>
        {
            if (list != null && list.Count > 1)
            {
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        if (list[i].CompareTo(list[j]) == 0)
                            return true;
                    }
                }
            }
            return false;
        } 
        #endregion

        #region 加密
        /// <summary>
        /// 根据指定的密码和哈希算法生成一个适合于存储在配置文件中的哈希密码
        /// </summary>
        /// <param name="str">要进行哈希运算的密码</param>
        /// <param name="type"> 要使用的哈希算法</param>
        /// <returns>经过哈希运算的密码</returns>
        public static string HashPasswordForStoringInConfigFile(string str, string type)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, type);
        }
        /// <summary>
        /// 根据指定的密码和哈希算法生成SHA1哈希密码
        /// </summary>
        /// <param name="str">要进行哈希运算的密码</param>
        /// <returns>经过哈希运算的密码</returns>
        public static string HashPasswordForStoringInConfigFile(string str)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            return str_sha1_out;
        }
        /// <summary>
        /// 对指定字符串进行MD5加密（采用.net的方式）
        /// </summary>
        /// <param name="source">需要被加密的字符串</param>
        /// <param name="source">输出参数，返回加密之后的结果</param>
        /// <returns>返回是否加密成功</returns>
        public static bool MD5(string source, out string result)
        {
            bool bSuccessed = false;
            result = "";
            try
            {
                byte[] byteSource = Encoding.Default.GetBytes(source);	//使用默认的ANSI编码
                System.Security.Cryptography.MD5 md5 = new MD5CryptoServiceProvider();				//创建MD5对象
                byte[] byteHash = md5.ComputeHash(byteSource);			//进行HASH运算
                result = BitConverter.ToString(byteHash);				//将得到的结果转化为字符串
                result = result.Replace("-", "");						//去掉结果中的减号“-”（以下2步是为了和ASP模式下的MD5兼容而设置）
                result = result.ToLower();								//将结果转化为小写
                md5.Clear();
                bSuccessed = true;
            }
            catch
            {
                bSuccessed = false;
            }
            if (!bSuccessed)
                result = "";
            return bSuccessed;
        }
        #endregion

        public static string GetConnectionStrings()
        {
            return ConfigurationManager.ConnectionStrings["strCon"].ToString();
        }
        public static string GetAppSettings()
        {
            return ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}
