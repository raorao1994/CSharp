using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 消息发送概况数据
    /// </summary>
    public class UpstreamMsg : IParsable
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public UpstreamMsgTypeEnum msg_type { get; private set; }
        /// <summary>
        /// 上行（向公众号发送了）消息的用户数
        /// </summary>
        public int msg_user { get; private set; }
        /// <summary>
        /// 上行消息总数
        /// </summary>
        public int msg_count { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpstreamMsg()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            msg_type = (UpstreamMsgTypeEnum)(int)jo["msg_type"];
            msg_user = (int)jo["msg_user"];
            msg_count = (int)jo["msg_count"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n消息类型：{1:g}\r\n上行消息的人数：{2}\r\n上行消息总数：{3}",
                ref_date, msg_type, msg_user, msg_count);
        }
    }
}