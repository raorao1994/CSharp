using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// MultiMediaHelper：多媒体辅助类
    /// </summary>
    public static class MultiMediaHelper
    {
        /// <summary>
        /// 上传多媒体文件的地址
        /// </summary>
        private const string urlForUploadingMedia = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
        /// <summary>
        /// 上传图文消息的地址
        /// </summary>
        private const string urlForUploadingArticles = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
        /// <summary>
        /// 下载多媒体文件的地址
        /// </summary>
        private const string urlForDownloadingMedia = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
        /// <summary>
        /// 下载多媒体文件的http方法
        /// </summary>
        private const string httpMethodForDownloading = WebRequestMethods.Http.Get;
        
        /// <summary>
        /// 获取视频媒体ID的地址
        /// </summary>
        private const string urlForGettingVideoMediaId = "https://file.api.weixin.qq.com/cgi-bin/media/uploadvideo?access_token={0}";
        /// <summary>
        /// 获取视频媒体ID的http方法
        /// </summary>
        private const string httpMethodForGettingVideoMediaId = WebRequestMethods.Http.Post;

        /// <summary>
        /// 解析响应字符串
        /// </summary>
        /// <param name="responseContent">响应字符串</param>
        /// <param name="errorMessage">返回错误消息</param>
        /// <returns>返回多媒体上传结果</returns>
        private static MultiMediaUploadResult ParseResult(string responseContent, out ErrorMessage errorMessage)
        {
            MultiMediaUploadResult result = null;
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "上传失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                if (MultiMediaUploadResult.TryParse(responseContent, out result))
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "上传成功。");
                else
                    errorMessage= new ErrorMessage(ErrorMessage.ExceptionCode, "解析多媒体上传结果失败。");
            }
            return result;
        }

        /// <summary>
        /// 上传多媒体文件
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">多媒体类型</param>
        /// <param name="pathName">包含路径的文件名</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回多媒体上传结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult Upload(string userName, MultiMediaTypeEnum type, string pathName, out ErrorMessage errorMessage)
        {
            if (type == MultiMediaTypeEnum.news)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "不能使用该方法上传图文消息，请使用上传图文消息的重载方法。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = string.Format(urlForUploadingMedia, token.access_token, type.ToString("g"));
            string responseContent = HttpHelper.Upload(url, pathName);
            return ParseResult(responseContent, out errorMessage);
        }

        /// <summary>
        /// 上传多媒体文件
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">多媒体类型</param>
        /// <param name="fileName">文件名（不包含路径）</param>
        /// <param name="data">数据</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回上传多媒体文件的结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult Upload(string userName, MultiMediaTypeEnum type, string fileName, byte[] data, out ErrorMessage errorMessage)
        {
            if (type == MultiMediaTypeEnum.news)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "不能使用该方法上传图文消息，请使用上传图文消息的重载方法。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = string.Format(urlForUploadingMedia, token.access_token, type.ToString("g"));
            string responseContent = HttpHelper.Upload(url, fileName, data);
            return ParseResult(responseContent, out errorMessage);
        }

        /// <summary>
        /// 上传图文消息
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="articles">图文消息</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回上传图文消息的结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult Upload(string userName, IEnumerable<MultiMediaArticle> articles, out ErrorMessage errorMessage)
        {
            if (articles == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "图文消息不能为空。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = string.Format(urlForUploadingArticles, token.access_token);
            string responseContent;
            HttpHelper.Request(url, out responseContent, WebRequestMethods.Http.Post, MultiMediaArticle.ToJson(articles));
            return ParseResult(responseContent, out errorMessage);
        }

        /// <summary>
        /// 获取下载多媒体文件的链接
        /// </summary>
        /// <param name="token">许可令牌</param>
        /// <param name="mediaId">媒体id</param>
        /// <returns>返回下载多媒体文件的链接</returns>
        public static string GetDownloadUrl(string token, string mediaId)
        {
            return string.Format(urlForDownloadingMedia, token, mediaId);
        }

        /// <summary>
        /// 下载多媒体文件
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">媒体ID</param>
        /// <param name="data">返回下载是否成功</param>
        /// <returns>返回多媒体文件数据；如果下载失败，返回null。</returns>
        public static byte[] Download(string userName, string mediaId, out ErrorMessage errorMessage)
        {
            if (string.IsNullOrWhiteSpace(mediaId))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "媒体id不能为空。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = GetDownloadUrl(token.access_token, mediaId);
            byte[] data;
            bool success = HttpHelper.Request(url, out data, httpMethodForDownloading, (byte[])null);
            if (!success)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "从微信服务器下载多媒体文件失败。");
                return null;
            }
            byte[] error = HttpHelper.ResponseEncoding.GetBytes("{\"errorcode\":");
            if (StartsWithBytes(data, error))
            {
                string json = HttpHelper.ResponseEncoding.GetString(data);
                data = null;
                if (ErrorMessage.IsErrorMessage(json))
                    errorMessage = ErrorMessage.Parse(json);
                else
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "从微信服务器下载多媒体文件失败。");
            }
            else
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "下载多媒体文件成功。");
            return data;
        }

        /// <summary>
        /// 判断目标字节数组是否位于源字节数组的开始
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="target">目标字节数组</param>
        /// <returns>返回目标字节数组是否位于源字节数组的开始</returns>
        private static bool StartsWithBytes(byte[] source, byte[] target)
        {
            if (source == null && target == null)
                return true;
            if (source == null && target != null || source != null && target == null)
                return false;
            if (source.Length < target.Length)
                return false;
            bool startsWith = true;
            for (int i = 0; i < target.Length; i++)
            {
                if (source[i] != target[i])
                {
                    startsWith = false;
                    break;
                }
            }
            return startsWith;
        }

        /// <summary>
        /// 得到消息群发中的视频媒体ID
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">上传得到的视频媒体ID</param>
        /// <param name="title">标题</param>
        /// <param name="description">描述</param>
        /// <returns>返回消息群发中的视频媒体ID</returns>
        public static string GetVideoMediaId(string userName, string mediaId, string title, string description)
        {
            string videoMediaId = string.Empty;
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
                return videoMediaId;
            string url = string.Format(urlForGettingVideoMediaId, token.access_token);
            var data = new
            {
                media_id = mediaId,
                title = title,
                description = description
            };
            string dataJson = JsonConvert.SerializeObject(data);
            string responseContent;
            if (!HttpHelper.Request(url, out responseContent, httpMethodForGettingVideoMediaId, dataJson))
                return videoMediaId;
            JObject jo = JObject.Parse(responseContent);
            JToken jt;
            if (jo.TryGetValue("media_id", out jt))
                videoMediaId = (string)jt;
            return videoMediaId;
        }
    }
}