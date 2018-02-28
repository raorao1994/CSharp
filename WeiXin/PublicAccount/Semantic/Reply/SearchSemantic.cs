using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 网页搜索语义
    /// </summary>
    public class SearchSemantic : BaseSemantic
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }
        /// <summary>
        /// 搜索引擎
        /// </summary>
        public string channel { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            keyword = joDetails.TryGetValue("keyword", out jt) ? (string)jt : null;
            channel = joDetails.TryGetValue("channel", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n关键词：{1}\r\n搜索引擎：{2}",
                base.ToString(), keyword ?? "", channel ?? "");
        }
    }
}