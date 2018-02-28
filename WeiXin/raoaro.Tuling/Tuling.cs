using System;
using System.Net;
using System.Web;
using System.Text;
using System.IO;

namespace Tuling
{
    public static class Tuling
    {
        /// <summary>
        /// 图灵服务器的请求地址
        /// </summary>
        private const string URL = "http://www.tuling123.com/openapi/api?key={0}&info={1}&userid={2}&loc={3}&lon={4}&lat={5}";
        /// <summary>
        /// 请求的http方法
        /// </summary>
        private const string HTTP_METHOD = WebRequestMethods.Http.Get;
        /// <summary>
        /// 请求的字符编码
        /// </summary>
        private static readonly Encoding ENCODING = Encoding.UTF8;

        /// <summary>
        /// 向图灵服务器发送请求，并返回结果
        /// </summary>
        /// <param name="key">开发者的app key</param>
        /// <param name="info">请求内容</param>
        /// <param name="userId">上下文相关用户id</param>
        /// <param name="location">地理位置</param>
        /// <returns>返回结果；如果获取失败，返回null。</returns>
        public static BaseResult Request(string key, string info, string userId = null, string location = null)
        {
            if (!CheckParameters(key, info, userId, location))
                return null;
            string url = string.Format(URL, key, HttpUtility.UrlEncode(info, ENCODING),
                string.IsNullOrWhiteSpace(userId) ? string.Empty : HttpUtility.UrlEncode(userId, ENCODING),
                string.IsNullOrWhiteSpace(location) ? string.Empty : HttpUtility.UrlEncode(location, ENCODING),
                string.Empty, string.Empty);
            return Request(url);
        }

        /// <summary>
        /// 向图灵服务器发送请求，并返回结果
        /// </summary>
        /// <param name="key">开发者的app key</param>
        /// <param name="info">请求内容</param>
        /// <param name="userId">上下文相关用户id</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        /// <returns>返回结果</returns>
        public static BaseResult Request(string key, string info, string userId = null, double? longitude = null, double? latitude = null)
        {
            if (!CheckParameters(key, info, userId, null))
                return null;
            string url = string.Format(URL, key, HttpUtility.UrlEncode(info, ENCODING),
                string.IsNullOrWhiteSpace(userId) ? string.Empty : HttpUtility.UrlEncode(userId, ENCODING),
                string.Empty,
                GetLongitudeOrLatitude(longitude), GetLongitudeOrLatitude(latitude));
            return Request(url);
        }

        /// <summary>
        /// 检查参数是否符合要求
        /// </summary>
        /// <param name="key">开发者的app key</param>
        /// <param name="info">请求内容</param>
        /// <param name="userId">上下文相关用户id</param>
        /// <param name="location">地理位置</param>
        /// <returns>返回参数是否符合要求</returns>
        private static bool CheckParameters(string key, string info, string userId = null, string location = null)
        {
            if(key==null||key.Length!=32)
                return false;
            if(string.IsNullOrWhiteSpace(info)||info.Length>30)
                return false;
            if(!string.IsNullOrEmpty(userId)&&userId.Length>32)
                return false;
            if(!string.IsNullOrEmpty(location)&&location.Length>30)
                return false;
            return true;
        }

        /// <summary>
        /// 获取经纬度的字符串表示形式
        /// </summary>
        /// <param name="l">经度或者纬度</param>
        /// <returns>返回经纬度的字符串表示形式</returns>
        private static string GetLongitudeOrLatitude(double? l)
        {
            string s = string.Empty;
            if (l.HasValue)
                s = ((int)(l.Value * 1000000)).ToString();
            return s;
        }

        /// <summary>
        /// 向图灵服务器发送请求，并获取图灵服务器响应的数据
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseData">返回响应数据</param>
        /// <param name="httpMethod">http方法</param>
        /// <param name="data">数据</param>
        /// <returns>返回是否提交成功</returns>
        private static bool Request(string url, out byte[] responseData,
            string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
        {
            bool success = false;
            responseData = null;
            Stream requestStream = null;
            HttpWebResponse response = null;
            Stream responseStream = null;
            MemoryStream ms = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = httpMethod;
                if (data != null && data.Length > 0)
                {
                    request.ContentLength = data.Length;
                    requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                ms = new MemoryStream();
                responseStream = response.GetResponseStream();
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int size = responseStream.Read(buffer, 0, bufferLength);
                while (size > 0)
                {
                    ms.Write(buffer, 0, size);
                    size = responseStream.Read(buffer, 0, bufferLength);
                }
                responseData = ms.ToArray();
                success = true;
            }
            finally
            {
                if (requestStream != null)
                    requestStream.Close();
                if (responseStream != null)
                    responseStream.Close();
                if (ms != null)
                    ms.Close();
                if (response != null)
                    response.Close();
            }
            return success;
        }

        /// <summary>
        /// 向图灵服务器发送请求，并获取图灵服务器响应的内容
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <param name="responseContent">返回响应内容</param>
        /// <returns>返回是否提交成功</returns>
        private static BaseResult Request(string url)
        {
            BaseResult result = null;
            byte[] responseData;
            string responseContent = string.Empty;
            bool success = Request(url, out responseData, HTTP_METHOD, null);
            if (success && responseData != null && responseData.Length > 0)
            {
                responseContent = ENCODING.GetString(responseData);
                if (!string.IsNullOrWhiteSpace(responseContent))
                    BaseResult.TryParse(responseContent, out result);
            }
            return result;
        }
    }
}