using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    public class UpstreamMsgDist : IParsable
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 发送消息量分布的区间，0代表 “0”，1代表“1-5”，2代表“6-10”，3代表“10次以上”
        /// </summary>
        public UpstreamMsgCountIntervalEnum count_interval { get; private set; }
        /// <summary>
        /// 上行消息的用户数
        /// </summary>
        public int msg_user { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpstreamMsgDist()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            count_interval = (UpstreamMsgCountIntervalEnum)(int)jo["count_interval"];
            msg_user = (int)jo["msg_user"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n分布区间：{1:g}\r\n用户数：{2}",
                ref_date, count_interval, msg_user);
        }
    }
}