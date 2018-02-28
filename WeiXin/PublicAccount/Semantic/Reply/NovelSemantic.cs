using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 小说语义
    /// </summary>
    public class NovelSemantic : BaseSemantic
    {
        /// <summary>
        /// 小说名
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; private set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 章节
        /// </summary>
        public NumberProtocol chapter { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public NovelSortEnum? sort { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            name = joDetails.TryGetValue("name", out jt) ? (string)jt : null;
            author = joDetails.TryGetValue("author", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
            chapter = joDetails.TryGetValue("chapter", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (NovelSortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n小说名：{1}\r\n作者：{2}\r\n类型：{3}\r\n" +
                "章节：{4}\r\n排序类型：{5}",
                base.ToString(), name ?? "", author ?? "", category ?? "",
                chapter != null ? chapter.ToString() : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}