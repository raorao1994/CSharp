using System;
using System.Collections.Generic;

namespace PublicAccount
{
    /// <summary>
    /// 客服聊天记录
    /// </summary>
    public class CustomerServiceRecord
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string worker { get; set; }
        /// <summary>
        /// 用户标志
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 操作ID（会话状态）
        /// </summary>
        public int opercode { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public int time { get; set; }
        /// <summary>
        /// 聊天记录
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="openid"></param>
        /// <param name="opercode"></param>
        /// <param name="time"></param>
        /// <param name="text"></param>
        public CustomerServiceRecord(string worker, string openid, int opercode, int time, string text)
        {
            this.worker = worker;
            this.openid = openid;
            this.opercode = opercode;
            this.time = time;
            this.text = text;
        }

        /// <summary>
        /// 获取操作时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetTime()
        {
            return Utility.ToDateTime(time);
        }

        /// <summary>
        /// 获取操作（会话状态）
        /// </summary>
        public CustomerServiceOperateEnum GetOperate()
        {
            return (CustomerServiceOperateEnum)opercode;
        }
    }
}
