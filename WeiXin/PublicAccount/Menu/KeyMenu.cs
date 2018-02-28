using System;

namespace PublicAccount
{
    /// <summary>
    /// 带键的菜单
    /// </summary>
    public class KeyMenu:BaseMenu
    {
        /// <summary>
        /// 菜单类型
        /// </summary>
        private MenuTypeEnum _type;
        /// <summary>
        /// 键
        /// </summary>
        private string _key;

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuTypeEnum type
        {
            get
            {
                return _type;
            }
            set
            {
                if (!(_type == MenuTypeEnum.click || _type == MenuTypeEnum.scancode_push || 
                    _type == MenuTypeEnum.scancode_waitmsg || _type == MenuTypeEnum.pic_sysphoto || 
                    _type == MenuTypeEnum.pic_photo_or_album || _type == MenuTypeEnum.pic_weixin || 
                    _type == MenuTypeEnum.location_select))
                    throw new ArgumentException("菜单类型错误。", "type");
                _type = value;
            }
        }
        /// <summary>
        /// 键
        /// </summary>
        public string key
        {
            get
            {
                return _key;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("key", "菜单键不能为空。");
                _key=value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">名称</param>
        /// <param name="key">键</param>
        internal KeyMenu(MenuTypeEnum type,string name,string key)
        {
            this.type = type;
            this.name = name;
            this.key = key;
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public override object ToAnonymousObject()
        {
            return new { type = type.ToString("g"), name = name, key = key };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("菜单名称：{0}\r\n菜单类型：{1:g}\r\n菜单键：{2}",
                name, type, key);
        }
    }
}
