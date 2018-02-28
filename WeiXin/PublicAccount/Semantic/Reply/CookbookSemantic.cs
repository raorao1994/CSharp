using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 菜谱语义
    /// </summary>
    public class CookbookSemantic : BaseSemantic
    {
        /// <summary>
        /// 菜名
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 菜系
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 食材
        /// </summary>
        public string ingredient { get; private set; }

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
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
            ingredient = joDetails.TryGetValue("ingredient", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n菜名：{1}\r\n菜系：{2}\r\n食材：{3}",
                base.ToString(),
                name ?? "",
                category ?? "",
                ingredient ?? "");
        }
    }
}