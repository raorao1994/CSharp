using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 航班结果
    /// </summary>
    public class FlightResult : BaseResult
    {
        /// <summary>
        /// 航班列表
        /// </summary>
        public List<Flight> Flights { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="flights"></param>
        internal FlightResult(string text, IEnumerable<Flight> flights)
            : base(CodeEnum.Flight, text)
        {
            if (flights == null)
                throw new ArgumentNullException("flights", "航班列表不能为空。");
            Flights = new List<Flight>(flights);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("航班数：{0}", Flights.Count);
            if (Flights.Count > 0)
            {
                foreach (Flight flight in Flights)
                    sb.AppendFormat("\r\n{0}", flight);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析航班结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static FlightResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            Flight[] flights = new Flight[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                flights[idx] = new Flight(item);
                idx++;
            }
            return new FlightResult(text, flights);
        }
    }
}
