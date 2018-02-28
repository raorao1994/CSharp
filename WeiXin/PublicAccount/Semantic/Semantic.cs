using System;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    public static class Semantic
    {
        /// <summary>
        /// 请求语义理解的地址
        /// </summary>
        private const string urlForQuerying = "https://api.weixin.qq.com/semantic/semproxy/search?access_token={0}";
        /// <summary>
        /// 请求语义理解的http方法
        /// </summary>
        private const string httpMethodForQuerying = WebRequestMethods.Http.Post;
        /// <summary>
        /// 默认的应用id
        /// </summary>
        private const string defaultAppid = "PublicAccount.Semantic";

        /// <summary>
        /// 获取语义理解的请求json数据
        /// </summary>
        /// <param name="query">输入文本（待理解的文本）</param>
        /// <param name="serviceType">服务类别</param>
        /// <param name="city">城市</param>
        /// <param name="region">区域</param>
        /// <param name="appid">应用id</param>
        /// <param name="uid">用户id</param>
        /// <returns>返回请求json数据</returns>
        private static string GetQueryJson(string query, ServiceTypeEnum serviceType, string city, 
            string region = null, string appid = defaultAppid, string uid = null)
        {
            dynamic data = new ExpandoObject();
            data.query = query;
            data.category = serviceType.ToString("g");
            data.city = city;
            if (!string.IsNullOrWhiteSpace(region))
                data.region = region;
            data.appid = string.IsNullOrWhiteSpace(appid) ? defaultAppid : appid;
            data.uid = string.IsNullOrWhiteSpace(uid) ? "" : uid;
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 获取语义理解的请求json数据
        /// </summary>
        /// <param name="query">输入文本（待理解的文本）</param>
        /// <param name="serviceType">服务类别</param>
        /// <param name="latitude">纬度坐标</param>
        /// <param name="longitude">经度坐标</param>
        /// <param name="appid">应用id</param>
        /// <param name="uid">用户id</param>
        /// <returns>返回请求json数据</returns>
        private static string GetQueryJson(string query, ServiceTypeEnum serviceType, float latitude, float longitude,
            string appid = defaultAppid, string uid = null)
        {
            dynamic data = new ExpandoObject();
            data.query = query;
            data.category = serviceType.ToString("g");
            data.latitude = latitude;
            data.longitude = longitude;
            data.appid = string.IsNullOrWhiteSpace(appid) ? defaultAppid : appid;
            data.uid = string.IsNullOrWhiteSpace(uid) ? "" : uid;
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 请求语义理解
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="query">输入文本（待理解的文本）</param>
        /// <param name="serviceType">服务类别</param>
        /// <param name="city">城市</param>
        /// <param name="region">区域</param>
        /// <param name="appid">应用id</param>
        /// <param name="uid">用户id</param>
        /// <returns>返回语义理解应答；如果请求失败，返回null。</returns>
        public static BaseReply Query(string userName, string query, ServiceTypeEnum serviceType, string city,
            string region = null, string appid = defaultAppid, string uid = null)
        {
            string json = GetQueryJson(query, serviceType, city, region, appid, uid);
            return Query(userName, json);
        }

        /// <summary>
        /// 请求语义理解
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="query">输入文本（待理解的文本）</param>
        /// <param name="serviceType">服务类别</param>
        /// <param name="latitude">纬度坐标</param>
        /// <param name="longitude">经度坐标</param>
        /// <param name="appid">应用id</param>
        /// <param name="uid">用户id</param>
        /// <returns>返回语义理解应答；如果请求失败，返回null。</returns>
        public static BaseReply Query(string userName, string query, ServiceTypeEnum serviceType, float latitude, float longitude,
            string appid = defaultAppid, string uid = null)
        {
            string json = GetQueryJson(query, serviceType, latitude, longitude, appid, uid);
            return Query(userName, json);
        }

        /// <summary>
        /// 请求语义理解
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="json">请求json数据</param>
        /// <returns>返回语义理解应答；如果请求失败，返回null。</returns>
        private static BaseReply Query(string userName,string json)
        {
            string responseContent = HttpHelper.RequestResponseContent(urlForQuerying, userName, null, httpMethodForQuerying, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                return null;
            else
                return BaseReply.ParseObject(JObject.Parse(responseContent));
        }
    }
}
