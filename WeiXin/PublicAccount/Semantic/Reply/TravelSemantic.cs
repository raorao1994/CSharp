using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 旅游语义
    /// </summary>
    public class TravelSemantic : BaseSemantic
    {
        /// <summary>
        /// 旅游目的地
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 景点名称
        /// </summary>
        public string spot { get; private set; }
        /// <summary>
        /// 旅游日期
        /// </summary>
        public DateTimeSingleProtocol datetime { get; private set; }
        /// <summary>
        /// 旅游类型词
        /// </summary>
        public string tag { get; private set; }
        /// <summary>
        /// 旅游类型
        /// </summary>
        public TravelCategoryEnum? category { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            location = joDetails.TryGetValue("location", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            spot = joDetails.TryGetValue("spot", out jt) ? (string)jt : null;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            tag = joDetails.TryGetValue("tag", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("category", out jt))
                category = (TravelCategoryEnum)(int)jt;
            else
                category = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n旅游目的地：{1}\r\n景点名称：{2}\r\n旅游日期：{3}\r\n" +
                "旅游类型词：{4}\r\n旅游类型：{5}",
                base.ToString(),
                location != null ? location.ToString() : "",
                spot ?? "",
                datetime != null ? datetime.ToString() : "",
                tag ?? "",
                category.HasValue ? category.Value.ToString("g") : "");
        }
    }
}