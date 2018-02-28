using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 音乐语义
    /// </summary>
    public class MusicSemantic : BaseSemantic
    {
        /// <summary>
        /// 歌曲名
        /// </summary>
        public string song { get; private set; }
        /// <summary>
        /// 歌手
        /// </summary>
        public string singer { get; private set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string album { get; private set; }
        /// <summary>
        /// 歌曲类型
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; private set; }
        /// <summary>
        /// 电影名
        /// </summary>
        public string movie { get; private set; }
        /// <summary>
        /// 电视剧名
        /// </summary>
        public string tv { get; private set; }
        /// <summary>
        /// 节目名
        /// </summary>
        public string show { get; private set; }
        /// <summary>
        /// 歌曲排序类型
        /// </summary>
        public MusicSortEnum? sort { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            song = joDetails.TryGetValue("song", out jt) ? (string)jt : null;
            singer = joDetails.TryGetValue("singer", out jt) ? (string)jt : null;
            album = joDetails.TryGetValue("album", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
            language = joDetails.TryGetValue("language", out jt) ? (string)jt : null;
            movie = joDetails.TryGetValue("movie", out jt) ? (string)movie : null;
            tv = joDetails.TryGetValue("tv", out jt) ? (string)jt : null;
            show = joDetails.TryGetValue("show", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("sort", out jt))
                sort = (MusicSortEnum)(int)jt;
            else
                sort = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n歌曲名：{1}\r\n歌手：{2}\r\n专辑：{3}\r\n" +
                "歌曲类型：{4}\r\n语言：{5}\r\n电影名：{6}\r\n电视剧名：{7}\r\n" +
                "节目名：{8}\r\n排序类型：{9}",
                base.ToString(), song ?? "", singer ?? "", album ?? "",
                category ?? "", language ?? "", movie ?? "", tv ?? "",
                show ?? "", sort.HasValue ? sort.Value.ToString("g") : "");
        }
    }
}