using System;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 股票语义应答
    /// </summary>
    public class StockReply : BaseReply
    {
        /// <summary>
        /// 股票语义
        /// </summary>
        public StockSemantic semantic { get; private set; }
        /// <summary>
        /// 股票结果
        /// </summary>
        public StockResult[] result { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            semantic = Utility.Parse<StockSemantic>((JObject)jo["semantic"]);
            JToken jt;
            if (jo.TryGetValue("result", out jt) && jt.Type == JTokenType.Array)
            {
                JArray ja = (JArray)jt;
                result = new StockResult[ja.Count];
                for (int i = 0; i < ja.Count; i++)
                {
                    result[i] = new StockResult();
                    result[i].Parse((JObject)ja[i]);
                }
            }
            else
                result = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("语义应答：{0}", semantic);
            if (result != null && result.Length > 0)
            {
                for (int i = 0; i < result.Length; i++)
                    sb.AppendFormat("\r\n股票结果{0}：{1}", i + 1, result[i]);
            }
            return sb.ToString();
        }
    }
}