using System;
using System.Text;
using System.Collections.Generic;

namespace PublicAccount
{
    /// <summary>
    /// 菜单容器，最多可以包含5项子菜单
    /// </summary>
    public class MenuContainer : BaseMenu
    {
        /// <summary>
        /// 菜单容器的最大容量
        /// </summary>
        private const int maxSubmenuCount = 5;
        /// <summary>
        /// 子菜单
        /// </summary>
        private List<BaseMenu> _submenus;

        /// <summary>
        /// 获取子菜单的数目
        /// </summary>
        public int Count
        {
            get
            {
                return _submenus.Count;
            }
        }

        /// <summary>
        /// 获取或者设置指定位置的子菜单
        /// </summary>
        /// <param name="index">子菜单的位置</param>
        /// <returns>返回指定位置的子菜单</returns>
        public BaseMenu this[int index]
        {
            get
            {
                return _submenus[index];
            }
            set
            {
                if (value is KeyMenu || value is ViewMenu)
                    _submenus[index] = value;
                else
                    throw new ArgumentException("子菜单类型不正确。", "submenu");
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        internal MenuContainer(string name)
        {
            this.name = name;
            _submenus=new List<BaseMenu>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="submenus">子菜单</param>
        internal MenuContainer(string name,IEnumerable<BaseMenu> submenus)
            :this(name)
        {
            Add(submenus);
        }

        /// <summary>
        /// 添加子菜单
        /// </summary>
        /// <param name="submenu">子菜单</param>
        /// <returns>返回添加是否成功</returns>
        public bool Add(BaseMenu submenu)
        {
            bool success = false;
            if (_submenus.Count < maxSubmenuCount && (submenu is KeyMenu || submenu is ViewMenu) && !_submenus.Contains(submenu))
            {
                _submenus.Add(submenu);
                success = true;
            }
            return success;
        }

        /// <summary>
        /// 添加一组子菜单
        /// </summary>
        /// <param name="submenus">子菜单</param>
        /// <returns>返回成功添加的子菜单数目</returns>
        public int Add(IEnumerable<BaseMenu> submenus)
        {
            int count = 0;
            foreach (BaseMenu submenu in submenus)
            {
                if (Add(submenu))
                    count++;
            }
            return count;
        }

        /// <summary>
        /// 清除子菜单
        /// </summary>
        public void Clear()
        {
            _submenus.Clear();
        }

        /// <summary>
        /// 删除子菜单
        /// </summary>
        /// <param name="submenu">子菜单</param>
        /// <returns>返回删除是否成功</returns>
        public bool Remove(BaseMenu submenu)
        {
            return _submenus.Remove(submenu);
        }

        /// <summary>
        /// 删除指定位置的子菜单
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            _submenus.RemoveAt(index);
        }

        /// <summary>
        /// 返回匿名对象
        /// </summary>
        /// <returns></returns>
        public override object ToAnonymousObject()
        {
            object[] subbuttons = new object[_submenus.Count];
            for (int idx = 0; idx < subbuttons.Length; idx++)
                subbuttons[idx] = _submenus[idx].ToAnonymousObject();
            return new { name = name, sub_button = subbuttons };
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("菜单名称：{0}\r\n菜单类型：菜单容器\r\n子菜单数目：{1}",
                name, Count);
            if(Count>0)
            {
                for(int i=0;i<Count;i++)
                    sb.AppendFormat("\r\n子菜单{0}：\r\n{1}", i, _submenus[i]);
            }
            return sb.ToString();
        }
    }
}
