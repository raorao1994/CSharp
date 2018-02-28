using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 电视节目预告语义
    /// </summary>
    public class TvSemantic : BaseSemantic
    {
        /// <summary>
        /// 播放时间
        /// </summary>
        public DateTimeSingleProtocol datetime { get; private set; }
        /// <summary>
        /// 电视台名称
        /// </summary>
        public string tv_name { get; private set; }
        /// <summary>
        /// 电视台频道
        /// </summary>
        public string tv_channel { get; private set; }
        /// <summary>
        /// 节目名称
        /// </summary>
        public string show_name { get; private set; }
        /// <summary>
        /// 节目类型
        /// </summary>
        public string category { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            tv_name = joDetails.TryGetValue("tv_name", out jt) ? (string)jt : null;
            tv_channel = joDetails.TryGetValue("tv_channel", out jt) ? (string)jt : null;
            show_name = joDetails.TryGetValue("show_name", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n播放时间：{1}\r\n电视台名称：{2}\r\n电视台频道：{3}\r\n" +
                "节目名称：{4}\r\n节目类型：{5}",
                base.ToString(),
                datetime != null ? datetime.ToString() : "",
                tv_name ?? "", tv_channel ?? "",
                show_name ?? "", category ?? "");
        }
    }
}