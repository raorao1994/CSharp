using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 用户累计数据
    /// </summary>
    public class UserCumulate : IParsable
    {
        /// <summary>
        /// 获取数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 获取总用户量
        /// </summary>
        public int cumulate_user { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserCumulate()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refDate">数据日期</param>
        /// <param name="cumulateUser">总用户量</param>
        internal UserCumulate(DateTime refDate, int cumulateUser)
        {
            ref_date = refDate;
            cumulate_user = cumulateUser;
        }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public void Parse(JObject jo)
        {
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            cumulate_user = (int)jo["cumulate_user"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n总用户量：{1}",
                ref_date, cumulate_user);
        }
    }
}