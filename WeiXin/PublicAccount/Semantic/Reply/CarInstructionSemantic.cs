using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 车载指令语义
    /// </summary>
    public class CarInstructionSemantic : BaseSemantic
    {
        /// <summary>
        /// 数字
        /// </summary>
        public int? number { get; private set; }
        /// <summary>
        /// 窗户位置
        /// </summary>
        public CarInstructionPositionEnum? position { get; private set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public CarInstructionOperatorEnum? Operator { get; private set; }

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
            if (joDetails.TryGetValue("position", out jt))
                position = (CarInstructionPositionEnum)(int)jt;
            else
                position = null;
            if (joDetails.TryGetValue("operator", out jt))
                Operator = (CarInstructionOperatorEnum)Enum.Parse(typeof(CarInstructionOperatorEnum), (string)jt);
            else
                Operator = null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n数字：{1}\r\n窗户位置：{2}\r\n操作值：{3}",
                base.ToString(),
                number.HasValue ? number.Value.ToString() : "",
                position.HasValue ? position.Value.ToString("g") : "",
                Operator.HasValue ? Operator.Value.ToString("g") : "");
        }
    }
}