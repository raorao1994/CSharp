using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 图文统计分时数据
    /// </summary>
    public class UserReadHour : UserRead
    {
        /// <summary>
        /// 数据的小时，包括从000到2300，分别代表的是[000,100)到[2300,2400)，即每日的第1小时和最后1小时
        /// </summary>
        public int ref_hour { get; private set; }

        /// <summary>
        /// 数据的小时（24小时制）
        /// </summary>
        public int Hour
        {
            get
            {
                return ref_hour / 100;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserReadHour()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            ref_hour = (int)jo["ref_hour"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据小时：{0}\r\n{1}", Hour, base.ToString());
        }
    }
}