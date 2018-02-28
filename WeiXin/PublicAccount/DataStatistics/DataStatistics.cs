using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 数据统计
    /// </summary>
    public static class DataStatistics
    {
        /// <summary>
        /// 获取统计数据的http方法
        /// </summary>
        private const string httpMethod = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取用户增减数据的地址
        /// </summary>
        private const string urlForGettingUserSummary = "https://api.weixin.qq.com/datacube/getusersummary?access_token={0}";
        /// <summary>
        /// 获取用户增减数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserSummary = 7;
        /// <summary>
        /// 获取累计用户数据的地址
        /// </summary>
        private const string urlForGettingUserCumulate = "https://api.weixin.qq.com/datacube/getusercumulate?access_token={0}";
        /// <summary>
        /// 获取累计用户数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserCumulate = 7;
        /// <summary>
        /// 获取图文群发每日数据的地址
        /// </summary>
        private const string urlForGettingArticleSummary = "https://api.weixin.qq.com/datacube/getarticlesummary?access_token={0}";
        /// <summary>
        /// 获取图文群发每日数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingArticleSummary = 1;
        /// <summary>
        /// 获取图文群发总数据的地址
        /// </summary>
        private const string urlForGettingArticleTotal = "https://api.weixin.qq.com/datacube/getarticletotal?access_token={0}";
        /// <summary>
        /// 获取图文群发总数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingArticleTotal = 1;
        /// <summary>
        /// 获取图文统计数据的地址
        /// </summary>
        private const string urlForGettingUserRead = "https://api.weixin.qq.com/datacube/getuserread?access_token={0}";
        /// <summary>
        /// 获取图文统计数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserRead = 3;
        /// <summary>
        /// 获取图文统计分时数据的地址
        /// </summary>
        private const string urlForGettingUserReadHour = "https://api.weixin.qq.com/datacube/getuserreadhour?access_token={0}";
        /// <summary>
        /// 获取图文统计分时数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserReadHour = 1;
        /// <summary>
        /// 获取图文分享转发数据的地址
        /// </summary>
        private const string urlForGettingUserShare = "https://api.weixin.qq.com/datacube/getusershare?access_token={0}";
        /// <summary>
        /// 获取图文分享转发数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserShare = 7;
        /// <summary>
        /// 获取图文分享转发分时数据的地址
        /// </summary>
        private const string urlForGettingUserShareHour = "https://api.weixin.qq.com/datacube/getusersharehour?access_token={0}";
        /// <summary>
        /// 获取图文分享转发分时数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUserShareHour = 1;
        /// <summary>
        /// 获取消息发送概况数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsg = "https://api.weixin.qq.com/datacube/getupstreammsg?access_token={0}";
        /// <summary>
        /// 获取消息发送概况数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsg = 7;
        /// <summary>
        /// 获取消息发送分时数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgHour = "https://api.weixin.qq.com/datacube/getupstreammsghour?access_token={0}";
        /// <summary>
        /// 获取消息发送分时数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgHour = 1;
        /// <summary>
        /// 获取消息发送周数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgWeek = "https://api.weixin.qq.com/datacube/getupstreammsgweek?access_token={0}";
        /// <summary>
        /// 获取消息发送周数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgWeek = 30;
        /// <summary>
        /// 获取消息发送月数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgMonth = "https://api.weixin.qq.com/datacube/getupstreammsgmonth?access_token={0}";
        /// <summary>
        /// 获取消息发送月数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgMonth = 30;
        /// <summary>
        /// 获取消息发送分布数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgDist = "https://api.weixin.qq.com/datacube/getupstreammsgdist?access_token={0}";
        /// <summary>
        /// 获取消息发送分布数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgDist = 15;
        /// <summary>
        /// 获取消息发送分布周数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgDistWeek = "https://api.weixin.qq.com/datacube/getupstreammsgdistweek?access_token={0}";
        /// <summary>
        /// 获取消息发送分布周数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgDistWeek = 30;
        /// <summary>
        /// 获取消息发送分布月数据的地址
        /// </summary>
        private const string urlForGettingUpstreamMsgDistMonth = "https://api.weixin.qq.com/datacube/getupstreammsgdistmonth?access_token={0}";
        /// <summary>
        /// 获取消息发送分布月数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingUpstreamMsgDistMonth = 30;
        /// <summary>
        /// 获取接口分析数据的地址
        /// </summary>
        private const string urlForGettingInterfaceSummary = "https://api.weixin.qq.com/datacube/getinterfacesummary?access_token={0}";
        /// <summary>
        /// 获取接口分析数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingInterfaceSummary = 30;
        /// <summary>
        /// 获取接口分析分时数据的地址
        /// </summary>
        private const string urlForGettingInterfaceSummaryHour = "https://api.weixin.qq.com/datacube/getinterfacesummaryhour?access_token={0}";
        /// <summary>
        /// 获取接口分析分时数据的最大时间跨度（天）
        /// </summary>
        private const int timeSpanForGettingInterfaceSummaryHour = 1;

        /// <summary>
        /// 检查时间跨度
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="timeSpan">最大时间跨度</param>
        /// <returns>返回时间跨度是否符合要求</returns>
        private static bool CheckTimeSpan(DateTime beginDate, DateTime endDate, int timeSpan)
        {
            beginDate = beginDate.Date;
            endDate = endDate.Date;
            if (beginDate > endDate)
                return false;
            return (endDate - beginDate).TotalDays <= timeSpan - 1;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="url">获取统计数据的地址</param>
        /// <param name="timeSpan">最大时间跨度</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回包含统计数据的JArray对象</returns>
        private static T[] Get<T>(string userName, DateTime beginDate, DateTime endDate, string url, int timeSpan, out ErrorMessage errorMessage)
            where T : IParsable, new()
        {
            errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "");
            if (!CheckTimeSpan(beginDate, endDate, timeSpan))
            {
                errorMessage.errmsg = "起止日期不正确或者超过范围。";
                return null;
            }
            string json = JsonConvert.SerializeObject(new { begin_date = beginDate.ToString("yyyy-MM-dd"), end_date = endDate.ToString("yyyy-MM-dd") });
            string responseData = HttpHelper.RequestResponseContent(url, userName, null, httpMethod, json);
            if (string.IsNullOrWhiteSpace(responseData))
            {
                errorMessage.errmsg = "请求失败。";
                return null;
            }
            else if (ErrorMessage.IsErrorMessage(responseData))
            {
                errorMessage = ErrorMessage.Parse(responseData);
                return null;
            }
            else
            {
                JObject jo = JObject.Parse(responseData);
                JToken jt;
                if (jo.TryGetValue("list", out jt) && jt.Type == JTokenType.Array)
                {
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "请求成功。");
                    JArray ja = (JArray)jt;
                    T[] result = new T[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                    {
                        T item = new T();
                        item.Parse((JObject)ja[i]);
                        result[i] = item;
                    }
                    return result;
                }
                else
                {
                    errorMessage.errmsg = "解析结果失败。";
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取用户增减数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回用户增减数据</returns>
        public static UserSummary[] GetUserSummary(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserSummary>(userName, beginDate, endDate, urlForGettingUserSummary, timeSpanForGettingUserSummary, out errorMessage);
        }

        /// <summary>
        /// 获取总用户量
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回总用户量</returns>
        public static UserCumulate[] GetUserCumulate(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserCumulate>(userName, beginDate, endDate, urlForGettingUserCumulate, timeSpanForGettingUserCumulate, out errorMessage);
        }

        /// <summary>
        /// 获取图文群发每日数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文群发每日数据</returns>
        public static ArticleSummary[] GetArticleSummary(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<ArticleSummary>(userName, beginDate, endDate, urlForGettingArticleSummary, timeSpanForGettingArticleSummary, out errorMessage);
        }

        /// <summary>
        /// 获取图文群发总数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文群发总数据</returns>
        public static ArticleTotal[] GetArticleTotal(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<ArticleTotal>(userName, beginDate, endDate, urlForGettingArticleTotal, timeSpanForGettingArticleTotal, out errorMessage);
        }

        /// <summary>
        /// 获取图文统计数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文统计数据</returns>
        public static UserRead[] GetUserRead(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserRead>(userName, beginDate, endDate, urlForGettingUserRead, timeSpanForGettingUserRead, out errorMessage);
        }

        /// <summary>
        /// 获取图文统计分时数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文统计分时数据</returns>
        public static UserReadHour[] GetUserReadHour(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserReadHour>(userName, beginDate, endDate, urlForGettingUserReadHour, timeSpanForGettingUserReadHour, out errorMessage);
        }

        /// <summary>
        /// 获取图文分享转发数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文分享转发数据</returns>
        public static UserShare[] GetUserShare(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserShare>(userName, beginDate, endDate, urlForGettingUserShare, timeSpanForGettingUserShare, out errorMessage);
        }

        /// <summary>
        /// 获取图文分享转发分时数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文分享转发分时数据</returns>
        public static UserShareHour[] GetUserShareHour(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UserShareHour>(userName, beginDate, endDate, urlForGettingUserShareHour, timeSpanForGettingUserShareHour, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送概况数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送概况数据</returns>
        public static UpstreamMsg[] GetUpstreamMsg(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsg>(userName, beginDate, endDate, urlForGettingUpstreamMsg, timeSpanForGettingUpstreamMsg, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送分时数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送分时数据</returns>
        public static UpstreamMsgHour[] GetUpstreamMsgHour(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsgHour>(userName, beginDate, endDate, urlForGettingUpstreamMsgHour, timeSpanForGettingUpstreamMsgHour, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送周数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送周数据</returns>
        public static UpstreamMsg[] GetUpstreamMsgWeek(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsg>(userName, beginDate, endDate, urlForGettingUpstreamMsgWeek, timeSpanForGettingUpstreamMsgWeek, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送月数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送月数据</returns>
        public static UpstreamMsg[] GetUpstreamMsgMonth(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsg>(userName, beginDate, endDate, urlForGettingUpstreamMsgMonth, timeSpanForGettingUpstreamMsgMonth, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送分布数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送分布数据</returns>
        public static UpstreamMsgDist[] GetUpstreamMsgDist(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsgDist>(userName, beginDate, endDate, urlForGettingUpstreamMsgDist, timeSpanForGettingUpstreamMsgDist, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送分布周数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送分布周数据</returns>
        public static UpstreamMsgDist[] GetUpstreamMsgDistWeek(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsgDist>(userName, beginDate, endDate, urlForGettingUpstreamMsgDistWeek, timeSpanForGettingUpstreamMsgDistWeek, out errorMessage);
        }

        /// <summary>
        /// 获取消息发送分布月数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回消息发送分布月数据</returns>
        public static UpstreamMsgDist[] GetUpstreamMsgDistMonth(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<UpstreamMsgDist>(userName, beginDate, endDate, urlForGettingUpstreamMsgDistMonth, timeSpanForGettingUpstreamMsgDistMonth, out errorMessage);
        }

        /// <summary>
        /// 获取接口分析数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回接口分析数据</returns>
        public static InterfaceSummary[] GetInterfaceSummary(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<InterfaceSummary>(userName, beginDate, endDate, urlForGettingInterfaceSummary, timeSpanForGettingInterfaceSummary, out errorMessage);
        }

        /// <summary>
        /// 获取接口分析分时数据
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回接口分析分时数据</returns>
        public static InterfaceSummaryHour[] GetInterfaceSummaryHour(string userName, DateTime beginDate, DateTime endDate, out ErrorMessage errorMessage)
        {
            return Get<InterfaceSummaryHour>(userName, beginDate, endDate, urlForGettingInterfaceSummaryHour, timeSpanForGettingInterfaceSummaryHour, out errorMessage);
        }
    }
}