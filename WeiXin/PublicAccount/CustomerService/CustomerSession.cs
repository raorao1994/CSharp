using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 客户的会话状态
    /// </summary>
    public class CustomerSession : IParsable
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 会话接入的时间
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerSession()
        { }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            kf_account = (string)jo["kf_account"];
            time = (int)jo["time"];
        }

        /// <summary>
        /// 获取接入会话的时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetTime()
        {
            return Utility.ToDateTime(time);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("客服账号：{0}\r\n接入会话的时间：{1}",
                kf_account, GetTime());
        }
    }
}