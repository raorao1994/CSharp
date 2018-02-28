using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 素材总数
    /// </summary>
    public class MeterialCount : IParsable
    {
        /// <summary>
        /// 语音总数
        /// </summary>
        public int VoiceCount { get; set; }
        /// <summary>
        /// 视频总数
        /// </summary>
        public int VideoCount { get; set; }
        /// <summary>
        /// 图片总数
        /// </summary>
        public int ImageCount { get; set; }
        /// <summary>
        /// 图文总数
        /// </summary>
        public int NewsCount { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            JToken jt;
            VoiceCount = jo.TryGetValue("voice_count", out jt) ? (int)jt : 0;
            VideoCount = jo.TryGetValue("video_count", out jt) ? (int)jt : 0;
            ImageCount = jo.TryGetValue("image_count", out jt) ? (int)jt : 0;
            NewsCount = jo.TryGetValue("news_count", out jt) ? (int)jt : 0;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("语音总数：{0}\r\n视频总数：{1}\r\n图片总数：{2}\r\n图文总数：{3}",
                VoiceCount, VideoCount, ImageCount, NewsCount);
        }
    }
}