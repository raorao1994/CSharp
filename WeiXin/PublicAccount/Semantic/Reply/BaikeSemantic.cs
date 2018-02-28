using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 百科语义
    /// </summary>
    public class BaikeSemantic : BaseSemantic
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }

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
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n关键词：{1}", base.ToString(), keyword ?? "");
        }
    }
}