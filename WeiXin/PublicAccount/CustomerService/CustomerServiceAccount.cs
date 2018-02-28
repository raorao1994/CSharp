using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 客服账号
    /// </summary>
    public class CustomerServiceAccount : IParsable
    {
        /// <summary>
        /// 客服账号
        /// </summary>
        public string kf_account { get; set; }
        /// <summary>
        /// 客服昵称
        /// </summary>
        public string kf_nick { get; set; }
        /// <summary>
        /// 客服工号
        /// </summary>
        public string kf_id { get; set; }
        /// <summary>
        /// 客服头像的地址
        /// </summary>
        public string kf_headimg { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomerServiceAccount()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nick"></param>
        /// <param name="id"></param>
        /// <param name="headimg"></param>
        public CustomerServiceAccount(string account, string nick, string id, string headimg)
        {
            kf_account = account;
            kf_nick = nick;
            kf_id = id;
            kf_headimg = headimg;
        }

        /// <summary>
        /// 从JObject对象解析
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            kf_account = (string)jo["kf_account"];
            kf_nick = (string)jo["kf_nick"];
            kf_id = (string)jo["kf_id"];
            kf_headimg = (string)jo["kf_headimg"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("客服账号：{0}\r\n昵称：{1}\r\n工号：{2}\r\n头像：{3}",
                kf_account, kf_nick, kf_id, kf_headimg);
        }
    }
}
