using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    public class ArticleSummary : ArticleData
    {
        /// <summary>
        /// 数据的日期
        /// </summary>
        public DateTime ref_date { get; private set; }
        /// <summary>
        /// 消息ID：实际上是由msgid（图文消息id）和index（消息次序索引）组成， 例如12003_3， 其中12003是msgid，即一次群发的id消息的； 3为index，假设该次群发的图文消息共5个文章（因为可能为多图文）， 3表示5个中的第3个
        /// </summary>
        public string msgid { get; private set; }
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; private set; }

        /// <summary>
        /// 获取图文消息id
        /// </summary>
        public long ArticleMsgId
        {
            get
            {
                long id = -1;
                if (!string.IsNullOrWhiteSpace(msgid))
                {
                    string[] arr = msgid.Split('_');
                    if (!long.TryParse(arr[0], out id))
                        id = -1;
                }
                return id;
            }
        }

        /// <summary>
        /// 获取消息次序索引
        /// </summary>
        public int Index
        {
            get
            {
                int index = -1;
                if (!string.IsNullOrWhiteSpace(msgid))
                {
                    string[] arr = msgid.Split('_');
                    if (arr.Length > 1)
                        if (!int.TryParse(arr[1], out index))
                            index = -1;
                }
                return index;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleSummary()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public override void Parse(JObject jo)
        {
            base.Parse(jo);
            ref_date = DateTime.Parse((string)jo["ref_date"]);
            msgid = (string)jo["msgid"];
            title = (string)jo["title"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("数据日期：{0:yyyy-MM-dd}\r\n图文消息ID：{1}\r\n消息次序索引：{2}\r\n标题：{3}\r\n{4}",
                ref_date, ArticleMsgId, Index, title, base.ToString());
        }
    }
}
