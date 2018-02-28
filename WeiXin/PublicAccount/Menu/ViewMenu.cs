using System;

namespace PublicAccount
{
    /// <summary>
    /// 跳转URL菜单
    /// </summary>
    public class ViewMenu : BaseMenu
    {
        /// <summary>
        /// 菜单类型
        /// </summary>
        private MenuTypeEnum _type;
        /// <summary>
        /// 跳转地址
        /// </summary>
        private string _url;

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
        /// 跳转地址
        /// </summary>
        public string url
        {
            get
            {
                return _url;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("url", "菜单跳转地址不能为空。");
                _url = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">名称</param>
        /// <param name="key">跳转地址</param>
        internal ViewMenu(string name, string url)
        {
            _type = MenuTypeEnum.view;
            this.name = name;
            this.url = url;
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public override object ToAnonymousObject()
        {
            return new { type = type.ToString("g"), name = name, url = url };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("菜单名称：{0}\r\n菜单类型：{1:g}\r\n跳转地址：{2}",
                name, type, url);
        }
    }
}