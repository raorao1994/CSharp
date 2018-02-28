using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 新闻
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; private set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl { get; private set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title"></param>
        /// <param name="source"></param>
        /// <param name="detailUrl"></param>
        /// <param name="icon"></param>
        internal Article(string title, string source, string detailUrl, string icon)
        {
            this.Title = title;
            Source = source;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal Article(JObject jo)
            : this((string)jo["article"], (string)jo["source"], (string)jo["detailurl"], (string)jo["icon"])
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("标题：{0}\r\n来源：{1}\r\n详情地址：{2}\r\n图标地址：{3}",
                Title, Source, DetailUrl, Icon);
        }
    }
}
