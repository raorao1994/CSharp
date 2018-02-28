using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 天气语义
    /// </summary>
    public class WeatherSemantic : BaseSemantic
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationProtocol location { get; private set; }
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
            location = joDetails.TryGetValue("location", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n地点：{1}\r\n时间：{2}",
                base.ToString(),
                location != null ? location.ToString() : "",
                datetime != null ? datetime.ToString() : "");
        }
    }
}