using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 用于公众号后台设置菜单的图文消息
    /// </summary>
    public class NewsForMenu
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; private set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; private set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string digest { get; private set; }
        /// <summary>
        /// 是否显示封面：0-不显示，1-显示
        /// </summary>
        public bool show_cover { get; private set; }
        /// <summary>
        /// 封面图片的url
        /// </summary>
        public string cover_url { get; private set; }
        /// <summary>
        /// 正文的url
        /// </summary>
        public string content_url { get; private set; }
        /// <summary>
        /// 原文的url
        /// </summary>
        public string source_url { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="author">作者</param>
        /// <param name="digest">摘要</param>
        /// <param name="show_cover">是否显示封面</param>
        /// <param name="cover_url">封面图片的url</param>
        /// <param name="content_url">正文的url</param>
        /// <param name="source_url">原文的url</param>
        internal NewsForMenu(string title, string author, string digest, bool show_cover,
            string cover_url, string content_url, string source_url)
        {
            this.title = title;
            this.author = author;
            this.digest = digest;
            this.show_cover = show_cover;
            this.cover_url = cover_url;
            this.content_url = content_url;
            this.source_url = source_url;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("标题：{0}\r\n作者：{1}\r\n摘要：{2}\r\n是否显示封面：{3}\r\n" +
                "封面图片的url：{4}\r\n正文的url：{5}\r\n原文的url：{6}",
                title, author, digest, show_cover, cover_url, content_url, source_url);
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static NewsForMenu Parse(JObject jo)
        {
            return new NewsForMenu((string)jo["title"], (string)jo["author"], (string)jo["digest"], 
                (int)jo["show_cover"] == 1, (string)jo["cover_url"], (string)jo["content_url"], (string)jo["source_url"]);
        }
    }
}