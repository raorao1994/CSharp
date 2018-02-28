using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 团购语义
    /// </summary>
    public class CouponSemantic : BaseSemantic
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 价格（单位：元）
        /// </summary>
        public NumberProtocol price { get; private set; }
        /// <summary>
        /// 距离（单位：米）
        /// </summary>
        public NumberProtocol radius { get; private set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public CouponEnum? coupon { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public CouponSortEnum? sort { get; private set; }

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
            price = joDetails.TryGetValue("price", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            radius = joDetails.TryGetValue("radius", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            keyword = joDetails.TryGetValue("keyword", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("coupon", out jt))
                coupon = (CouponEnum)(int)jt;
            else
                coupon = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (CouponSortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n地点：{1}\r\n价格：{2}\r\n距离：{3}\r\n" +
                "关键词：{4}\r\n优惠信息：{5}\r\n排序类型：{6}",
                base.ToString(),
                location != null ? location.ToString() : "",
                price != null ? price.ToString() : "",
                radius != null ? radius.ToString() : "",
                keyword ?? "",
                coupon.HasValue ? coupon.Value.ToString("g") : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}