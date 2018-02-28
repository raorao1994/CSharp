using System;

namespace PublicAccount
{
    /// <summary>
    /// 用户分组
    /// </summary>
    public class UserGroup
    {
        /// <summary>
        /// 分组id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 分组名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 分组内用户数量
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">分组id</param>
        /// <param name="name">分组名称</param>
        /// <param name="count">分组内用户数量</param>
        public UserGroup(int id, string name, int count)
        {
            this.id = id;
            this.name = name;
            this.count = count;
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("分组id：{0}\r\n分组名称：{1}\r\n分组内用户数量：{2}",
                id, name, count);
        }
    }
}
