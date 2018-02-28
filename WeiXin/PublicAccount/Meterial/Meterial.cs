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
    /// 素材管理。
    /// 注：我的账号权限不足，素材管理未经完全测试。
    /// </summary>
    public static class Meterial
    {
        #region Temporary
        /// <summary>
        /// 新增临时素材（多媒体文件）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="pathName">包含路径的文件名</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回多媒体上传结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult AddTemporary(string userName, MultiMediaTypeEnum type, string pathName, out ErrorMessage errorMessage)
        {
            return MultiMediaHelper.Upload(userName, type, pathName, out errorMessage);
        }

        /// <summary>
        /// 新增临时素材（多媒体文件）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="fileName">文件名（不包含路径）</param>
        /// <param name="data">数据</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回上传多媒体文件的结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult AddTemporary(string userName, MultiMediaTypeEnum type, string fileName, byte[] data, out ErrorMessage errorMessage)
        {
            return MultiMediaHelper.Upload(userName, type, fileName, data, out errorMessage);
        }

        /// <summary>
        /// 新增临时素材（图文消息）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="articles">图文消息</param>
        /// <param name="errorMessage">返回上传是否成功</param>
        /// <returns>返回上传图文消息的结果；如果上传失败，返回null。</returns>
        public static MultiMediaUploadResult AddTemporary(string userName, IEnumerable<MultiMediaArticle> articles, out ErrorMessage errorMessage)
        {
            return MultiMediaHelper.Upload(userName, articles, out errorMessage);
        }

        /// <summary>
        /// 获取下载临时素材（媒体文件，不包括视频）的链接
        /// </summary>
        /// <param name="token">许可令牌</param>
        /// <param name="mediaId">媒体id</param>
        /// <returns>返回下载多媒体文件的链接</returns>
        public static string GetDownloadUrlOfTemporary(string token, string mediaId)
        {
            return MultiMediaHelper.GetDownloadUrl(token, mediaId);
        }

        /// <summary>
        /// 下载临时素材（多媒体文件）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">媒体ID</param>
        /// <param name="data">返回下载是否成功</param>
        /// <returns>返回多媒体文件数据；如果下载失败，返回null。</returns>
        public static byte[] Download(string userName, string mediaId, out ErrorMessage errorMessage)
        {
            return MultiMediaHelper.Download(userName, mediaId, out errorMessage);
        }
        #endregion //temporary

        #region immortal
        /// <summary>
        /// 编辑（新增、获取、删除、修改）永久素材的http方法
        /// </summary>
        private const string httpMethodForEditing = WebRequestMethods.Http.Post;
        /// <summary>
        /// 新增永久图文素材的地址
        /// </summary>
        private const string urlForAddingNews = "https://api.weixin.qq.com/cgi-bin/material/add_news?access_token={0}";
        /// <summary>
        /// 新增永久多媒体素材的地址
        /// </summary>
        private const string urlForAddingMeterial = "http://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}&type={1}";
        /// <summary>
        /// 删除永久素材的地址
        /// </summary>
        private const string urlForDeletingMeterial = "https://api.weixin.qq.com/cgi-bin/material/del_material?access_token={0}";
        /// <summary>
        /// 修改永久图文素材的地址
        /// </summary>
        private const string urlForUpdatingNews = "https://api.weixin.qq.com/cgi-bin/material/update_news?access_token={0}";
        /// <summary>
        /// 获取永久素材的http方法
        /// </summary>
        private const string httpMethodForGettingMeterial = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取永久素材的地址
        /// </summary>
        private const string urlForGettingMeterial = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token={0}";
        /// <summary>
        /// 获取永久素材总数的http方法
        /// </summary>
        private const string httpMethodForGettingMeterialCount = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取永久素材总数的地址
        /// </summary>
        private const string urlForGettingMeterialCount = "https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token={0}";
        /// <summary>
        /// 获取永久素材列表的http方法
        /// </summary>
        private const string httpMethodForBatchGettingMeterial = WebRequestMethods.Http.Post;
        /// <summary>
        /// 获取永久素材列表的地址
        /// </summary>
        private const string urlForBatchGettingMeterial = "https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}";

        /// <summary>
        /// 新增永久图文素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="articles">图文消息</param>
        /// <param name="errorMessage">返回新增是否成功</param>
        /// <returns>返回图文消息的媒体id；如果失败，返回null。</returns>
        public static string Add(string userName, IEnumerable<MultiMediaArticle> articles, out ErrorMessage errorMessage)
        {
            string responseContent = HttpHelper.RequestResponseContent(urlForAddingNews, userName,
                null, httpMethodForEditing, MultiMediaArticle.ToJson(articles));
            return GetMediaId(responseContent, out errorMessage);
        }

        /// <summary>
        /// 新增永久素材（多媒体文件）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="pathName">包含路径的文件名</param>
        /// <param name="errorMessage">返回新增是否成功</param>
        /// <param name="title">视频素材的标题（对于其他媒体类型，忽略该参数）</param>
        /// <param name="introduction">视频素材的描述（对于其他媒体类型，忽略该参数）</param>
        /// <returns>返回多媒体的id；如果上传失败，返回null。</returns>
        public static string Add(string userName, MultiMediaTypeEnum type, string pathName,
            out ErrorMessage errorMessage, string title = null, string introduction = null)
        {
            if (type == MultiMediaTypeEnum.news)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "不能使用该方法新增永久图文消息，请使用新增图文消息的重载方法。");
                return null;
            }
            if (type == MultiMediaTypeEnum.video && (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(introduction)))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "新增永久视频素材时，必须提供标题和描述。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = string.Format(urlForAddingMeterial, token.access_token, type.ToString("g"));
            NameValueCollection formData = null;
            if (type == MultiMediaTypeEnum.video)
                formData.Add("description", JsonConvert.SerializeObject(new { title = title, introduction = introduction }));
            string responseContent = HttpHelper.Upload(url, pathName, formData);
            return GetMediaId(responseContent, out errorMessage);
        }

        /// <summary>
        /// 新增永久素材（多媒体文件）
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="fileName">文件名（不包含路径）</param>
        /// <param name="data">数据</param>
        /// <param name="errorMessage">返回新增是否成功</param>
        /// <param name="title">视频素材的标题（对于其他媒体类型，忽略该参数）</param>
        /// <param name="introduction">视频素材的描述（对于其他媒体类型，忽略该参数）</param>
        /// <returns>返回多媒体的id；如果上传失败，返回null。</returns>
        public static string Add(string userName, MultiMediaTypeEnum type, string fileName, byte[] data,
            out ErrorMessage errorMessage, string title = null, string introduction = null)
        {
            if (type == MultiMediaTypeEnum.news)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "不能使用该方法新增永久图文消息，请使用新增图文消息的重载方法。");
                return null;
            }
            if (type == MultiMediaTypeEnum.video && (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(introduction)))
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "新增永久视频素材时，必须提供标题和描述。");
                return null;
            }
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取许可令牌失败。");
                return null;
            }
            string url = string.Format(urlForAddingMeterial, token.access_token, type.ToString("g"));
            NameValueCollection formData = null;
            if (type == MultiMediaTypeEnum.video)
                formData.Add("description", JsonConvert.SerializeObject(new { title = title, introduction = introduction }));
            string responseContent = HttpHelper.Upload(url, fileName, data, formData);
            return GetMediaId(responseContent, out errorMessage);
        }

        /// <summary>
        /// 从响应中获取媒体id
        /// </summary>
        /// <param name="responseContent">响应内容</param>
        /// <param name="errorMessage">返回是否成功</param>
        /// <returns>返回媒体id；如果失败，返回null。</returns>
        private static string GetMediaId(string responseContent, out ErrorMessage errorMessage)
        {
            if (ErrorMessage.IsErrorMessage(responseContent))
            {
                errorMessage = ErrorMessage.Parse(responseContent);
                return null;
            }
            else
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "新增永久图文素材成功。");
                return responseContent;
            }
        }

        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">媒体id</param>
        /// <returns>返回素材的字节数据；如果失败，返回null。</returns>
        private static byte[] Get(string userName, string mediaId)
        {
            AccessToken token = AccessToken.Get(userName);
            if (token == null)
                return null;
            string url = string.Format(urlForGettingMeterial, token.access_token);
            byte[] responseData;
            string json = JsonConvert.SerializeObject(new { media_id = mediaId });
            HttpHelper.Request(url, out responseData, httpMethodForGettingMeterial, json);
            return responseData;
        }

        /// <summary>
        /// 获取永久图文素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">图文素材的id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回图文消息；如果获取失败，返回null。</returns>
        public static MultiMediaArticle[] GetNews(string userName, string mediaId, out ErrorMessage errorMessage)
        {
            byte[] responseData = Get(userName, mediaId);
            if (responseData == null || responseData.Length == 0)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取永久图文素材失败。");
                return null;
            }
            string responseContent = HttpHelper.ResponseEncoding.GetString(responseData);
            if (ErrorMessage.IsErrorMessage(responseContent))
            {
                errorMessage = ErrorMessage.Parse(responseContent);
                return null;
            }
            else
            {
                JObject jo = JObject.Parse(responseContent);
                JToken jt;
                if (jo.TryGetValue("news_item", out jt))
                {
                    errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "获取永久图文素材成功。");
                    JArray ja = (JArray)jt;
                    MultiMediaArticle[] articles = new MultiMediaArticle[ja.Count];
                    for (int i = 0; i < articles.Length; i++)
                        articles[i] = new MultiMediaArticle((JObject)ja[i]);
                    return articles;
                }
                else
                {
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取永久图文素材失败。");
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取永久视频素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">视频素材的id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回视频素材；如果获取失败，返回null。</returns>
        public static VideoMeterial GetVideo(string userName, string mediaId, out ErrorMessage errorMessage)
        {
            string json = JsonConvert.SerializeObject(new { media_id = mediaId });
            return HttpHelper.RequestParsableResult<VideoMeterial>(urlForGettingMeterial, userName, out errorMessage, null, httpMethodForGettingMeterial, json);
        }

        /// <summary>
        /// 获取永久素材（图片（image）、语音（voice）、缩略图（thumb））
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">素材的id</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回素材数据；如果获取失败，返回null。</returns>
        public static byte[] Get(string userName, string mediaId, out ErrorMessage errorMessage)
        {
            byte[] responseData = Get(userName, mediaId);
            string error = "{\"errcode\":40007,\"errmsg\":\"invalid media_id\"}";
            byte[] errorData = HttpHelper.ResponseEncoding.GetBytes(error);
            if (responseData == null || responseData.Length == 0)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取永久素材失败。");
                return null;
            }
            else if (responseData.Length > errorData.Length * 3)
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "获取永久素材成功。");
                return responseData;
            }
            else
            {
                try
                {
                    string responseContent = HttpHelper.ResponseEncoding.GetString(responseData);
                    errorMessage = ErrorMessage.Parse(responseContent);
                }
                catch
                {
                    errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "获取永久素材失败。");
                }
                return null;
            }
        }

        /// <summary>
        /// 删除永久素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">媒体id</param>
        /// <returns>返回删除是否成功</returns>
        public static ErrorMessage Delete(string userName,string mediaId)
        {
            string json = JsonConvert.SerializeObject(new { media_id = mediaId });
            return HttpHelper.RequestErrorMessage(urlForDeletingMeterial, userName, null, httpMethodForEditing, json);
        }

        /// <summary>
        /// 修改永久图文素材
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="mediaId">媒体id</param>
        /// <param name="index">要更新的文章在图文消息中的位置（多图文消息时，此字段才有意义），第一篇为0</param>
        /// <param name="articles">图文素材</param>
        /// <returns>返回修改是否成功</returns>
        public static ErrorMessage UpdateNews(string userName,string mediaId,int index,IEnumerable<MultiMediaArticle> articles)
        {
            List<object> objs;
            if (articles == null)
                objs = null;
            else
            {
                objs = new List<object>();
                foreach (MultiMediaArticle article in articles)
                    objs.Add(article.ToAnonymousObject());
            }
            string json = JsonConvert.SerializeObject(new { media_id = mediaId, index = index, articles = objs });
            return HttpHelper.RequestErrorMessage(urlForUpdatingNews, userName, null, httpMethodForEditing, json);
        }

        /// <summary>
        /// 获取素材总数
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回是否获取成功</param>
        /// <returns>返回素材总数</returns>
        public static MeterialCount GetMeterialCount(string userName,out ErrorMessage errorMessage)
        {
            return HttpHelper.RequestParsableResult<MeterialCount>(urlForGettingMeterialCount, userName, out errorMessage, null, httpMethodForGettingMeterialCount, null);
        }

        /// <summary>
        /// 批量获取素材列表
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="type">素材类型</param>
        /// <param name="offset">从全部素材的该偏移位置开始返回，0表示从第一个素材返回</param>
        /// <param name="count">返回素材的数量，取值在1到20之间</param>
        /// <param name="errorMessage">返回是否获取成功</param>
        /// <returns>返回素材列表；如果获取失败，返回null。</returns>
        public static BatchMeterial BatchGet(string userName,MultiMediaTypeEnum type,int offset,int count,out ErrorMessage errorMessage)
        {
            if(offset<0)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "偏移位置不能小于0。");
                return null;
            }
            if(count<1||count>20)
            {
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "素材数量必须介于1至20之间。");
                return null;
            }
            string json = JsonConvert.SerializeObject(new { type = type.ToString("g"), offset = offset, count = count });
            return HttpHelper.RequestParsableResult<BatchMeterial>(urlForBatchGettingMeterial, userName, out errorMessage, null, httpMethodForBatchGettingMeterial, json);
        }
        #endregion //immortal
    }
}
