using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 网址语义
    /// </summary>
    public class WebsiteSemantic : BaseSemantic
    {
        /// <summary>
        /// 网址名
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; private set; }

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
            url = joDetails.TryGetValue("url", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n网址名：{1}\r\n地址：{2}",
                base.ToString(), name ?? "", url ?? "");
        }
    }
}