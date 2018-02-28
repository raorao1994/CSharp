using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 航班语义
    /// </summary>
    public class FlightSemantic : BaseSemantic
    {
        /// <summary>
        /// 航班号
        /// </summary>
        public string flight_no { get; private set; }
        /// <summary>
        /// 出发地
        /// </summary>
        public LocationProtocol start_loc { get; private set; }
        /// <summary>
        /// 目的地
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
        /// 航空公司
        /// </summary>
        public string airline { get; private set; }
        /// <summary>
        /// 座位级别
        /// </summary>
        public FlightSeatEnum? seat { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public FlightSortEnum? sort { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            flight_no = joDetails.TryGetValue("flight_no", out jt) ? (string)jt : null;
            start_loc = joDetails.TryGetValue("start_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_loc = joDetails.TryGetValue("end_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            start_date = joDetails.TryGetValue("start_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_date = joDetails.TryGetValue("end_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            airline = joDetails.TryGetValue("airline", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("seat", out jt))
                seat = (FlightSeatEnum)Enum.Parse(typeof(FlightSeatEnum), (string)jt);
            else
                seat = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (FlightSortEnum)Enum.Parse(typeof(FlightSortEnum), (string)jt);
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n航班号：{1}\r\n出发地点：{2}\r\n目的地点：{3}\r\n" +
                "出发日期：{4}\r\n返回日期：{5}\r\n航空公司：{6}\r\n座位级别：{7}\r\n排序类型：{8}",
                base.ToString(),
                start_loc != null ? start_loc.ToString() : "",
                end_loc != null ? end_loc.ToString() : "",
                start_date != null ? start_date.ToString() : "",
                end_date != null ? end_date.ToString() : "",
                airline ?? "",
                seat.HasValue ? seat.Value.ToString("g") : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}