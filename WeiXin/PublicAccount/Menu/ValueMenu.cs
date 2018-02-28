using System;

namespace PublicAccount
{
    /// <summary>
    /// 带值的菜单
    /// </summary>
    public class ValueMenu : BaseMenu
    {
        /// <summary>
        /// 菜单类型
        /// </summary>
        private MenuTypeEnum _type;
        /// <summary>
        /// 值
        /// </summary>
        private string _value;

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
                if (!(_type == MenuTypeEnum.text || _type == MenuTypeEnum.img ||
                    _type == MenuTypeEnum.video || _type == MenuTypeEnum.voice))
                    throw new ArgumentException("菜单类型错误。", "type");
                _type = value;
            }
        }
        /// <summary>
        /// 键
        /// </summary>
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "菜单值不能为null。");
                _value = value;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        internal ValueMenu(MenuTypeEnum type, string name, string value)
        {
            this.type = type;
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public override object ToAnonymousObject()
        {
            return new { type = type.ToString("g"), name = name, value = value };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("菜单名称：{0}\r\n菜单类型：{1:g}\r\n菜单值：{2}",
                name, type, value);
        }
    }
}