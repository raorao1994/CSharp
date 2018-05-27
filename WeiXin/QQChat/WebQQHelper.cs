using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QQChat
{
    public class WebQQHelper
    {
        /// <summary>
        /// 获取hash值的算法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public string Hash(string b, string j)
        {
            string a = j + "password error";
            string i = "";
            List<int> E = new List<int>();
            while (true)
            {
                if (i.Length <= a.Length)
                {
                    i += b;
                    if (i.Length == a.Length)
                    {
                        break;
                    }
                }
                else
                {
                    i = i.Substring(0, a.Length);
                    break;
                }

            }
            for (int c = 0; c < i.Length; c++)
            {
                int tmp = (char)i[c] ^ (char)a[c];
                E.Add(tmp);
            }
            string[] seed = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            i = "";
            for (int c = 0; c < E.Count; c++)
            {
                i += seed[E[c] >> 4 & 15];
                i += seed[E[c] & 15];
            }

            return i;
        }

        public bool check()
        {
            string url = "https://ssl.ptlogin2.qq.com/check?uin ={$uin}&appid = 1003903 & js_ver = 10092 & js_type = 0 & login_sig = K5F0E8woS74td4sRIqKiSHmH6B2RYYP467z2r * 6YWaH4wc7vE * 4G* X7V2kGP9s1*&u1 = http % 3A % 2F % 2Fweb2.qq.com % 2Floginproxy.html & r = 0.2689784204121679";
            //url = url.Replace("{$uin}", this.qq);
            //HttpItem item = new HttpItem()
            //{
            //    URL = url,//URL     必需项    
            //    Encoding = System.Text.Encoding.GetEncoding("utf-8"),//URL     可选项 默觉得Get   
            //    Method = "get",//URL     可选项 默觉得Get   
            //    IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写   
            //    Timeout = 100000,//连接超时时间     可选项默觉得100000    
            //    ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默觉得30000   
            //    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本号，操作系统     可选项有默认值   
            //    ContentType = "application/x-www-form-urlencoded",//返回类型    可选项有默认值   
            //    ResultType = ResultType.String,
            //};
            //HttpResult result = http.GetHtml(item);
            //Regex reg = new Regex(@"ptui_checkVC\('(.*)','(.*)','(.*)', '(.*)'\);");
            //Match m = reg.Match(result.Html);
            //string[] ret = new string[m.Groups.Count];
            //for (int i = 0; i < m.Groups.Count; i++)
            //{
            //    ret[i] = m.Groups[i].Value;
            //}
            //this.checkv2 = ret[3];
            //if (ret[1] == "0")
            //{
            //    this.checkcode = ret[2];
            //    this.session = ret[4];
            //    return false;
            //}
            //else
                return true;
        }
        /// <summary>
        /// 一次登录
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            string url = "https://ssl.ptlogin2.qq.com/login?u={$uin}&p={$pwd}&verifycode={$verify}&webqq_type=10&remember_uin=1&login2qq=0&aid=1003903&u1=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html%3Flogin2qq%3D0%26webqq_type%3D10&h=1&ptredirect=0&ptlang=2052&daid=164&from_ui=1&pttype=1&dumy=&fp=loginerroralert&action=2-9-33854&mibao_css=m_webqq&t=1&g=1&js_type=0&js_ver=10092&login_sig=K5F0E8woS74td4sRIqKiSHmH6B2RYYP467z2r*6YWaH4wc7vE*4G*X7V2kGP9s1*&pt_uistyle=5&pt_vcode_v1=0&pt_verifysession_v1={$session}";
            //url = url.Replace("{$uin}", this.qq);
            //url = url.Replace("{$verify}", this.checkcode);
            //string pwd = encrypt.Encrypt_Password(this.qq, this.password, this.checkcode);
            //url = url.Replace("{$pwd}", pwd);
            //url = url.Replace("{$session}", this.session);
            //HttpItem item = new HttpItem()
            //{
            //    URL = url,
            //    Encoding = System.Text.Encoding.GetEncoding("utf-8"),
            //    Method = "get",
            //    IsToLower = false,
            //    Timeout = 100000,
            //    Referer = "https://ui.ptlogin2.qq.com/cgi-bin/login?daid=164&target=self&style=5&mibao_css=m_webqq&appid=1003903&enable_qlogin=0&no_verifyimg=1&s_url=http%3A%2F%2Fweb2.qq.com%2Floginproxy.html&f_url=loginerroralert&strong_login=1&login_state=10&t=20140612002",
            //    Host = "d.web2.qq.com",
            //    UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0",//用户的浏览器类型，版本号。操作系统     可选项有默认值   
            //    ContentType = "application/x-www-form-urlencoded",
            //    ResultType = ResultType.String
            //};
            //HttpResult result = http.GetHtml(item);

            //foreach (Cookie c in result.CookieCollection)
            //{
            //    if (c.Name == "ptwebqq")
            //        this.ptwebqq = c.Value;
            //}
            //this.hash = Hash(this.qq, this.ptwebqq);

            //Regex reg = new Regex(@"ptuiCB\('(.*)','(.*)','(.*)','(.*)','(.*)',\s'(.*)'\);");
            //Match m = reg.Match(result.Html);
            //string[] ret = new string[m.Groups.Count];
            //for (int i = 0; i < m.Groups.Count; i++)
            //{
            //    ret[i] = m.Groups[i].Value;
            //}
            //if (ret[1] == "0")
            //{
            //    this.proxyurl = ret[3];
            //    return true;
            //}
            //else
                return false;

        }


    }
}
