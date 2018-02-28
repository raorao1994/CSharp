using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 地图语义
    /// </summary>
    public class MapSemantic : BaseSemantic
    {
        /// <summary>
        /// 起点区域，地点区域是地点位置的修饰描述，比如：“我现在在皇寺广场远东大厦门口”，起点区域是：“皇寺广场”，起点位置是：“远东大厦门口”
        /// </summary>
        public LocationProtocol start_area { get; private set; }
        /// <summary>
        /// 地点位置
        /// </summary>
        public LocationProtocol start_loc { get; private set; }
        /// <summary>
        /// 终点区域，地点区域是地点位置的修饰描述，比如：“我现在在皇寺广场远东大厦门口”，起点区域是：“皇寺广场”，起点位置是：“远东大厦门口”
        /// </summary>
        public LocationProtocol end_area { get; private set; }
        /// <summary>
        /// 终点位置
        /// </summary>
        public LocationProtocol end_loc { get; private set; }
        /// <summary>
        /// 出行方式
        /// </summary>
        public RouteTypeEnum? route_type { get; private set; }
        /// <summary>
        /// 公交车号
        /// </summary>
        public int? bus_num { get; private set; }
        /// <summary>
        /// 地铁线
        /// </summary>
        public string subway_num { get; private set; }
        /// <summary>
        /// 出行排序方式
        /// </summary>
        public RouteSortTypeEnum? type { get; private set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string keyword { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            start_area = joDetails.TryGetValue("start_area", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            start_loc = joDetails.TryGetValue("start_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_area = joDetails.TryGetValue("end_area", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            end_loc = joDetails.TryGetValue("end_loc", out jt) ? (LocationProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("route_type", out jt))
                route_type = (RouteTypeEnum)Enum.Parse(typeof(RouteTypeEnum), (string)jt);
            else
                route_type = null;
            if (joDetails.TryGetValue("bus_num", out jt))
                bus_num = (int)jt;
            else
                bus_num = null;
            subway_num = joDetails.TryGetValue("subway_num", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("type", out jt))
                type = (RouteSortTypeEnum)(int)jt;
            else
                type = null;
            keyword = joDetails.TryGetValue("keyword", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n起点区域：{1}\r\n起点位置：{2}\r\n终点区域：{3}\r\n" +
                "终点位置：{4}\r\n出行方式：{5}\r\n公交车号：{6}\r\n地铁线：{7}\r\n" +
                "出行排序类型：{8}\r\n关键词：{9}",
                base.ToString(),
                start_area != null ? start_area.ToString() : "",
                start_loc != null ? start_loc.ToString() : "",
                end_area != null ? end_area.ToString() : "",
                end_loc != null ? end_loc.ToString() : "",
                route_type.HasValue ? route_type.Value.ToString("g") : "",
                bus_num.HasValue ? bus_num.Value.ToString() : "",
                subway_num ?? "",
                type.HasValue ? type.Value.ToString() : "",
                keyword ?? "");
        }
    }
}