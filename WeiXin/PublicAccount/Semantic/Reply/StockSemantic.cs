using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 股票语义
    /// </summary>
    public class StockSemantic : BaseSemantic
    {
        /// <summary>
        /// 股票名称
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 标准股票代码
        /// </summary>
        public string code { get; private set; }
        /// <summary>
        /// 市场
        /// </summary>
        public StockCategoryEnum? category { get; private set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTimeSingleProtocol datetime { get; private set; }

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
            code = joDetails.TryGetValue("code", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("category", out jt))
                category = (StockCategoryEnum)Enum.Parse(typeof(StockCategoryEnum), (string)jt);
            else
                category = null;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n股票名称：{1}\r\n代码：{2}\r\n市场：{3}\r\n时间：{4}",
                base.ToString(),
                name ?? "",
                code ?? "",
                category.HasValue ? category.Value.ToString("g") : "",
                datetime != null ? datetime.ToString() : "");
        }
    }
}