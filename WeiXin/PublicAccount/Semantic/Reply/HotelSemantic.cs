using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 
    /// </summary>
    public class HotelSemantic : BaseSemantic
    {
        /// <summary>
        /// 地点
        /// </summary>
        public LocationProtocol location { get; private set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTimeSingleProtocol start_date { get; private set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public DateTimeSingleProtocol end_date { get; private set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 价格（单位：元）
        /// </summary>
        public NumberProtocol price { get; private set; }
        /// <summary>
        /// 距离（单位：米）
        /// </summary>
        public NumberProtocol radius { get; private set; }
        /// <summary>
        /// 酒店等级
        /// </summary>
        public HotelLevelEnum? level { get; private set; }
        /// <summary>
        /// 是否有wifi
        /// </summary>
        public bool? wifi { get; private set; }
        /// <summary>
        /// 房间类型
        /// </summary>
        public RoomTypeEnum? roomtype { get; private set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public CouponEnum? coupon { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public SortEnum? sort { get; private set; }

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
            start_date = joDetails.TryGetValue("start_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_date = joDetails.TryGetValue("end_date", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            name = joDetails.TryGetValue("name", out jt) ? (string)jt : null;
            price = joDetails.TryGetValue("price", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            radius = joDetails.TryGetValue("radius", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("level", out jt))
                level = (HotelLevelEnum)Enum.Parse(typeof(HotelLevelEnum), (string)jt);
            else
                level = null;
            if (joDetails.TryGetValue("wifi", out jt))
                wifi = (int)jt == 1;
            else
                wifi = null;
            if (joDetails.TryGetValue("roomtype", out jt))
                roomtype = (RoomTypeEnum)Enum.Parse(typeof(RoomTypeEnum), (string)jt);
            else
                roomtype = null;
            if (joDetails.TryGetValue("coupon", out jt))
                coupon = (CouponEnum)(int)jt;
            else
                coupon = null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (SortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n地点：{1}\r\n入住时间：{2}\r\n离开时间：{3}\r\n" +
                "酒店名称：{4}\r\n价格：{5}\r\n距离：{6}\r\n酒店等级：{7}\r\n" +
                "是否有wifi：{8}\r\n房间类型：{9}\r\n优惠信息：{10}\r\n排序类型：{11}",
                base.ToString(),
                location != null ? location.ToString() : "",
                start_date != null ? start_date.ToString() : "",
                end_date != null ? end_date.ToString() : "",
                name ?? "",
                price != null ? price.ToString() : "",
                radius != null ? radius.ToString() : "",
                level.HasValue ? level.Value.ToString("g") : "",
                wifi.HasValue ? wifi.Value.ToString() : "",
                roomtype.HasValue ? roomtype.Value.ToString() : "",
                coupon.HasValue ? coupon.Value.ToString("g") : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}