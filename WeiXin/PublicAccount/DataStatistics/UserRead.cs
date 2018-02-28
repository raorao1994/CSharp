using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 图文统计数据
    /// </summary>
    public class UserRead : ArticleData
    {
        /// <summary>
        /// 获取数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UserRead()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            ref_date = DateTime.Parse((string)jo["ref_date"]);
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n{1}", ref_date, base.ToString());
        }
    }
}