using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 餐馆语义
    /// </summary>
    public class RestaurantSemantic : BaseSemantic
    {
        /// <summary>
        /// 获取地点
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 获取餐馆名称
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 获取餐馆类型或菜系
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 获取菜名
        /// </summary>
        public string special { get; private set; }
        /// <summary>
        /// 获取价格（单位：元）
        /// </summary>
        public NumberProtocol price { get; private set; }
        /// <summary>
        /// 获取距离（单位：米）
        /// </summary>
        public NumberProtocol radius { get; private set; }
        /// <summary>
        /// 获取优惠信息
        /// </summary>
        public CouponEnum? coupon { get; private set; }
        /// <summary>
        /// 获取排序类型
        /// </summary>
        public SortEnum? sort { get; private set; }
        /// <summary>
        /// 用户所处城市
        /// </summary>
        public string user_city { get; private set; }
        /// <summary>
        /// 用户所处纬度
        /// </summary>
        public float? user_latitude { get; private set; }
        /// <summary>
        /// 用户所处经度
        /// </summary>
        public float? user_longitude { get; private set; }

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
            name = joDetails.TryGetValue("name", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
            special = joDetails.TryGetValue("special", out jt) ? (string)jt : null;
            price = joDetails.TryGetValue("price", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            radius = joDetails.TryGetValue("radius", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("coupon", out jt))
                coupon = (CouponEnum)(int)jt;
            else
                coupon = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (SortEnum)(int)jt;
            else
                sort = null;
            user_city = joDetails.TryGetValue("user_city", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("user_latitude", out jt))
                user_latitude = float.Parse((string)jt);
            else
                user_latitude = null;
            if (joDetails.TryGetValue("user_longitude", out jt))
                user_longitude = float.Parse((string)jt);
            else
                user_longitude = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n地点：{1}\r\n餐馆名称：{2}\r\n餐馆类型/菜系：{3}\r\n" +
                "菜名：{4}\r\n价格：{5}\r\n距离：{6}\r\n优惠信息：{7}\r\n" +
                "排序类型：{8}\r\n用户所处城市：{9}\r\n用户所处纬度：{10}\r\n用户所处经度：{11}\r\n",
                base.ToString(), location != null ? location.ToString() : "", name ?? "", category ?? "",
                special ?? "", price != null ? price.ToString() : "", radius != null ? radius.ToString() : "",
                coupon.HasValue ? coupon.Value.ToString("g") : "", sort.HasValue ? sort.Value.ToString("g") : "",
                user_city ?? "", user_latitude.HasValue ? user_latitude.Value.ToString() : "",
                user_longitude.HasValue ? user_longitude.Value.ToString() : "");
        }
    }
}