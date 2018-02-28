using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount.Semantic
{
    /// <summary>
    /// 数字协议
    /// </summary>
    public class NumberProtocol : CommonProtocol
    {
        /// <summary>
        /// 无上限或下限
        /// </summary>
        public const int INFINITY = -1;
        /// <summary>
        /// 字段无信息
        /// </summary>
        public const int NO_INFO = -2;

        /// <summary>
        /// 开始
        /// </summary>
        public int begin { get; private set; }
        /// <summary>
        /// 结束
        /// </summary>
        public int end { get; private set; }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            begin = (int)jo["begin"];
            end = (int)jo["end"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}\r\n开始：{1}\r\n结束：{2}",
                base.ToString(), begin, end);
        }
    }
}
