using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// MultiMediaArticle：多媒体图文消息
    /// </summary>
    public class MultiMediaArticle
    {
        private string thumbMediaId;
        /// <summary>
        /// 获取或设置缩略图媒体ID
        /// </summary>
        public string ThumbMediaId
        {
            get
            {
                return thumbMediaId;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("ThumbMediaId", "ThumbMediaId为空。");
                thumbMediaId = value;
            }
        }

        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public string Author { get; set; }

        private string title;
        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Title", "Title为空。");
                title = value;
            }
        }

        /// <summary>
        /// 获取或设置阅读原文地址
        /// </summary>
        public string ContentSourceUrl { get; set; }

        private string content;
        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Content", "Content为空。");
                content = value;
            }
        }

        /// <summary>
        /// 获取或设置摘要
        /// </summary>
        public string Digest { get; set; }

        /// <summary>
        /// 获取或设置是否显示封面
        /// </summary>
        public bool ShowCoverPic { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="thumbMediaId">缩略图媒体ID</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="author">作者</param>
        /// <param name="contentSourceUrl">阅读原文地址</param>
        /// <param name="digest">摘要</param>
        /// <param name="showCoverPic">是否显示封面</param>
        public MultiMediaArticle(string thumbMediaId, string title, string content,
            string author = null, string contentSourceUrl = null, string digest = null,
            bool showCoverPic = false)
        {
            ThumbMediaId = thumbMediaId;
            Title = title;
            Content = content;
            Author = author;
            ContentSourceUrl = contentSourceUrl;
            Digest = digest;
            ShowCoverPic = showCoverPic;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">JObject对象</param>
        public MultiMediaArticle(JObject jo)
        {
            ThumbMediaId = (string)jo["thumb_media_id"];
            Title = (string)jo["title"];
            Content = (string)jo["content"];
            Author = (string)jo["author"];
            ContentSourceUrl = (string)jo["content_source_url"];
            Digest = (string)jo["digest"];
            ShowCoverPic = (int)jo["show_cover_pic"] == 1;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("媒体缩略图ID：{0}\r\n标题：{1}\r\n内容：{2}\r\n作者：{3}\r\n" +
                "阅读原文地址：{4}\r\n摘要：{5}\r\n是否显示封面：{6}",
                ThumbMediaId, Title, Content, Author,
                ContentSourceUrl, Digest, ShowCoverPic);
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public object ToAnonymousObject()
        {
            var obj = new
            {
                thumb_media_id = ThumbMediaId,
                author = Author,
                title = Title,
                content_source_url = ContentSourceUrl,
                content = Content,
                digest = Digest,
                show_cover_pic = ShowCoverPic ? "1" : "0"
            };
            return obj;
        }

        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <param name="articles">多媒体图文消息</param>
        /// <returns></returns>
        public static string ToJson(IEnumerable<MultiMediaArticle> articles)
        {
            List<object> objs;
            if (articles == null)
                objs = null;
            else
            {
                objs = new List<object>();
                foreach (MultiMediaArticle article in articles)
                    objs.Add(article.ToAnonymousObject());
            }
            return JsonConvert.SerializeObject(new { articles = objs });
        }
    }
}