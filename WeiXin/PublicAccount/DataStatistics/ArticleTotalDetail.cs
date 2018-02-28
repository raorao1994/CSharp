using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 图文群发总数据中的详细数据
    /// </summary>
    public class ArticleTotalDetail : ArticleData
    {
        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime stat_date { get; private set; }
        /// <summary>
        /// 送达人数，一般约等于总粉丝数（需排除黑名单或其他异常情况下无法收到消息的粉丝）
        /// </summary>
        public int target_user { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleTotalDetail()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            stat_date = DateTime.Parse((string)jo["stat_date"]);
            target_user = (int)jo["target_user"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("统计日期：{0:yyyy-MM-dd}\r\n送达人数：{1}\r\n{2}",
                stat_date, target_user, base.ToString());
        }
    }
}