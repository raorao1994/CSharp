using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 新闻语义
    /// </summary>
    public class NewsSemantic : BaseSemantic
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }
        /// <summary>
        /// 新闻类别
        /// </summary>
        public string category { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            keyword = joDetails.TryGetValue("keyword", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n关键词：{1}\r\n新闻类别：{2}",
                base.ToString(), keyword ?? "", category ?? "");
        }
    }
}