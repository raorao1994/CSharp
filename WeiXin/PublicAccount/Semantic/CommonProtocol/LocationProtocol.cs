using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 地点协议
    /// </summary>
    public class LocationProtocol : CommonProtocol
    {
        /// <summary>
        /// 原始地名
        /// </summary>
        public string loc_ori { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            loc_ori = (string)jo["loc_ori"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n原始地名：{1}",
                base.ToString(), loc_ori);
        }
    }

    /// <summary>
    /// 国家协议
    /// </summary>
    public class LocationCountryProtocol : LocationProtocol
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; private set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; private set; }
        /// <summary>
        /// 城市简称
        /// </summary>
        public string city_simple { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            country = (string)jo["country"];
            city = (string)jo["city"];
            JToken jt;
            city_simple = jo.TryGetValue("city_simple", out jt) ? (string)jt : string.Empty;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n国家：{1}\r\n城市：{2}\r\n城市简称：{3}",
                base.ToString(), country, city, city_simple);
        }
    }

    /// <summary>
    /// 省份协议
    /// </summary>
    public class LocationProvinceProtocol : LocationProtocol
    {
        /// <summary>
        /// 省全称，例如：广东省
        /// </summary>
        public string province { get; private set; }
        /// <summary>
        /// 省简称，例如：广东|粤
        /// </summary>
        public string province_simple { get; private set; }
        /// <summary>
        /// 市全称，例如：北京市
        /// </summary>
        public string city { get; private set; }
        /// <summary>
        /// 市简称，例如：北京
        /// </summary>
        public string city_simple { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            province = (string)jo["province"];
            province_simple = (string)jo["province_simple"];
            city = (string)jo["city"];
            city_simple = (string)jo["city_simple"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n省：{1}\r\n省简称：{2}\r\n市：{3}\r\n市简称：{4}",
                base.ToString(), province, province_simple, city, city_simple);
        }
    }

    /// <summary>
    /// 城市协议
    /// </summary>
    public class LocationCityProtocol : LocationProtocol
    {
        /// <summary>
        /// 省全称，例如：广东省
        /// </summary>
        public string province { get; private set; }
        /// <summary>
        /// 省简称，例如：广东|粤
        /// </summary>
        public string province_simple { get; private set; }
        /// <summary>
        /// 市全称，例如：北京市
        /// </summary>
        public string city { get; private set; }
        /// <summary>
        /// 市简称，例如：北京
        /// </summary>
        public string city_simple { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JToken jt;
            province = jo.TryGetValue("province", out jt) ? (string)jt : string.Empty;
            province_simple = jo.TryGetValue("province_simple", out jt) ? (string)jt : string.Empty;
            city = (string)jo["city"];
            city_simple = (string)jo["city_simple"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n省：{1}\r\n省简称：{2}\r\n市：{3}\r\n市简称：{4}",
                base.ToString(), province, province_simple, city, city_simple);
        }
    }

    /// <summary>
    /// 县区协议
    /// </summary>
    public class LocationTownProtocol : LocationCityProtocol
    {
        /// <summary>
        /// 县区全称，例如：海淀区
        /// </summary>
        public string town { get; private set; }
        /// <summary>
        /// 县区简称，例如：海淀
        /// </summary>
        public string town_simple { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            town = (string)jo["town"];
            town_simple = (string)jo["town_simple"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n县区：{1}\r\n县区简称：{2}",
                base.ToString(), town, town_simple);
        }
    }

    /// <summary>
    /// 详细地址协议
    /// </summary>
    public class LocationPoiProtocol : LocationTownProtocol
    {
        /// <summary>
        /// 详细地址
        /// </summary>
        public string poi { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            poi = (string)jo["poi"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n详细地址：{1}", base.ToString(), poi);
        }
    }

    /// <summary>
    /// 地图上偏向机构的详细地址协议
    /// </summary>
    public class LocationNormalPoiProtocol : LocationProtocol
    {
        /// <summary>
        /// 地图上偏向机构的 poi 点，比如：饭馆、酒店、大厦等等
        /// </summary>
        public string poi { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            poi = (string)jo["poi"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n详细地址：{1}", base.ToString(), poi);
        }
    }
}
