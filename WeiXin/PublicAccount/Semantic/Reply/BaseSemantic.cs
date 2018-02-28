using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 语义基类
    /// </summary>
    public class BaseSemantic : IParsable
    {
        /// <summary>
        /// 动作
        /// </summary>
        public string action { get; private set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string answer { get; private set; }
        /// <summary>
        /// 对话
        /// </summary>
        public string dialog { get; private set; }
        /// <summary>
        /// 意图
        /// </summary>
        public IntentEnum intent { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            intent = (IntentEnum)Enum.Parse(typeof(IntentEnum), (string)jo["intent"]);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            action = joDetails.TryGetValue("action", out jt) ? (string)jt : null;
            answer = joDetails.TryGetValue("answer", out jt) ? (string)jt : null;
            dialog = joDetails.TryGetValue("dialog", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("动作：{0}\r\n答案：{1}\r\n对话：{2}\r\n意图：{3:g}",
                action ?? "", answer ?? "", dialog ?? "", intent);
        }
    }
}