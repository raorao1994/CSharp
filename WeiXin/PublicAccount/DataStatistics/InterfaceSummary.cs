using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 接口分析数据
    /// </summary>
    public class InterfaceSummary : IParsable
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 通过服务器配置地址获得消息后，被动回复用户消息的次数
        /// </summary>
        public int callback_count { get; private set; }
        /// <summary>
        /// 失败次数
        /// </summary>
        public int fail_count { get; private set; }
        /// <summary>
        /// 总耗时（毫秒）
        /// </summary>
        public long total_time_cost { get; private set; }
        /// <summary>
        /// 最大耗时（毫秒）
        /// </summary>
        public int max_time_cost { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public InterfaceSummary()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            callback_count = (int)jo["callback_count"];
            fail_count = (int)jo["fail_count"];
            total_time_cost = (long)jo["total_time_cost"];
            max_time_cost = (int)jo["max_time_cost"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n被动回复消息次数：{1}\r\n失败次数：{2}\r\n总耗时：{3}\r\n最大耗时：{4}",
                ref_date, callback_count, fail_count, total_time_cost, max_time_cost);
        }
    }
}