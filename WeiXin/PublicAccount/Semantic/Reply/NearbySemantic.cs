using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 周边语义
    /// </summary>
    public class NearbySemantic : BaseSemantic
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }
        /// <summary>
        /// 限定词
        /// </summary>
        public string limit { get; private set; }
        /// <summary>
        /// 价格（单位：元）
        /// </summary>
        public NumberProtocol price { get; private set; }
        /// <summary>
        /// 距离（单位：米）
        /// </summary>
        public NumberProtocol radius { get; private set; }
        /// <summary>
        /// 是否是服务类，比如：找家政、租房、招聘等即为服务类；找 ATM、羽毛球馆等即为非服务类
        /// </summary>
        public bool? service { get; private set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public CouponEnum? coupon { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public SortEnum? sort { get; private set; }
        /// <summary>
        /// 是否扩展
        /// </summary>
        public bool? is_expand { get; private set; }

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
            keyword = joDetails.TryGetValue("keyword", out jt) ? (string)jt : null;
            limit = joDetails.TryGetValue("limit", out jt) ? (string)jt : null;
            price = joDetails.TryGetValue("price", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            radius = joDetails.TryGetValue("radius", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("service", out jt))
                service = (int)jt == 1;
            else
                service = null;
            if (joDetails.TryGetValue("coupon", out jt))
                coupon = (CouponEnum)(int)jt;
            else
                coupon = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (SortEnum)(int)jt;
            else
                sort = null;
            if (joDetails.TryGetValue("is_expand", out jt))
                is_expand = (int)jt == 1;
            else
                is_expand = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n地点：{1}\r\n关键词：{2}\r\n限定词：{3}\r\n" +
                "价格：{4}\r\n距离：{5}\r\n是否服务类：{6}\r\n优惠信息：{7}\r\n" +
                "排序类型：{8}\r\n是否扩展：{9}",
                base.ToString(),
                location != null ? location.ToString() : "",
                keyword ?? "",
                limit ?? "",
                price != null ? price.ToString() : "",
                radius != null ? radius.ToString() : "",
                service.HasValue ? service.Value.ToString() : "",
                coupon.HasValue ? coupon.Value.ToString("g") : "",
                sort.HasValue ? sort.Value.ToString("g") : "",
                is_expand.HasValue ? is_expand.Value.ToString() : "");
        }
    }
}