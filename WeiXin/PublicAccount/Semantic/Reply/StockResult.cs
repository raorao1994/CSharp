using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 股票结果
    /// </summary>
    public class StockResult : IParsable
    {
        /// <summary>
        /// 股票代码
        /// </summary>
        public string cd { get; private set; }
        /// <summary>
        /// 当前价
        /// </summary>
        public string np { get; private set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string ap { get; private set; }
        /// <summary>
        /// 涨幅比率
        /// </summary>
        public string apn { get; private set; }
        /// <summary>
        /// 最高价
        /// </summary>
        public string tp_max { get; private set; }
        /// <summary>
        /// 最低价
        /// </summary>
        public string tp_min { get; private set; }
        /// <summary>
        /// 成交量（单位：万）
        /// </summary>
        public string dn { get; private set; }
        /// <summary>
        /// 成交额（单位：亿）
        /// </summary>
        public string de { get; private set; }
        /// <summary>
        /// 市盈率
        /// </summary>
        public string pe { get; private set; }
        /// <summary>
        /// 市值（单位：亿）
        /// </summary>
        public string sz { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            JToken jt;
            cd = jo.TryGetValue("cd", out jt) ? (string)jt : null;
            np = jo.TryGetValue("np", out jt) ? (string)jt : null;
            ap = jo.TryGetValue("ap", out jt) ? (string)jt : null;
            apn = jo.TryGetValue("apn", out jt) ? (string)jt : null;
            tp_max = jo.TryGetValue("tp_max", out jt) ? (string)jt : null;
            tp_min = jo.TryGetValue("tp_min", out jt) ? (string)jt : null;
            dn = jo.TryGetValue("dn", out jt) ? (string)jt : null;
            de = jo.TryGetValue("de", out jt) ? (string)jt : null;
            pe = jo.TryGetValue("pe", out jt) ? (string)jt : null;
            sz = jo.TryGetValue("sz", out jt) ? (string)jt : null;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("股票代码：{0}\r\n当前价：{1}\r\n涨幅：{2}\r\n涨幅比率：{3}\r\n" +
                "最高价：{4}\r\n最低价：{5}\r\n成交量：{6}\r\n成交额：{7}\r\n" +
                "市盈率：{8}\r\n市值：{9}",
                cd ?? "", np ?? "", ap ?? "", apn ?? "",
                tp_max ?? "", tp_min ?? "", dn ?? "", de ?? "",
                pe ?? "", sz ?? "");
        }
    }
}