using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 电视指令语义
    /// </summary>
    public class TvInstructionSemantic : BaseSemantic
    {
        /// <summary>
        /// 电视台名称
        /// </summary>
        public string tv_name { get; private set; }
        /// <summary>
        /// 电视频道名称
        /// </summary>
        public string tv_channel { get; private set; }
        /// <summary>
        /// 节目类型
        /// </summary>
        public string category { get; private set; }
        /// <summary>
        /// 数字
        /// </summary>
        public int? number { get; private set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public TvInstructionValueEnum? value { get; private set; }
        /// <summary>
        /// 操作值
        /// </summary>
        public TvInstructionOperatorEnum? Operator { get; private set; }
        /// <summary>
        /// 设备
        /// </summary>
        public TvInstructionDeviceEnum? device { get; private set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public TvInstructionFileTypeEnum? file_type { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            JObject joDetails = (JObject)jo["details"];
            JToken jt;
            tv_name = joDetails.TryGetValue("tv_name", out jt) ? (string)jt : null;
            tv_channel = joDetails.TryGetValue("tv_channel", out jt) ? (string)jt : null;
            category = joDetails.TryGetValue("category", out jt) ? (string)jt : null;
            if (joDetails.TryGetValue("number", out jt))
                number = (int)jt;
            else
                number = null;
            if (joDetails.TryGetValue("value", out jt))
            {
                string v = (string)jt;
                if (v == "3D")
                    value = TvInstructionValueEnum.ThreeD;
                else
                    value = (TvInstructionValueEnum)Enum.Parse(typeof(TvInstructionValueEnum), v);
            }
            else
                value = null;
            if (joDetails.TryGetValue("operator", out jt))
                Operator = (TvInstructionOperatorEnum)Enum.Parse(typeof(TvInstructionOperatorEnum), (string)jt);
            else
                Operator = null;
            if (joDetails.TryGetValue("device", out jt))
                device = (TvInstructionDeviceEnum)Enum.Parse(typeof(TvInstructionDeviceEnum), (string)jt);
            else
                device = null;
            if (joDetails.TryGetValue("file_type", out jt))
                file_type = (TvInstructionFileTypeEnum)Enum.Parse(typeof(TvInstructionFileTypeEnum), (string)jt);
            else
                file_type = null;
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
            return string.Format("{0}\r\n电视台名称：{1}\r\n电视频道名称：{2}\r\n节目类型：{3}\r\n" +
                "操作值：{4}\r\n设备：{5}\r\n文件类型：{6}",
                base.ToString(),
                tv_name ?? "",
                tv_channel ?? "",
                category ?? "",
                v,
                device.HasValue ? device.Value.ToString("g") : "",
                file_type.HasValue ? file_type.Value.ToString("g") : "");
        }
    }
}