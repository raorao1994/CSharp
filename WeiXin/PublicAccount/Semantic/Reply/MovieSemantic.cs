using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 电影语义
    /// </summary>
    public class MovieSemantic : BaseSemantic
    {
        /// <summary>
        /// 电影名
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 演员
        /// </summary>
        public string actor { get; private set; }
        /// <summary>
        /// 导演
        /// </summary>
        public string director { get; private set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string tag { get; private set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string country { get; private set; }
        /// <summary>
        /// 电影院
        /// </summary>
        public string cinema { get; private set; }
        /// <summary>
        /// 地点
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTimeSingleProtocol datetime { get; private set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public CouponEnum? coupon { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public MovieSortEnum? sort { get; private set; }

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
            actor = joDetails.TryGetValue("actor", out jt) ? (string)jt : null;
            director = joDetails.TryGetValue("director", out jt) ? (string)jt : null;
            tag = joDetails.TryGetValue("tag", out jt) ? (string)jt : null;
            country = joDetails.TryGetValue("country", out jt) ? (string)jt : null;
            cinema = joDetails.TryGetValue("cinema", out jt) ? (string)jt : null;
            location = joDetails.TryGetValue("location", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("coupon", out jt))
                coupon = (CouponEnum)(int)jt;
            else
                coupon = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (MovieSortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n电影名：{1}\r\n演员：{2}\r\n导演：{3}\r\n" +
                "类型：{4}\r\n地区：{5}\r\n电影院：{6}\r\n地点：{7}\r\n" +
                "时间：{8}\r\n优惠信息：{9}\r\n排序类型：{10}",
                base.ToString(), name ?? "", actor ?? "", director ?? "",
                tag ?? "", country ?? "", cinema ?? "",
                location != null ? location.ToString() : "",
                datetime != null ? datetime.ToString() : "",
                coupon.HasValue ? coupon.Value.ToString("g") : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}