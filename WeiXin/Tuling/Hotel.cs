using System;
using Newtonsoft.Json.Linq;

namespace Tuling
{
    /// <summary>
    /// 酒店
    /// </summary>
    public class Hotel
    {
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; private set; }
        /// <summary>
        /// 满意度
        /// </summary>
        public string Satisfaction { get; private set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Count { get; private set; }
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
        /// <param name="satisfaction">满意度</param>
        /// <param name="count">数量</param>
        /// <param name="detailUrl">详情地址</param>
        /// <param name="icon">图标地址</param>
        internal Hotel(string name, string price, string satisfaction, string count, string detailUrl, string icon)
        {
            Name = name;
            Price = price;
            Satisfaction = satisfaction;
            Count = count;
            DetailUrl = detailUrl;
            Icon = icon;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="jo">jobject对象</param>
        internal Hotel(JObject jo)
        {
            Name = (string)jo["name"];
            Price = (string)jo["price"];
            Satisfaction = (string)jo["satisfaction"];
            JToken jt;
            Count = jo.TryGetValue("count", out jt) ? (string)jt : string.Empty;
            DetailUrl = (string)jo["detailurl"];
            Icon = (string)jo["icon"];
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("酒店名称：{0}\r\n价格：{1}\r\n满意度：{2}\r\n数量：{3}\r\n详情地址：{4}\r\n图标地址：{5}",
                Name, Price, Satisfaction, Count, DetailUrl, Icon);
        }
    }
}