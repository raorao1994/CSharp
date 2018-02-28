using System;
using System.Text;

namespace PublicAccount
{
    /// <summary>
    /// 图文消息菜单
    /// </summary>
    public class NewsMenu : BaseMenu
    {
        /// <summary>
        /// 菜单类型
        /// </summary>
        private MenuTypeEnum _type;
        /// <summary>
        /// 图文消息
        /// </summary>
        private NewsForMenu[] _news_info;

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum type
        {
            get
            {
                return _type;
            }
        }
        /// <summary>
        /// 键
        /// </summary>
        public NewsForMenu[] news_info
        {
            get
            {
                return _news_info;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("news_info", "图文消息不能为空。");
                _news_info = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">名称</param>
        /// <param name="news_info">图文消息</param>
        internal NewsMenu(string name, NewsForMenu[] news_info)
        {
            _type = MenuTypeEnum.news;
            this.name = name;
            this.news_info = news_info;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">名称</param>
        /// <param name="news_info">图文消息</param>
        internal NewsMenu(string name, NewsForMenu news)
            : this(name, new NewsForMenu[] { news })
        { }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public override object ToAnonymousObject()
        {
            return new { type = type.ToString("g"), name = name, news_info = new { list = news_info } };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("菜单名称：{0}\r\n", name);
            sb.AppendFormat("菜单类型：{1:g}\r\n", type);
            sb.AppendFormat("图文消息数：{0}", news_info.Length);
            if (news_info.Length > 0)
            {
                for (int i = 0; i < news_info.Length; i++)
                    sb.AppendFormat("\r\n图文{0}：{1}", i + 1, news_info[i]);
            }
            return sb.ToString();
        }
    }
}
