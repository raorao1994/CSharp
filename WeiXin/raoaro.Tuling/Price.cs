using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 价格
    /// </summary>
    public class Price
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string price { get; private set; }
        /// <summary>
        /// 详情地址
        /// </summary>
        public string DetailUrl { get; private set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="price">价格</param>
        /// <param name="detailUrl">详情地址</param>
        /// <param name="icon">图标地址</param>
        internal Price(string name, string price, string detailUrl, string icon)
        {
            Name = name;
            this.price = price;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal Price(JObject jo)
            : this((string)jo["name"], (string)jo["price"], (string)jo["detailurl"], (string)jo["icon"])
        { }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("名称：{0}\r\n价格：{1}\r\n详情地址：{2}\r\n图标地址：{3}",
                Name, price, DetailUrl, Icon);
        }
    }
}