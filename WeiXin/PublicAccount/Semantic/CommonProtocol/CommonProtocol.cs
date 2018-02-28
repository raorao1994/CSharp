using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 通用协议
    /// </summary>
    public class CommonProtocol : IParsable
    {
        /// <summary>
        /// 通用协议类型
        /// </summary>
        public CommonProtocolTypeEnum type { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            type = (CommonProtocolTypeEnum)Enum.Parse(typeof(CommonProtocolTypeEnum), (string)jo["type"]);
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        /// <returns>返回通用协议对象</returns>
        internal static CommonProtocol ParseObject(JObject jo)
        {
            CommonProtocol cp = new CommonProtocol();
            cp.Parse(jo);
            switch(cp.type)
            {
                case CommonProtocolTypeEnum.DT_SINGLE:
                    cp = Utility.Parse<DateTimeSingleProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.DT_ORI:
                    cp = Utility.Parse<DateTimeOriProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.DT_INFER:
                    cp = Utility.Parse<DateTimeInferProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.DT_INTERVAL:
                    cp = Utility.Parse<DateTimeIntervalProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.DT_REPEAT:
                case CommonProtocolTypeEnum.DT_RORI:
                case CommonProtocolTypeEnum.DT_RINFER:
                    cp = Utility.Parse<DateTimeRepeatProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC:
                    cp = Utility.Parse<LocationProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC_COUNTRY:
                    cp = Utility.Parse<LocationCountryProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC_PROVINCE:
                    cp = Utility.Parse<LocationProvinceProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC_CITY:
                    cp = Utility.Parse<LocationCityProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC_TOWN:
                    cp = Utility.Parse<LocationTownProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.LOC_POI:
                    cp = Utility.Parse<LocationPoiProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.NORMAL_POI:
                    cp = Utility.Parse<LocationNormalPoiProtocol>(jo);
                    break;
                case CommonProtocolTypeEnum.NUMBER:
                case CommonProtocolTypeEnum.NUM_PRICE:
                case CommonProtocolTypeEnum.NUM_PADIUS:
                case CommonProtocolTypeEnum.NUM_DISCOUNT:
                case CommonProtocolTypeEnum.NUM_SEASON:
                case CommonProtocolTypeEnum.NUM_EPI:
                case CommonProtocolTypeEnum.NUM_CHAPTER:
                    cp = Utility.Parse<NumberProtocol>(jo);
                    break;
            }
            return cp;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("通用协议类型：{0:g}", type);
        }
    }
}
