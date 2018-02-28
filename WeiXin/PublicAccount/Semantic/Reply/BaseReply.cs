using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 语义理解应答基类
    /// </summary>
    public class BaseReply : IParsable
    {
        /// <summary>
        /// 获取表示请求后的状态
        /// </summary>
        public int errcode { get; private set; }
        /// <summary>
        /// 获取请求内容，如果发生错误，该值为null
        /// </summary>
        public string query { get; private set; }
        /// <summary>
        /// 获取服务类型，如果发生错误，该值为null
        /// </summary>
        public ServiceTypeEnum? type { get; private set; }

        /// <summary>
        /// 获取请求是否成功
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return errcode == ErrorMessage.SuccessCode;
            }
        }

        /// <summary>
        /// 获取汉字错误提示
        /// </summary>
        public string ChineseMessage
        {
            get
            {
                return ErrorMessage.GetChineseMessage(errcode);
            }
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            JToken jt;
            if (jo.TryGetValue("errcode", out jt) || jo.TryGetValue("ret", out jt))
                errcode = (int)jt;
            else
                throw new ArgumentException("解析语义理解响应错误。", "errcode");
            query = jo.TryGetValue("query", out jt) ? (string)jt : null;
            if (jo.TryGetValue("type", out jt))
                type = (ServiceTypeEnum)Enum.Parse(typeof(ServiceTypeEnum), (string)jt);
            else
                type = null;
        }

        internal static BaseReply ParseObject(JObject jo)
        {
            BaseReply reply = new BaseReply();
            reply.Parse(jo);
            if (reply.IsSuccess && reply.type.HasValue)
            {
                switch (reply.type.Value)
                {
                    case ServiceTypeEnum.restaurant:
                        reply = Utility.Parse<RestaurantReply>(jo);
                        break;
                    case ServiceTypeEnum.map:
                        reply = Utility.Parse<MapReply>(jo);
                        break;
                    case ServiceTypeEnum.nearby:
                        reply = Utility.Parse<NearbyReply>(jo);
                        break;
                    case ServiceTypeEnum.coupon:
                        reply = Utility.Parse<CouponReply>(jo);
                        break;
                    case ServiceTypeEnum.hotel:
                        reply = Utility.Parse<HotelReply>(jo);
                        break;
                    case ServiceTypeEnum.travel:
                        reply = Utility.Parse<TravelReply>(jo);
                        break;
                    case ServiceTypeEnum.flight:
                        reply = Utility.Parse<FlightReply>(jo);
                        break;
                    case ServiceTypeEnum.train:
                        reply = Utility.Parse<TrainReply>(jo);
                        break;
                    case ServiceTypeEnum.movie:
                        reply = Utility.Parse<MovieReply>(jo);
                        break;
                    case ServiceTypeEnum.music:
                        reply = Utility.Parse<MusicReply>(jo);
                        break;
                    case ServiceTypeEnum.video:
                        reply = Utility.Parse<VideoReply>(jo);
                        break;
                    case ServiceTypeEnum.novel:
                        reply = Utility.Parse<NovelReply>(jo);
                        break;
                    case ServiceTypeEnum.weather:
                        reply = Utility.Parse<WeatherReply>(jo);
                        break;
                    case ServiceTypeEnum.stock:
                        reply = Utility.Parse<StockReply>(jo);
                        break;
                    case ServiceTypeEnum.remind:
                        reply = Utility.Parse<RemindReply>(jo);
                        break;
                    case ServiceTypeEnum.telephone:
                        reply = Utility.Parse<TelephoneReply>(jo);
                        break;
                    case ServiceTypeEnum.cookbook:
                        reply = Utility.Parse<CookbookReply>(jo);
                        break;
                    case ServiceTypeEnum.baike:
                        reply = Utility.Parse<BaikeReply>(jo);
                        break;
                    case ServiceTypeEnum.news:
                        reply = Utility.Parse<NewsReply>(jo);
                        break;
                    case ServiceTypeEnum.tv:
                        reply = Utility.Parse<TvReply>(jo);
                        break;
                    case ServiceTypeEnum.instruction:
                        reply = Utility.Parse<InstructionReply>(jo);
                        break;
                    case ServiceTypeEnum.tv_instruction:
                        reply = Utility.Parse<TvInstructionReply>(jo);
                        break;
                    case ServiceTypeEnum.car_instruction:
                        reply = Utility.Parse<CarInstructionReply>(jo);
                        break;
                    case ServiceTypeEnum.app:
                        reply = Utility.Parse<AppReply>(jo);
                        break;
                    case ServiceTypeEnum.website:
                        reply = Utility.Parse<WebsiteReply>(jo);
                        break;
                    case ServiceTypeEnum.search:
                        reply = Utility.Parse<SearchReply>(jo);
                        break;
                }
            }
            return reply;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("请求状态：{0}\r\n错误提示：{1}\r\n请求内容：{2}\r\n服务类型：{3}",
                errcode, ChineseMessage ?? "", query ?? "", type.HasValue ? type.Value.ToString("g") : "");
        }
    }
}