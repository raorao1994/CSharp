using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 酒店结果
    /// </summary>
    public class HotelResult : BaseResult
    {
        /// <summary>
        /// 酒店列表
        /// </summary>
        public List<Hotel> Hotels { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="hotels"></param>
        internal HotelResult(string text, IEnumerable<Hotel> hotels)
            : base(CodeEnum.Hotel, text)
        {
            if (hotels == null)
                throw new ArgumentNullException("hotels", "酒店列表不能为空。");
            Hotels = new List<Hotel>(hotels);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("酒店数：{0}", Hotels.Count);
            if (Hotels.Count > 0)
            {
                foreach (Hotel hotel in Hotels)
                    sb.AppendFormat("\r\n{0}", hotel);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析酒店结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static HotelResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            Hotel[] hotels = new Hotel[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                hotels[idx] = new Hotel(item);
                idx++;
            }
            return new HotelResult(text, hotels);
        }
    }
}