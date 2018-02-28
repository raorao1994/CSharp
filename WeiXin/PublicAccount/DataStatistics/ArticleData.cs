using System;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 图文分析数据接口中的图文数据，包括：阅读、分享、收藏数据
    /// </summary>
    public class ArticleData : IParsable
    {
        /// <summary>
        /// 图文页（点击群发图文卡片进入的页面）的阅读人数
        /// </summary>
        public int int_page_read_user { get; private set; }
        /// <summary>
        /// 图文页的阅读次数
        /// </summary>
        public int int_page_read_count { get; private set; }
        /// <summary>
        /// 原文页（点击图文页“阅读原文”进入的页面）的阅读人数，无原文页时此处数据为0
        /// </summary>
        public int ori_page_read_user { get; private set; }
        /// <summary>
        /// 原文页的阅读次数
        /// </summary>
        public int ori_page_read_count { get; private set; }
        /// <summary>
        /// 分享的人数
        /// </summary>
        public int share_user { get; private set; }
        /// <summary>
        /// 分享的次数
        /// </summary>
        public int share_count { get; private set; }
        /// <summary>
        /// 收藏的人数
        /// </summary>
        public int add_to_fav_user { get; private set; }
        /// <summary>
        /// 收藏的次数
        /// </summary>
        public int add_to_fav_count { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ArticleData()
        { }

        /// <summary>
        /// 从JObject对象解析统计数据
        /// </summary>
        /// <param name="jo"></param>
        public virtual void Parse(JObject jo)
        {
            int_page_read_user = (int)jo["int_page_read_user"];
            int_page_read_count = (int)jo["int_page_read_count"];
            ori_page_read_user = (int)jo["ori_page_read_user"];
            ori_page_read_count = (int)jo["ori_page_read_count"];
            share_user = (int)jo["share_user"];
            share_count = (int)jo["share_count"];
            add_to_fav_user = (int)jo["add_to_fav_user"];
            add_to_fav_count = (int)jo["add_to_fav_count"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("图文阅读人数：{0}\r\n图文阅读次数：{1}\r\n原文阅读人数：{2}\r\n原文阅读次数：{3}\r\n" +
            "分享人数：{4}\r\n分享次数：{5}\r\n收藏人数：{6}\r\n收藏次数：{7}",
                int_page_read_user, int_page_read_count, ori_page_read_user, ori_page_read_count,
            share_user, share_count, add_to_fav_user, add_to_fav_count);
        }
    }
}
