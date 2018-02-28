using System;
using System.Net;
using Newtonsoft.Json;

namespace PublicAccount
{
    /// <summary>
    /// 短链接
    /// </summary>
    public static class ShortUrl
    {
        /// <summary>
        /// 地址
        /// </summary>
        private const string urlForGettingShortUrl = "https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}";
        /// <summary>
        /// 将长链接转成短链接的http方法
        /// </summary>
        private const string httpMethodForGettingShortUrl = WebRequestMethods.Http.Post;
        /// <summary>
        /// 将长链接转成短链接的动作
        /// </summary>
        private const string actionForGettingShortUrl = "long2short";

        /// <summary>
        /// 将长链接转成短链接
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="longUrl">长链接</param>
        /// <param name="errorMessage">返回转换是否成功</param>
        /// <returns>返回短链接；如果转换失败，返回空字符串。</returns>
        public static string Get(string userName, string longUrl, out ErrorMessage errorMessage)
        {
            string url = string.Empty;
            string json = JsonConvert.SerializeObject(new { action = actionForGettingShortUrl, long_url = longUrl });
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingShortUrl,
                userName, null, httpMethodForGettingShortUrl, json);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else
            {
                var result = JsonConvert.DeserializeAnonymousType(responseContent, new { errcode = 0, errmsg = "", short_url = "" });
                url = result.short_url;
                errorMessage = new ErrorMessage(result.errcode, result.errmsg);
            }
            return url;
        }
    }
}
