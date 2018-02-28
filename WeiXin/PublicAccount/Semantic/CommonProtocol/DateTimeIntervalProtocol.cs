using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 时间段协议
    /// </summary>
    public class DateTimeIntervalProtocol : DateTimeSingleProtocol
    {
        /// <summary>
        /// 终止日期，格式：YYYY-MM-DD，默认是当前时间
        /// </summary>
        public string end_date { get; private set; }
        /// <summary>
        /// 终止日期的原始字符串
        /// </summary>
        public string end_date_ori { get; private set; }
        /// <summary>
        /// 终止时间，24 小时制，格式：HH:MM:SS ，默认为00:00:00
        /// </summary>
        public string end_time { get; private set; }
        /// <summary>
        /// 终止时间的原始字符串
        /// </summary>
        public string end_time_ori { get; private set; }

        /// <summary>
        /// 获取终止日期时间
        /// </summary>
        public DateTime EndDateTime
        {
            get
            {
                System.DateTime dt;
                if (!System.DateTime.TryParse(string.Format("{0} {1}", end_date, end_time), out dt))
                    dt = System.DateTime.Now;
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
            end_date = (string)jo["end_date"];
            end_date_ori = (string)jo["end_date_ori"];
            end_time = (string)jo["end_time"];
            end_time_ori = (string)jo["end_time_ori"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n终止时间：{1}", base.ToString(), EndDateTime);
        }
    }
}