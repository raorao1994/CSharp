using System;
using Newtonsoft.Json.Linq;
namespace PublicAccount.Semantic
{
    /// <summary>
    /// 通用指令语义
    /// </summary>
    public class InstructionSemantic : BaseSemantic
    {
        /// <summary>
        /// 数字
        /// </summary>
        public int? number { get; private set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public InstructionValueEnum? value { get; private set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public InstructionOperatorEnum? Operator { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            if (joDetails.TryGetValue("number", out jt))
                number = (int)jt;
            else
                number = null;
            if (joDetails.TryGetValue("value", out jt))
                value = (InstructionValueEnum)Enum.Parse(typeof(InstructionValueEnum), (string)jt);
            else
                value = null;
            if (joDetails.TryGetValue("operator", out jt))
                Operator = (InstructionOperatorEnum)Enum.Parse(typeof(InstructionOperatorEnum), (string)jt);
            else
                Operator = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string v = "";
            if (number.HasValue)
                v = number.Value.ToString();
            else if (value.HasValue)
                v = value.Value.ToString("g");
            else if (Operator.HasValue)
                v = Operator.Value.ToString("g");
            return string.Format("{0}\r\n操作值：{1}",
                base.ToString(), v);
        }
    }
}