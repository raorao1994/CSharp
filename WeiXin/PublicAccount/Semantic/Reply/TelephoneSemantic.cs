using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 常用电话语义
    /// </summary>
    public class TelephoneSemantic : BaseSemantic
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone { get; private set; }

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
            telephone = joDetails.TryGetValue("telephone", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n名字：{1}\r\n电话：{2}",
                base.ToString(), name ?? "", telephone ?? "");
        }
    }
}