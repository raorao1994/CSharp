using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tools
{
    public static class HttpHelper
    {
        private static CookieContainer cookie = new CookieContainer();
        private static string contentType = "application/x-www-form-urlencoded";
        private static string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        private static string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        /// <summary>
        /// 下载Http请求内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns>错误返回"" </returns>
        public static string GetHttpData(string url)
        {
            string result = String.Empty;
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                Byte[] pageData = wc.DownloadData(url);
                result = System.Text.Encoding.Default.GetString(pageData);
                //s = System.Text.Encoding.UTF8.GetString(pageData);去除中文乱码 
                return result;
            }
            catch (Exception e)
            {
                return result;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">网页地址</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string GetHtml(string url, Encoding encoding)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgent;
                request.ContentType = contentType;
                request.CookieContainer = cookie;
                request.Accept = accept;
                request.Method = "get";

                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, encoding);
                String html = reader.ReadToEnd();
                response.Close();

                return html;
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public static string GetURLData(string url)
        {
            int hitCount = 0;
            WebBrowser browser = new WebBrowser();

            browser.ScriptErrorsSuppressed = true;

            browser.Navigating += (sender, e) =>
            {
                hitCount++;
            };

            browser.DocumentCompleted += (sender, e) =>
            {
                hitCount++;
            };

            browser.Navigate(url);

            while (browser.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }

            while (hitCount < 16)
                Application.DoEvents();

            string html=browser.DocumentText;

            return html;
            var htmldocument = (mshtml.HTMLDocument)browser.Document.DomDocument;

            string gethtml = htmldocument.documentElement.outerHTML;

            //写入文件
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "//1.html"))
            {
                sw.WriteLine(gethtml);
            }
            Console.Write(gethtml);

            Console.WriteLine("html 文件 已经生成！");

            Console.Read();
        }
    }
}
