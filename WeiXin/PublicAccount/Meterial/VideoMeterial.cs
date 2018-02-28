using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 视频素材
    /// </summary>
    public class VideoMeterial : IParsable
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        public string DownUrl { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            JToken jt;
            Title = jo.TryGetValue("title", out jt) ? (string)jt : "";
            Description = jo.TryGetValue("description", out jt) ? (string)jt : "";
            DownUrl = jo.TryGetValue("down_url", out jt) ? (string)jt : "";
        }
    }
}