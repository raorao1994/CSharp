using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 火车语义
    /// </summary>
    public class TrainSemantic : BaseSemantic
    {
        /// <summary>
        /// 车次代码
        /// </summary>
        public string code { get; private set; }
        /// <summary>
        /// 起点
        /// </summary>
        public LocationProtocol start_loc { get; private set; }
        /// <summary>
        /// 终点
        /// </summary>
        public LocationProtocol end_loc { get; private set; }
        /// <summary>
        /// 出发日期
        /// </summary>
        public DateTimeSingleProtocol start_date { get; private set; }
        /// <summary>
        /// 返回日期
        /// </summary>
        public DateTimeSingleProtocol end_date { get; private set; }
        /// <summary>
        /// 座位级别
        /// </summary>
        public TrainSeatEnum? seat { get; private set; }
        /// <summary>
        /// 车次类型
        /// </summary>
        public TrainCategoryEnum? category { get; private set; }
        /// <summary>
        /// 单程或往返
        /// </summary>
        public TrainTypeEnum? type { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            code = joDetails.TryGetValue("code", out jt) ? (string)jt : null;
            start_loc = joDetails.TryGetValue("start_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_loc = joDetails.TryGetValue("end_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            start_date = joDetails.TryGetValue("start_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_date = joDetails.TryGetValue("end_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("seat", out jt))
                seat = (TrainSeatEnum)Enum.Parse(typeof(TrainSeatEnum), (string)jt);
            else
                seat = null;
            if (joDetails.TryGetValue("category", out jt))
                category = (TrainCategoryEnum)Enum.Parse(typeof(TrainCategoryEnum), (string)jt);
            else
                category = null;
            if (joDetails.TryGetValue("type", out jt))
                type = (TrainTypeEnum)Enum.Parse(typeof(TrainTypeEnum), (string)jt);
            else
                type = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n车次代码：{1}\r\n起点：{2}\r\n终点：{3}\r\n" +
                "出发日期：{4}\r\n返回日期：{5}\r\n座位级别：{6}\r\n车次类型：{7}\r\n单程或往返：",
                base.ToString(),
                code ?? "",
                start_loc != null ? start_loc.ToString() : "",
                end_loc != null ? end_loc.ToString() : "",
                start_date != null ? start_date.ToString() : "",
                end_date != null ? end_date.ToString() : "",
                seat.HasValue ? seat.Value.ToString("g") : "",
                category.HasValue ? category.Value.ToString("g") : "",
                type.HasValue ? type.Value.ToString("g") : "");
        }
    }
}