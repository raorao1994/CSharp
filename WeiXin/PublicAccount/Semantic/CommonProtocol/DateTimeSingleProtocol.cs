using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 单时间协议
    /// </summary>
    public class DateTimeSingleProtocol : CommonProtocol
    {
        /// <summary>
        /// 日期，格式：YYYY-MM-DD，默认是当天时间
        /// </summary>
        public string date { get; private set; }
        /// <summary>
        /// 日期的原始字符串
        /// </summary>
        public string date_ori { get; private set; }
        /// <summary>
        /// 时间，24 小时制，格式：HH:MM:SS ，默认为00:00:00
        /// </summary>
        public string time { get; private set; }
        /// <summary>
        /// 时间的原始字符串
        /// </summary>
        public string time_ori { get; private set; }

        /// <summary>
        /// 获取日期时间
        /// </summary>
        public DateTime DateTime
        {
            get
            {
                System.DateTime dt;
                if (!System.DateTime.TryParse(string.Format("{0} {1}", date, time), out dt))
                    dt = System.DateTime.Today;
                return dt;
            }
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            date = (string)jo["date"];
            date_ori = (string)jo["date_ori"];
            time = (string)jo["time"];
            time_ori = (string)jo["time_ori"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n时间：{1}", base.ToString(), DateTime);
        }
    }

    /// <summary>
    /// 字面时间协议
    /// </summary>
    public class DateTimeOriProtocol : DateTimeSingleProtocol
    { }

    /// <summary>
    /// 推理时间协议
    /// </summary>
    public class DateTimeInferProtocol:DateTimeSingleProtocol
    { }
}
