using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 素材项
    /// </summary>
    public class MeterialItem : IParsable
    {
        /// <summary>
        /// 媒体id
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            MediaId = (string)jo["media_id"];
            int updateTime = (int)jo["update_time"];
            UpdateTime = Utility.ToDateTime(updateTime);
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        public static MeterialItem ParseItem(JObject jo)
        {
            MeterialItem item;
            JToken jt;
            if (jo.TryGetValue("name", out jt))
                item = new MeterialOtherItem();
            else
                item = new MeterialNewsItem();
            item.Parse(jo);
            return item;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("媒体id：{0}\r\n更新时间：{1}",
                MediaId, UpdateTime);
        }
    }

    /// <summary>
    /// 素材项-图文消息
    /// </summary>
    public class MeterialNewsItem : MeterialItem
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        public MultiMediaArticle[] Content { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JToken jt;
            if (jo.TryGetValue("content", out jt) && jt.Type == JTokenType.Object &&
                ((JObject)jt).TryGetValue("news_item", out jt) && jt.Type == JTokenType.Array && ((JArray)jt).Count > 0)
            {
                JArray ja = (JArray)jt;
                Content = new MultiMediaArticle[ja.Count];
                for (int i = 0; i < ja.Count; i++)
                    Content[i] = new MultiMediaArticle((JObject)ja[i]);
            }
            else
                Content = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            if (Content != null && Content.Length > 0)
            {
                sb.AppendFormat("图文数：{0}", Content.Length);
                for (int i = 0; i < Content.Length; i++)
                    sb.AppendFormat("\r\n图文{0}：\r\n{1}", i, Content[i]);
            }
            else
                sb.Append("图文数：0");
            return sb.ToString();
        }
    }

    /// <summary>
    /// 素材项-图片、语音、视频
    /// </summary>
    public class MeterialOtherItem : MeterialItem
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JToken jt;
            Name = jo.TryGetValue("name", out jt) ? (string)jt : "";
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n文件名称：{1}",
                base.ToString(), Name);
        }
    }
}