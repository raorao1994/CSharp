using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 应用语义
    /// </summary>
    public class AppSemantic : BaseSemantic
    {
        /// <summary>
        /// 应用名称
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 应用类别
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 应用排序方式
        /// </summary>
        public AppSortEnum? sort { get; private set; }
        /// <summary>
        /// 应用的查看方式
        /// </summary>
        public AppCheckTypeEnum? type { get; private set; }

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
            if (joDetails.TryGetValue("sort", out jt))
                sort = (AppSortEnum)(int)jt;
            else
                sort = null;
            if (joDetails.TryGetValue("type", out jt))
                type = (AppCheckTypeEnum)Enum.Parse(typeof(AppCheckTypeEnum), (string)jt);
            else
                type = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n应用名称：{1}\r\n应用类型：{2}\r\n排序方式：{3}\r\n查看类别：{4}\r\n",
                base.ToString(),
                name ?? "",
                category ?? "",
                sort.HasValue ? sort.Value.ToString("g") : "",
                type.HasValue ? type.Value.ToString("g") : "");
        }
    }
}