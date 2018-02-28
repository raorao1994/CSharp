using System;

namespace PublicAccount
{
    /// <summary>
    /// 微信菜单基类
    /// </summary>
    public class BaseMenu : IEquatable<BaseMenu>
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _name;
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("name", "菜单名称不能为空。");
                _name = value;
            }
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public virtual object ToAnonymousObject()
        {
            return new { name = name };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("菜单名称：{0}", name);
        }

        /// <summary>
        /// 判断两项菜单是否相等
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BaseMenu other)
        {
            if (other == null)
                return false;

            if (this.name == other.name)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 判断两项菜单是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            BaseMenu other = obj as BaseMenu;
            if (other == null)
                return false;
            else
                return Equals(other);
        }

        /// <summary>
        /// 获取哈希码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }

        /// <summary>
        /// 相等比较器
        /// </summary>
        /// <param name="menu1"></param>
        /// <param name="menu2"></param>
        /// <returns></returns>
        public static bool operator ==(BaseMenu menu1, BaseMenu menu2)
        {
            if ((object)menu1 == null || (object)menu2 == null)
                return Object.Equals(menu1, menu2);

            return menu1.Equals(menu2);
        }

        /// <summary>
        /// 不相等比较器
        /// </summary>
        /// <param name="menu1"></param>
        /// <param name="menu2"></param>
        /// <returns></returns>
        public static bool operator !=(BaseMenu menu1, BaseMenu menu2)
        {
            if (menu1 == null || menu2 == null)
                return !Object.Equals(menu1, menu2);

            return !(menu1.Equals(menu2));
        }
    }
}