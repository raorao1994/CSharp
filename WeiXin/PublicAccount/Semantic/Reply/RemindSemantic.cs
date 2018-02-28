using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 提醒语义
    /// </summary>
    public class RemindSemantic : BaseSemantic
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTimeSingleProtocol datetime { get; private set; }
        /// <summary>
        /// 事件
        /// </summary>
        public string Event { get; private set; }
        /// <summary>
        /// 类别
        /// </summary>
        public RemindTypeEnum? remind_type { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            datetime = joDetails.TryGetValue("datetime", out jt) ? (DateTimeSingleProtocol)CommonProtocol.ParseObject((JObject)jt) : null;
            Event = joDetails.TryGetValue("event", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("remind_type", out jt))
                remind_type = (RemindTypeEnum)(int)jt;
            else
                remind_type = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n时间：{1}\r\n事件：{2}\r\n类别：{3}",
                base.ToString(),
                datetime != null ? datetime.ToString() : "",
                Event ?? "",
                remind_type.HasValue ? remind_type.Value.ToString("g") : "");
        }
    }
}