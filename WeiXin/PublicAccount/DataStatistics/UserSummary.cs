using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 用户增减数据
    /// </summary>
    public class UserSummary : IParsable
    {
        /// <summary>
        /// 获取数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 获取用户来源
        /// </summary>
        public UserSourceEnum user_source { get; private set; }
        /// <summary>
        /// 获取新增的用户数量
        /// </summary>
        public int new_user { get; private set; }
        /// <summary>
        /// 获取取消关注的用户数量
        /// </summary>
        public int cancel_user { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserSummary()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refDate">数据日期</param>
        /// <param name="userSource">用户来源</param>
        /// <param name="newUser">新增的用户数量</param>
        /// <param name="cancelUser">取消关注的用户数量</param>
        internal UserSummary(DateTime refDate, UserSourceEnum userSource, int newUser, int cancelUser)
        {
            ref_date = refDate;
            user_source = userSource;
            new_user = newUser;
            cancel_user = cancelUser;
        }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            user_source = (UserSourceEnum)(int)jo["user_source"];
            new_user = (int)jo["new_user"];
            cancel_user = (int)jo["cancel_user"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n用户来源：{1:g}\r\n新增用户数量：{2}\r\n取消关注用户数量：{3}",
                ref_date, user_source, new_user, cancel_user);
        }
    }
}
