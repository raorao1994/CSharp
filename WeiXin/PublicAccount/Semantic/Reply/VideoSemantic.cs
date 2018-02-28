using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 视频语义
    /// </summary>
    public class VideoSemantic : BaseSemantic
    {
        /// <summary>
        /// 视频名
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 主演/嘉宾
        /// </summary>
        public string actor { get; private set; }
        /// <summary>
        /// 导演/主持人
        /// </summary>
        public string director { get; private set; }
        /// <summary>
        /// 视频类型
        /// </summary>
        public VideoCategoryEnum? category { get; private set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string tag { get; private set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string country { get; private set; }
        /// <summary>
        /// 季/部
        /// </summary>
        public NumberProtocol season { get; private set; }
        /// <summary>
        /// 集
        /// </summary>
        public NumberProtocol episode { get; private set; }
        /// <summary>
        /// 排序类型
        /// </summary>
        public VideoSortEnum? sort { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            name = joDetails.TryGetValue("name", out jt) ? (string)jt : null;
            actor = joDetails.TryGetValue("actor", out jt) ? (string)jt : null;
            director = joDetails.TryGetValue("director", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("category", out jt))
                category = (VideoCategoryEnum)Enum.Parse(typeof(VideoCategoryEnum), (string)jt);
            else
                category = null;
            tag = joDetails.TryGetValue("tag", out jt) ? (string)jt : null;
            country = joDetails.TryGetValue("country", out jt) ? (string)jt : null;
            season = joDetails.TryGetValue("season", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            episode = joDetails.TryGetValue("episode", out jt) ? (NumberProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (VideoSortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n视频名：{1}\r\n主演/嘉宾：{2}\r\n导演/主持人：{3}\r\n" +
                "视频类型：{4}\r\n类型：{5}\r\n地区：{6}\r\n季/部：{7}\r\n" +
                "集：{8}\r\n排序类型：{9}",
                base.ToString(), name ?? "", actor ?? "", director ?? "",
                category.HasValue ? category.Value.ToString("g") : "",
                tag ?? "", country ?? "",
                season != null ? season.ToString() : "",
                episode != null ? episode.ToString() : "",
                sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}