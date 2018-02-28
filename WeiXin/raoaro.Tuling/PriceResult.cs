using System;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 价格结果
    /// </summary>
    public class PriceResult : BaseResult
    {
        /// <summary>
        /// 价格列表
        /// </summary>
        public List<Price> Prices { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="prices"></param>
        internal PriceResult(string text, IEnumerable<Price> prices)
            : base(CodeEnum.Price, text)
        {
            if (prices == null)
                throw new ArgumentNullException("prices", "价格列表不能为空。");
            Prices = new List<Price>(prices);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendFormat("价格数目：{0}", Prices.Count);
            if (Prices.Count > 0)
            {
                foreach (Price price in Prices)
                    sb.AppendFormat("\r\n{0}", price);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析价格结果
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        internal static PriceResult Parse(JObject jo)
        {
            string text = (string)jo["text"];
            JArray ja = (JArray)jo["list"];
            Price[] prices = new Price[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                prices[idx] = new Price(item);
                idx++;
            }
            return new PriceResult(text, prices);
        }
    }
}