using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 新闻结果
    /// </summary>
    public class ArticleResult:BaseResult
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<Article> Articles { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="articles"></param>
        internal ArticleResult(string text, IEnumerable<Article> articles)
            : base(CodeEnum.News, text)
        {
            if (articles == null)
                throw new ArgumentNullException("articles", "新闻列表不能为空。");
            Articles = new List<Article>(articles);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("新闻数：{0}", Articles.Count);
            if(Articles.Count>0)
            {
                foreach (Article article in Articles)
                    sb.AppendFormat("\r\n{0}", article);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析新闻结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static ArticleResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            Article[] articles = new Article[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                articles[idx] = new Article(item);
                idx++;
            }
            return new ArticleResult(text, articles);
        }
    }
}
