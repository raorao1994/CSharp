using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 响应图文消息
    /// </summary>
    public class ResponseNewsMessage : ResponseBaseMessage
    {
        /// <summary>
        /// 获取或设置图文列表
        /// </summary>
        public List<Article> Articles { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="articles">图文列表</param>
        public ResponseNewsMessage(string toUserName, string fromUserName, DateTime createTime,
            List<Article> articles)
            : base(toUserName, fromUserName, createTime, ResponseMessageTypeEnum.news)
        {
            if (articles == null)
                Articles = new List<Article>();
            else
                Articles = articles;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="toUserName">接收方账号</param>
        /// <param name="fromUserName">开发者微信号</param>
        /// <param name="createTime">消息创建时间</param>
        /// <param name="article">图文</param>
        public ResponseNewsMessage(string toUserName, string fromUserName, DateTime createTime,
            Article article)
            : this(toUserName, fromUserName, createTime, article != null ? new List<Article>() { article } : (List<Article>)null)
        {
        }

        /// <summary>
        /// 返回消息字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string newLine = System.Environment.NewLine;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("图文数目：{0}{1}", Articles != null ? Articles.Count : 0, newLine);
            if (Articles != null && Articles.Count > 0)
            {
                foreach (Article article in Articles)
                    sb.AppendLine(article.ToString());
            }
            sb.Remove(sb.Length - newLine.Length, newLine.Length);
            return sb.ToString();
        }

        /// <summary>
        /// 返回XML格式的响应消息
        /// </summary>
        /// <returns>返回响应消息</returns>
        protected override string ToXml()
        {
            XmlDocument doc = CreateXmlDocument();
            XmlElement root = doc.DocumentElement;
            root.AppendChild(CreateXmlElement(doc, "ArticleCount", Articles != null ? Articles.Count : 0));
            XmlElement articles = CreateXmlElement(doc, "Articles");
            if (Articles != null && Articles.Count > 0)
            {
                foreach (Article article in Articles)
                    articles.AppendChild(article.ToXmlElement(doc));
            }
            root.AppendChild(articles);
            return doc.InnerXml;
        }

        /// <summary>
        /// 返回JSON格式的客服消息
        /// </summary>
        /// <returns></returns>
        public override string ToJson()
        {
            List<object> articles = null;
            if (Articles != null && Articles.Count > 0)
            {
                articles = new List<object>(Articles.Count);
                foreach (Article article in Articles)
                    articles.Add(article.ToAnonymousObject());
            }
            var customerService = new
            {
                touser = ToUserName,
                msgtype = MsgType.ToString("g"),
                news = new
                {
                    articles = articles
                }
            };
            return JsonConvert.SerializeObject(customerService);
        }
    }

    /// <summary>
    /// 图文
    /// </summary>
    public class Article
    {
        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 获取或设置描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 获取或设置图片链接（尺寸建议：较好的效果为大图360*200，小图200*200）
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 获取或设置跳转链接
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <param name="picUrl">图片链接</param>
        /// <param name="url">跳转链接</param>
        public Article(string title=null, string description=null, string picUrl=null,string url=null)
        {
            Title = title;
            Description = description;
            PicUrl = picUrl;
            Url = url;
        }

        /// <summary>
        /// 返回图文字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("标题：{0}\r\n描述：{1}\r\n图片链接：{2}\r\n跳转链接：{3}",
                Title, Description, PicUrl, Url);
        }

        /// <summary>
        /// 返回XmlElement
        /// </summary>
        /// <param name="doc">xml文档</param>
        /// <returns></returns>
        public XmlElement ToXmlElement(XmlDocument doc)
        {
            XmlElement item = doc.CreateElement("item");
            XmlElement title = doc.CreateElement("Title");
            title.AppendChild(doc.CreateCDataSection(Title ?? ""));
            item.AppendChild(title);
            XmlElement description = doc.CreateElement("Description");
            description.AppendChild(doc.CreateCDataSection(Description ?? ""));
            item.AppendChild(description);
            XmlElement picurl = doc.CreateElement("PicUrl");
            picurl.AppendChild(doc.CreateCDataSection(PicUrl ?? ""));
            item.AppendChild(picurl);
            XmlElement url = doc.CreateElement("Url");
            url.AppendChild(doc.CreateCDataSection(Url ?? ""));
            item.AppendChild(url);
            return item;
        }

        /// <summary>
        /// 返回匿名对象，用于获取图文消息的JSON字符串
        /// </summary>
        /// <returns></returns>
        public object ToAnonymousObject()
        {
            return new
            {
                title = Title,
                description = Description,
                picurl = PicUrl,
                url = Url
            };
        }
    }
}